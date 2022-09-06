using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract partial class PassiveSkill : BaseSkill
            {
                public const float Infinite_Duration = 0.0f;

                #region Field
                private float duration;
                protected bool isTargetingAll;
                protected RESTRICTION_FLAG restrictionFlag;
                protected SKILL_REMAINTIME_TYPE remainTimeType;
                protected int maxAreaStack;
                protected bool isUsingTick;
                protected float tickCycleTime;
                protected int maxTickStack;
                protected bool isInf;

                protected Dictionary<int, Unit> unitsInEffect = new Dictionary<int, Unit>();
                protected HashSet<Unit> newUnits = new HashSet<Unit>();
                protected HashSet<int> removeUnitIDs = new HashSet<int>();

                public delegate void OnPassiveEffectEvent(Unit unit, int stack);
                public event OnPassiveEffectEvent OnBeginPassiveEffectEvent;
                public event OnPassiveEffectEvent OnEndPassiveEffectEvent;
                public event OnPassiveEffectEvent OnTickPassiveEffectEvent;
                public event Delegate_NoRetVal_NoParam OnEmptyInEffectEvent;
                #endregion

                #region Properties
                public override bool IsUsingCritical => false;
                public RESTRICTION_FLAG RestrictionFlag => restrictionFlag;
                public int TypeID => GetType().GetHashCode();
                protected float Duration
                {
                    get => duration;
                    set
                    {
                        duration = value;
                        isInf = duration == Infinite_Duration;
                    }
                }
                #endregion

                #region Methods
                protected PassiveSkill(PassiveSkillData data, Unit skillOwner) : base(data, skillOwner)
                {
                    Duration = data.duration;
                    isTargetingAll = data.isTargetingAll;
                    restrictionFlag = data.restrictionFlag;
                    remainTimeType = data.remainTimeType;
                    maxAreaStack = data.maxAreaStack;
                    isUsingTick = data.isUsingTick;
                    tickCycleTime = data.tickCycleTime;
                    maxTickStack = data.maxTickStack;

                    caster.StartCoroutine(UpdatePassiveEffect());
                    if (isUsingTick)
                        caster.StartCoroutine(UpdateTickEffect());

                    Active = true;
                }
                /// <summary>
                /// 패시브 스킬 대상으로 unit을 추가
                /// </summary>
                /// <param name="unit">추가될 대상</param>
                public virtual void AddPassiveSkillToUnit(Unit unit)
                {
                    if (!Active)
                        return;

                    newUnits.Add(unit);
                }
                /// <summary>
                /// 패시브 스킬 제외 대상으로 unit을 추가
                /// </summary>
                /// <param name="unit">제외될 대상</param>
                public virtual void RemovePassiveSkillToUnit(Unit unit)
                {
                    int id = unit.GetInstanceID();
                    if (unitsInEffect.ContainsKey(id))
                        removeUnitIDs.Add(id);
                }

                /// <summary>
                /// 패시브 효과 범위 내에 있는 유닛들을 구한다
                /// </summary>
                protected virtual void FindUnitsInEffectRange()
                {
                    if (isTargetingAll)
                    {
                        var units = caster.gm.GetAllUnits();
                        foreach (var unit in units)
                            if (Utility.HasFlag(TargetLayerMask, unit.gameObject.layer) && Utility.HasFlag((int)TargetType, 1 << (int)unit.UnitType))
                                newUnits.Add(unit);
                    }
                    else if (inRangeHitboxes.Length != 0)
                    {
                        var hits = Utility.SweepUnit(inRangeHitboxes, caster.transform.position, Quaternion.identity, true, TargetLayerMask, TargetType);
                        foreach (var hit in hits)
                            newUnits.Add(hit.transform.GetComponent<Unit>());
                    }
                }
                /// <summary>
                /// unit이 패시브 효과 적용 대상인가?
                /// </summary>
                /// <param name="unit">대상</param>
                /// <returns>효과 적용 대상이면 true, 아니면 false</returns>
                public virtual bool IsUnitInPassive(Unit unit)
                {
                    if (unitsInEffect.ContainsKey(unit.GetInstanceID()))
                        return isInf || unit.curPSkills[TypeID].remainTime > 0.0f;

                    return false;
                }
                /// <summary>
                /// 패시브 효과에 적용될 때 효과
                /// </summary>
                /// <param name="unit">적용 대상</param>
                /// <param name="areaStack">영역 중첩 스택</param>
                protected virtual void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    OnBeginPassiveEffectEvent?.Invoke(unit, areaStack);
                }
                /// <summary>
                /// 패시브 효과에서 벗어날 때 효과
                /// </summary>
                /// <param name="unit">적용 대상</param>
                /// <param name="areaStack">영역 중첩 스택</param>
                protected virtual void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    OnEndPassiveEffectEvent?.Invoke(unit, areaStack);
                }
                /// <summary>
                /// 패시브 효과에 유닛들이 모두 없어졌을때(있다가 없어졌을때)
                /// </summary>
                protected virtual void OnEmptyInEffect()
                {
                    OnEmptyInEffectEvent?.Invoke();
                }
                /// <summary>
                /// 매 틱마다 적용 효과
                /// </summary>
                /// <param name="unit">대상</param>
                /// <param name="tickStack">틱 스택</param>
                protected virtual void OnTickPassiveEffect(Unit unit, int tickStack)
                {
                    OnTickPassiveEffectEvent?.Invoke(unit, tickStack);
                }
                private void CleanUpRemoveUnits()
                {
                    PassiveSkillStateData unitPSkillData;

                    // 삭제리스트 정리
                    foreach (var removeUnitID in removeUnitIDs)
                    {
                        Unit unit = unitsInEffect[removeUnitID];
                        unitPSkillData = unit.curPSkills[TypeID];
                        --unitPSkillData.areaStack;
                        unit.curPSkills[TypeID] = unitPSkillData;

                        OnEndPassiveEffect(unit, unitPSkillData.areaStack);

                        if (unitPSkillData.areaStack == 0)
                            unit.curPSkills.Remove(TypeID);

                        unitsInEffect.Remove(removeUnitID);
                        if (unitsInEffect.Count == 0)
                        {
                            OnEmptyInEffect();
                            break;
                        }
                    }
                    removeUnitIDs.Clear();
                }
                private IEnumerator UpdatePassiveEffect()
                {
                    WaitForSeconds wf100ms = CachedWaitForSeconds.Get(0.1f);

                    while (true)
                    {
                        PassiveSkillStateData unitPSkillData;

                        if (isInf)
                            foreach (var unitInEffectID in unitsInEffect.Keys)
                                removeUnitIDs.Add(unitInEffectID);

                        if (Active)
                        {
                            FindUnitsInEffectRange();

                            foreach (var unit in newUnits)
                            {
                                var id = unit.GetInstanceID();

                                // 현재 스킬에 영향받고 있던 중인지
                                if (!unitsInEffect.ContainsKey(id))
                                {
                                    // 대상에게 같은 스킬의 영향을 받고있는가?
                                    if (unit.curPSkills.ContainsKey(TypeID))
                                    {
                                        // 받고있다면 중복카운트 증가
                                        unitPSkillData = unit.curPSkills[TypeID];

                                        // 최대 중첩중이라면 다음유닛으로
                                        if (unitPSkillData.areaStack == maxAreaStack)
                                            continue;
                                        
                                        ++unitPSkillData.areaStack;
                                    }
                                    else
                                    {
                                        // 안받고 있다면 새로운 데이터 추가
                                        unitPSkillData = new PassiveSkillStateData(this, 1, 0, remainTimeType == SKILL_REMAINTIME_TYPE.NONE ? Duration : 0.0f);
                                        unit.curPSkills.Add(TypeID, unitPSkillData);
                                    }

                                    unitsInEffect.Add(id, unit);

                                    OnBeginPassiveEffect(unit, unitPSkillData.areaStack);
                                }
                                else
                                {
                                    unitPSkillData = unit.curPSkills[TypeID];

                                    // 지속시간이 무한대일 시 삭제리스트에 영향 받던 유닛들이 들어있으므로 영향 범위 안에 또 존재하는 유닛들에 대해 삭제리스트에서 제거
                                    if (isInf)
                                        removeUnitIDs.Remove(id);
                                }

                                switch (remainTimeType)
                                {
                                    case SKILL_REMAINTIME_TYPE.RESET:
                                        unitPSkillData.remainTime = Duration;
                                        break;
                                    case SKILL_REMAINTIME_TYPE.ADD:
                                        unitPSkillData.remainTime += Duration;
                                        break;
                                }

                                unit.curPSkills[TypeID] = unitPSkillData;
                            }

                            newUnits.Clear();
                        }

                        // 지속시간이 무한대라면 바로 삭제
                        if (isInf)
                            CleanUpRemoveUnits();

                        yield return wf100ms;

                        if (!isInf)
                        {
                            foreach (var unit in unitsInEffect.Values)
                            {
                                unitPSkillData = unit.curPSkills[TypeID];
                                unitPSkillData.remainTime -= 0.1f;
                                if (unitPSkillData.remainTime < 0.0f)
                                    unitPSkillData.remainTime = 0.0f;
                                unit.curPSkills[TypeID] = unitPSkillData;
                            }

                            foreach (var unit in unitsInEffect.Values)
                                // unit이 패시브 효과에서 벗어났다면
                                if (!IsUnitInPassive(unit))
                                    // 삭제리스트에 추가
                                    removeUnitIDs.Add(unit.GetInstanceID());

                            // 삭제리스트 정리
                            CleanUpRemoveUnits();
                        }
                    }
                }
                private IEnumerator UpdateTickEffect()
                {
                    WaitForSeconds waitCycleTime = CachedWaitForSeconds.Get(tickCycleTime);
                    while (true)
                    {
                        foreach (var unit in unitsInEffect.Values)
                        {
                            var unitPSkillData = unit.curPSkills[TypeID];
                            if (unitPSkillData.tickStack != maxTickStack)
                            {
                                var t = unit.curPSkills[TypeID];
                                ++t.tickStack;
                                unit.curPSkills[TypeID] = t;
                            }
                            OnTickPassiveEffect(unit, unit.curPSkills[TypeID].tickStack);
                        }

                        yield return waitCycleTime;
                    }
                }
                #endregion
            }

            [System.Serializable]
            public abstract class PassiveSkillData : BaseSkillData
            {
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("지속시간 설정 사용")]
                protected bool PP_UseDuration = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("상태이상 설정 사용")]
                protected bool PP_UseRestrictionFlag = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("갱신타입 설정 사용")]
                protected bool PP_UseRemainTimeType = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("중첩 설정 사용")]
                protected bool PP_UseAreaStack = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("틱 설정 사용")]
                protected bool PP_UseTick = true;

