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
                /// ?????? ???? ???????? unit?? ????
                /// </summary>
                /// <param name="unit">?????? ????</param>
                public virtual void AddPassiveSkillToUnit(Unit unit)
                {
                    if (!Active)
                        return;

                    newUnits.Add(unit);
                }
                /// <summary>
                /// ?????? ???? ???? ???????? unit?? ????
                /// </summary>
                /// <param name="unit">?????? ????</param>
                public virtual void RemovePassiveSkillToUnit(Unit unit)
                {
                    int id = unit.GetInstanceID();
                    if (unitsInEffect.ContainsKey(id))
                        removeUnitIDs.Add(id);
                }

                /// <summary>
                /// ?????? ???? ???? ???? ???? ???????? ??????
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
                /// unit?? ?????? ???? ???? ?????????
                /// </summary>
                /// <param name="unit">????</param>
                /// <returns>???? ???? ???????? true, ?????? false</returns>
                public virtual bool IsUnitInPassive(Unit unit)
                {
                    if (unitsInEffect.ContainsKey(unit.GetInstanceID()))
                        return isInf || unit.curPSkills[TypeID].remainTime > 0.0f;

                    return false;
                }
                /// <summary>
                /// ?????? ?????? ?????? ?? ????
                /// </summary>
                /// <param name="unit">???? ????</param>
                /// <param name="areaStack">???? ???? ????</param>
                protected virtual void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    OnBeginPassiveEffectEvent?.Invoke(unit, areaStack);
                }
                /// <summary>
                /// ?????? ???????? ?????? ?? ????
                /// </summary>
                /// <param name="unit">???? ????</param>
                /// <param name="areaStack">???? ???? ????</param>
                protected virtual void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    OnEndPassiveEffectEvent?.Invoke(unit, areaStack);
                }
                /// <summary>
                /// ?????? ?????? ???????? ???? ??????????(?????? ??????????)
                /// </summary>
                protected virtual void OnEmptyInEffect()
                {
                    OnEmptyInEffectEvent?.Invoke();
                }
                /// <summary>
                /// ?? ?????? ???? ????
                /// </summary>
                /// <param name="unit">????</param>
                /// <param name="tickStack">?? ????</param>
                protected virtual void OnTickPassiveEffect(Unit unit, int tickStack)
                {
                    OnTickPassiveEffectEvent?.Invoke(unit, tickStack);
                }
                private void CleanUpRemoveUnits()
                {
                    PassiveSkillStateData unitPSkillData;

                    // ?????????? ????
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

                                // ???? ?????? ???????? ???? ??????
                                if (!unitsInEffect.ContainsKey(id))
                                {
                                    // ???????? ???? ?????? ?????? ???????????
                                    if (unit.curPSkills.ContainsKey(TypeID))
                                    {
                                        // ?????????? ?????????? ????
                                        unitPSkillData = unit.curPSkills[TypeID];

                                        // ???? ???????????? ????????????
                                        if (unitPSkillData.areaStack == maxAreaStack)
                                            continue;
                                        
                                        ++unitPSkillData.areaStack;
                                    }
                                    else
                                    {
                                        // ?????? ?????? ?????? ?????? ????
                                        unitPSkillData = new PassiveSkillStateData(this, 1, 0, remainTimeType == SKILL_REMAINTIME_TYPE.NONE ? Duration : 0.0f);
                                        unit.curPSkills.Add(TypeID, unitPSkillData);
                                    }

                                    unitsInEffect.Add(id, unit);

                                    OnBeginPassiveEffect(unit, unitPSkillData.areaStack);
                                }
                                else
                                {
                                    unitPSkillData = unit.curPSkills[TypeID];

                                    // ?????????? ???????? ?? ???????????? ???? ???? ???????? ???????????? ???? ???? ???? ?? ???????? ???????? ???? ?????????????? ????
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

                        // ?????????? ?????????? ???? ????
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
                                // unit?? ?????? ???????? ??????????
                                if (!IsUnitInPassive(unit))
                                    // ???????????? ????
                                    removeUnitIDs.Add(unit.GetInstanceID());

                            // ?????????? ????
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
                [BoxGroup("???????? ????"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("???????? ???? ????")]
                protected bool PP_UseDuration = true;
                [BoxGroup("???????? ????"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("???????? ???? ????")]
                protected bool PP_UseRestrictionFlag = true;
                [BoxGroup("???????? ????"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("???????? ???? ????")]
                protected bool PP_UseRemainTimeType = true;
                [BoxGroup("???????? ????"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("???? ???? ????")]
                protected bool PP_UseAreaStack = true;
                [BoxGroup("???????? ????"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("?? ???? ????")]
                protected bool PP_UseTick = true;

#if UNITY_EDITOR
                [ShowIf("PP_UseDuration")]
#endif
                [FoldoutGroup("PassiveSkill", 4), Min(0.0f)]
                [LabelText("?????? ???? ????????"), Tooltip("?????? ?????? ??????????????.\n0???? ???????? ?????????? ????????.")]
                public float duration;
#if UNITY_EDITOR
                [ShowIf("PP_UseInRangeHitboxes")]
#endif
                [FoldoutGroup("PassiveSkill")]
                [LabelText("???? ???? ????"), Tooltip("true : ???? ???????? ???????? ??????????.\nfalse : ???????? ???? ???????? ?????????? ???????? ??????????.")]
                public bool isTargetingAll;
#if UNITY_EDITOR
                [ShowIf("PP_UseRestrictionFlag")]
#endif
                [FoldoutGroup("PassiveSkill"), EnumToggleButtons]
                [LabelText("????????"), Tooltip("?????? ???? ???? ???????? ???? ?????????? ?????? ????")]
                public RESTRICTION_FLAG restrictionFlag;
#if UNITY_EDITOR
                [ShowIf("PP_UseRemainTimeType")]
#endif
                [FoldoutGroup("PassiveSkill"), EnumToggleButtons]
                [LabelText("???? ????"), Tooltip("???? ???? ????\nNONE : ???????? ???????????? ??????.\nRESET : ???????? ?????? ??????.\nADD : ???? ?????? ???? ???????????? ????????.")]
                public SKILL_REMAINTIME_TYPE remainTimeType;
#if UNITY_EDITOR
                [ShowIf("PP_UseAreaStack")]
#endif
                [FoldoutGroup("PassiveSkill"), Min(1)]
                [LabelText("???? ???? ???? ????"), Tooltip("???? ???? ???? ???? ????\n?????? ?????? ?? ???????? ???? ??????????.")]
                public int maxAreaStack = 1;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill")]
                [LabelText("?? ?????? ????"), Tooltip("?????? ???? ???????? ???? ???????? ???? ?????? ???????? ????")]
                public bool isUsingTick;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill"), EnableIf("isUsingTick")]
                [LabelText("?? ??????(??)"), Tooltip("?????????? ???????? ?? ???????? ????????")]
                public float tickCycleTime;
#if UNITY_EDITOR
                [ShowIf("PP_UseTick")]
#endif
                [FoldoutGroup("PassiveSkill"), EnableIf("isUsingTick"), Min(1)]
                [LabelText("???? ?? ????"), Tooltip("?? ???? ???? ????\n?????? ?????? ???????? ?? ???????? ???????? ?????????? ?????? ???? ????????????.")]
                public int maxTickStack = 1;
            }
        }
    }
}