#if UNITY_EDITOR
                [ShowIf("PP_UseDuration")]
#endif
                [FoldoutGroup("PassiveSkill", 4), Min(0.0f)]
                [LabelText("패시브 효과 지속시간"), Tooltip("패시브 효과의 지속시간입니다.\n0일시 무한대의 지속시간을 갖습니다.")]
                public float duration;
#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [FoldoutGroup("PassiveSkill")]
                [LabelText("모든 유닛 대상"), Tooltip("true : 모든 유닛들을 대상으로 지정합니다.\nfalse : 히트박스 내의 유닛들을 패시브스킬 대상으로 지정합니다.")]
                public bool isTargetingAll;
#if UNITY_EDITOR
                [ShowIf("PP_UseRestrictionFlag")]
#endif
                [FoldoutGroup("PassiveSkill"), EnumToggleButtons]
                [LabelText("상태이상"), Tooltip("패시브 스킬 지속 효과동안 어떤 상태이상에 걸리게 할지")]
                public RESTRICTION_FLAG restrictionFlag;
#if UNITY_EDITOR
                [ShowIf("PP_UseRemainTimeType")]
#endif
                [FoldoutGroup("PassiveSkill"), EnumToggleButtons]
                [LabelText("갱신 타입"), Tooltip("갱신 타입 설정\nNONE : 타이머를 초기화시키지 않는다.\nRESET : 타이머를 초기화 시킨다.\nADD : 남은 시간에 원래 지속시간만큼 추가한다.")]
                public SKILL_REMAINTIME_TYPE remainTimeType;
#if UNITY_EDITOR
                [ShowIf("PP_UseAreaStack")]
#endif
                [FoldoutGroup("PassiveSkill"), Min(1)]
                [LabelText("최대 영역 중첩 개수"), Tooltip("영역 중첩 효과 최대 개수\n중첩된 수만큼 틱 이벤트도 배로 증가합니다.")]
                public int maxAreaStack = 1;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill")]
                [LabelText("틱 이벤트 사용"), Tooltip("패시브 스킬 효과동안 일정 시간마다 특정 효과가 발동되게 할지")]
                public bool isUsingTick;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill"), EnableIf("isUsingTick")]
                [LabelText("틱 사이클(초)"), Tooltip("얼마만큼의 간격으로 틱 이벤트가 발동할지")]
                public float tickCycleTime;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill"), EnableIf("isUsingTick"), Min(1)]
                [LabelText("최대 틱 스택"), Tooltip("틱 스택 최대 크기\n패시브 효과가 끝나기전 틱 이벤트가 발생할때 쌓아올리는 스택에 대한 최대값입니다.")]
                public int maxTickStack = 1;
            }
        }
    }
}