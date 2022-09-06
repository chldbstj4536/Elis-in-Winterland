using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract partial class ActiveSkill : BaseSkill
            {
                #region Field
                protected SKILL_CASTING_TYPE skillCastingType;
                
                // 애니메이션 정보
                protected AnimationTrackSet[] animSetCasting;
                protected AnimationTrackSet[] animSetCastingInMove;
                protected AnimationTrackSet[] animSetCastingInStop;
                protected AnimationTrackSet[] animSetAttack;
                protected AnimationTrackSet[] animSetAttackInMove;
                protected AnimationTrackSet[] animSetAttackInStop;

                // 현재 스킬이 기본공격인지
                private readonly bool isDefaultAttack;
                private float coolTime;
                private int maxCharge;
                private readonly float chargeTime;
                private readonly int manaCost;
                private readonly int maxCombo;
                private int curCharge;
                protected int curCombo;
                private float remainCoolTime;
                private float remainChargeTime;

                // 캐스팅 또는 공격 중 이동속도 변화량
                protected float moveSpdInCasting = 0.0f;
                protected float moveSpdInAttack = 0.0f;


                // 스킬 도중에 캔슬 가능한지 (스킬중 다른 입력이 들어오면 캔슬하고 다른 입력으로 전환 가능한지)
                protected bool bCanCancelInCasting;
                protected bool bCanCancelInAttack;

                // 시전자가 플레이어인지
                protected bool isPlayer;

                public event OnChangedValue OnChangedRemainCoolTime;
                public event OnChangedValue OnChangedCurrentCharge;
                public event OnChangedValue OnChangedRemainChargeTime;
                public event Delegate_NoRetVal_NoParam OnBeginSelectingTargetEvent;
                public event Delegate_NoRetVal_NoParam OnUpdateSelectingTargetEvent;
                public event Delegate_NoRetVal_NoParam OnEndSelectingTargetEvent;
                public event Delegate_NoRetVal_NoParam OnBeginCastingEvent;
                public event Delegate_NoRetVal_NoParam OnUpdateCastingEvent;
                public event Delegate_NoRetVal_NoParam OnEndCastingEvent;
                public event Delegate_NoRetVal_NoParam OnBeginAttackEvent;
                public event Delegate_NoRetVal_NoParam OnUpdateAttackEvent;
                public event Delegate_NoRetVal_NoParam OnEndAttackEvent;
                public event Delegate_NoRetVal_NoParam OnTriggerEvent;
                #endregion

                #region Properties
                public override bool IsUsingCritical => isDefaultAttack;
                public SKILL_CASTING_TYPE SkillCastingType => skillCastingType;
                public bool IsInCoolTime => RemainCoolTime != 0.0f;
                public float RemainCoolTime
                {
                    get { return remainCoolTime; }
                    protected set
                    {
                        remainCoolTime = value;
                        OnChangedRemainCoolTime?.Invoke();
                    }
                }
                public float RemainChargeTime
                {
                    get { return remainChargeTime; }
                    protected set
                    {
                        remainChargeTime = value;
                        OnChangedRemainChargeTime?.Invoke();
                    }
                }
                public int CurrentCharge
                {
                    get { return curCharge; }
                    protected set
                    {
                        curCharge = value;
                        if (curCharge >= maxCharge)
                        {
                            curCharge = maxCharge;
                            remainChargeTime = ChargeTime;
                        }
                        OnChangedCurrentCharge?.Invoke();
                    }
                }
                public float CoolTime => coolTime * caster.cdr;
                public float ChargeTime => chargeTime;
                public int ManaCost => manaCost;
                public int MaxCombo => maxCombo;
                public int CurrentCombo => curCombo;
                public bool IsInCombo => curCombo != 0;
                public bool IsUsingCharge => maxCharge > 1;
                public bool CanCancel => (caster.curSkill.bCanCancelInCasting && caster.fsm.CurrentStateIndex == FSM.STATE_INDEX.CASTING) ||
                        (caster.curSkill.bCanCancelInAttack && caster.fsm.CurrentStateIndex == FSM.STATE_INDEX.ATTACK);
                #endregion

                #region Methods
                protected ActiveSkill(ActiveSkillData data, Unit skillOwner) : base(data, skillOwner)
                {
                    skillCastingType = data.skillCastingType;
                    animSetCasting = data.animSetCasting;
                    animSetCastingInMove = data.animSetCastingInMove;
                    animSetCastingInStop = data.animSetCastingInStop;
                    animSetAttack = data.animSetAttack;
                    animSetAttackInMove = data.animSetAttackInMove;
                    animSetAttackInStop = data.animSetAttackInStop;
                    isDefaultAttack  = data.isDefaultAttack;
                    coolTime = data.coolTime;
                    maxCharge = data.maxCharge;
                    curCharge =  data.baseCharge;
                    chargeTime = data.chargeTime;
                    manaCost = data.manaCost;
                    maxCombo = data.maxCombo;
                    moveSpdInCasting  = data.moveSpdInCasting;
                    moveSpdInAttack  = data.moveSpdInAttack;
                    bCanCancelInCasting = data.bCanCancelInCasting;
                    bCanCancelInAttack = data.bCanCancelInAttack;

                    curCombo = 0;
                    
                    isPlayer = caster is PlayableUnit;

                    if (IsUsingCharge)
                        caster.StartCoroutine(ChargeCooltimeCalc());
                }
                /// <summary>
                /// 스킬 발동함수
                /// 스킬 발동에 실패하면 원인에 대한 에러코드를 반환
                /// </summary>
                public virtual SKILL_ERROR_CODE CastSkill()
                {
                    SKILL_ERROR_CODE err;

                    if (IsInCombo)
                        err = CastCombo();

                    else
                    {
                        // 스킬 사용 준비가 되었는지 체크
                        err = IsReady();

                        // 발동 가능하다면 스킬에 대한 FSM 설정
                        if (err == SKILL_ERROR_CODE.NO_ERR)
                        {
                            Active = true;
                            caster.curSkill = this;
                            caster.fsm.stateSelectingTarget.OnEnter = OnBeginSelectingTarget;
                            caster.fsm.stateSelectingTarget.Update = OnUpdateSelectingTarget;
                            caster.fsm.stateSelectingTarget.OnExit = OnEndSelectingTarget;
                            caster.fsm.stateCasting.OnEnter = OnBeginCasting;
                            caster.fsm.stateCasting.Update = OnUpdateCasting;
                            caster.fsm.stateCasting.OnExit = OnEndCasting;
                            caster.fsm.stateAttack.OnEnter = OnBeginAttack;
                            caster.fsm.stateAttack.Update = OnUpdateAttack;
                            caster.fsm.stateAttack.OnExit = OnEndAttack;

                            if (!(this is NoneSkill) && isPlayer && !caster.gm.Setting.isUsingFastSkill && skillCastingType != SKILL_CASTING_TYPE.CHARGING)
                                caster.fsm.ChangeState(caster.fsm.stateSelectingTarget);
                            else if (animSetCasting != null && animSetCasting.Length != 0)
                                caster.fsm.ChangeState(caster.fsm.stateCasting, true);
                            else if (animSetAttack != null && animSetAttack.Length != 0)
                                caster.fsm.ChangeState(caster.fsm.stateAttack, true);
                            else
                            {
                                Trigger();
                                caster.curSkill = null;
                            }

                            curCombo = ++curCombo % maxCombo;
                            if (IsUsingCharge)
                                --CurrentCharge;
                        }
                    }

                    return err;
                }
                public virtual SKILL_ERROR_CODE CastCombo()
                {
                    return SKILL_ERROR_CODE.NO_ERR;
                }
                /// <summary>
                /// 현재 스킬 타겟 선택 중이라면 캐스팅/공격으로 바꾸기
                /// </summary>
                public void CompleteSelectingTarget()
                {
                    if (animSetCasting == null || animSetCasting.Length == 0)
                        caster.fsm.ChangeState(caster.fsm.stateAttack);
                    else
                        caster.fsm.ChangeState(caster.fsm.stateCasting);
                }
                /// <summary>
                /// 현재 스킬 캐스팅 중이라면 캐스팅 완료로 바꾸기
                /// </summary>
                public void CompleteCasting()
                {
                    if (skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        caster.fsm.ChangeState(caster.fsm.stateAttack);
                }
                /// <summary>
                /// 현재 지속 스킬 중이라면 지속 스킬 끝내기
                /// </summary>
                public void CompleteTickAttack()
                {
                    if (skillCastingType == SKILL_CASTING_TYPE.TICK)
                        caster.mainAnimPlayer.ExitLoopAllTrack();
                }
                /// <summary>
                /// 스킬이 사용 가능한 상태인가?
                /// 불가능하다면 원인에 대한 에러코드를 반환
                /// </summary>
                protected virtual SKILL_ERROR_CODE IsReady()
                {
                    // AI라면 스킬의 범위 내에 적이 존재하는지 체크를 통해 스킬 발동 가능 여부를 결정
                    if (!isPlayer)
                        if (!TargetCheck(Utility.SweepUnit(inRangeHitboxes, caster.transform.position, InRangeHitBoxesRot, true, TargetLayerMask, TargetType)))
                            return SKILL_ERROR_CODE.NO_TARGET;

                    // 쿨타임 중인가
                    if (IsInCoolTime || (IsUsingCharge && curCharge == 0))
                        return SKILL_ERROR_CODE.COOLTIME;
                    // 마나가 부족한가
                    if (caster.curMP < manaCost)
                        return SKILL_ERROR_CODE.NOT_ENOUGH_MANA;
                    // 공격 불가능한 상태이상에 걸렸는가
                    if (!caster.IsAttackable)
                        return SKILL_ERROR_CODE.IN_CC;
                    // 캔슬 불가능한 스킬인가
                    if (caster.curSkill != null && !CanCancel)
                        return SKILL_ERROR_CODE.CANT_CANCEL;

                    return SKILL_ERROR_CODE.NO_ERR;
                }
                protected virtual void Trigger()
                {
                    OnTriggerEvent?.Invoke();
                }

                /// <summary>
                /// [AI 전용함수]
                /// 공격 감지 범위안에 포함된 유닛들중 스킬의 타겟 조건에 맞는지 확인하는 함수.
                /// 성공한다면 타겟(위치)지정까지 같이 설정
                /// ex) 고위등급 몬스터를 대상으로 하는 스킬의 경우, targets의 리스트에 고위등급 몬스터가 있는지 체크
                /// </summary>
                /// <param name="targets">공격 감지 범위에 포함된 유닛들</param>
                /// <returns>타겟 존재 여부</returns>
                protected virtual bool TargetCheck(List<RaycastHit> targets)
                {
                    if (caster.IsTaunt)
                    {
                        foreach (var target in targets)
                            if (target.transform == caster.tauntUnit.transform)
                                return true;

                        return false;
                    }

                    return targets.Count != 0;
                }

                /// <summary>
                /// 스킬 쿨타임 시작 함수
                /// </summary>
                protected void CooltimeStart()
                {
                    // 기본 공격이라면 기본공격에 맞게끔 쿨타임 설정
                    if (isDefaultAttack)
                        coolTime = 1.0f / caster.TotalAttackSpeed;

                    caster.StartCoroutine(CooltimeCalc());
                }
                private IEnumerator CooltimeCalc()
                {
                    WaitForSeconds wf50ms = CachedWaitForSeconds.Get(0.05f);

                    RemainCoolTime = CoolTime;
                    while (RemainCoolTime > 0.0f)
                    {
                        yield return wf50ms;

                        RemainCoolTime -= 0.05f;
                    }
                    RemainCoolTime = 0.0f;
                }
                private IEnumerator ChargeCooltimeCalc()
                {
                    WaitForSeconds wf50ms = CachedWaitForSeconds.Get(0.05f);

                    RemainChargeTime = ChargeTime;
                    while (true)
                    {
                        yield return wf50ms;

                        if (CurrentCharge < maxCharge)
                        {
                            RemainChargeTime -= 0.05f;

                            if (RemainChargeTime <= 0.0f)
                            {
                                RemainChargeTime = ChargeTime;
                                ++CurrentCharge;
                            }
                        }
                    }
                }
                // 각 이벤트별로 모든 스킬의 공통된 연산들을 수행
                #region FSM Event
                protected virtual void OnBeginSelectingTarget()
                {
                    OnBeginSelectingTargetEvent?.Invoke();
                    (caster as PlayableUnit).SetRangeFX(true, Vector3.one * TotalRange);
                }
                protected virtual void OnUpdateSelectingTarget()
                {
                    OnUpdateSelectingTargetEvent?.Invoke();
                    Utility.FlipUnitMoveDir(caster);
                }
                protected virtual void OnEndSelectingTarget(FSM.State newState)
                {
                    OnEndSelectingTargetEvent?.Invoke();
                    (caster as PlayableUnit).SetRangeFX(false, Vector3.one);

                    // 다음 상태가 공격이 아니라면, 캔슬 된 경우이므로 curSkill을 null로 초기화
                    if (newState.id != FSM.STATE_INDEX.CASTING && newState.id != FSM.STATE_INDEX.ATTACK)
                        caster.curSkill = null;
                }
                protected virtual void OnBeginCasting()
                {
                    OnBeginCastingEvent?.Invoke();
                    if (caster.mc != null)
                    {
                        if (moveSpdInCasting == 0f)
                            caster.mc.FixMove();
                        else
                            caster.mc.MoveSpeedCoef *= moveSpdInCasting;
                    }

                    caster.mainAnimPlayer.SetAnimationSets(animSetCasting, true);
                    caster.mainAnimPlayer.Complete += () =>
                    {
                        caster.fsm.ChangeState(caster.fsm.stateAttack);
                    };

                    caster.moveAnimPlayer.SetAnimationSets(animSetCastingInMove, caster.mc.IsMoving);
                    caster.stopAnimPlayer.SetAnimationSets(animSetCastingInStop, !caster.mc.IsMoving);

                    // 캐스팅 단계에서 마우스위치나 특정 대상에 대한 위치가 빌요하다면 필요한 스킬에서 이 함수를 오버라이드해 구현
                }
                protected virtual void OnUpdateCasting()
                {
                    OnUpdateCastingEvent?.Invoke();
                    // 캐스팅동안 범위가 증가와 같은 지속적인 효과를 주고싶다면 이 함수를 오버라이드해 구현
                }
                protected virtual void OnEndCasting(FSM.State newState)
                {
                    OnEndCastingEvent?.Invoke();
                    if (caster.mc != null)
                    {
                        if (moveSpdInCasting == 0f)
                            caster.mc.Stop();
                        else
                            caster.mc.MoveSpeedCoef /= moveSpdInCasting;
                    }

                    // 다음 상태가 공격이 아니라면, 캔슬 된 경우이므로 curSkill을 null로 초기화
                    if (newState.id != FSM.STATE_INDEX.ATTACK)
                        caster.curSkill = null;
                }
                protected virtual void OnBeginAttack()
                {
                    OnBeginAttackEvent?.Invoke();
                    if (caster.mc != null)
                    {
                        if (moveSpdInAttack == 0f)
                            caster.mc.FixMove();
                        else
                            caster.mc.MoveSpeedCoef *= moveSpdInAttack;
                    }

                    caster.mainAnimPlayer.SetAnimationSets(animSetAttack, true, isDefaultAttack ? caster.TotalAttackSpeed : 1.0f);
                    caster.mainAnimPlayer.Complete += () =>
                    {
                        caster.fsm.ChangeState(caster.fsm.stateIdle);
                    };
                    caster.mainAnimPlayer.Event = (Spine.TrackEntry te, Spine.Event e) =>
                    {
                        if (e.Data.Name == "Attack")
                        {
                            OnTriggerEvent?.Invoke();
                        }
                    };

                    caster.moveAnimPlayer.SetAnimationSets(animSetAttackInMove, caster.mc.IsMoving);
                    caster.stopAnimPlayer.SetAnimationSets(animSetAttackInStop, !caster.mc.IsMoving);
                }
                protected virtual void OnUpdateAttack()
                {
                    OnUpdateAttackEvent?.Invoke();
                }
                protected virtual void OnEndAttack(FSM.State newState)
                {
                    OnEndAttackEvent?.Invoke();
                    if (caster.mc != null)
                    {
                        if (moveSpdInAttack == 0f)
                            caster.mc.Stop();
                        else
                            caster.mc.MoveSpeedCoef /= moveSpdInAttack;
                    }

                    caster.curSkill = null;
                    if (!IsInCombo)
                        Active = false;
                }
                #endregion
                #endregion
            }

            [System.Serializable]
            public abstract class ActiveSkillData : BaseSkillData
            {
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("콤보 사용")]
                protected bool PP_UseCombo = true;
                [BoxGroup("프로퍼티 세팅"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("플레이어 전용")]
                protected bool PP_UsePlayerOnly = true;
                [FoldoutGroup("ActiveSkill", 4), DisableIf("@true")]
                [LabelText("스킬 캐스팅 종류"), Tooltip("TOGGLE : 스킬키를 누르면 발동\nCASTING : 스킬키를 누르고있으면 발동\nCHARGING_ACTION: 스킬 키를 누른 후 때면 발동\nCASTING_ACTION : 스킬 키를 누르고 상호작용시 발동")]
                public SKILL_CASTING_TYPE skillCastingType;

                // 애니메이션 정보
                [FoldoutGroup("ActiveSkill/애니메이션", false)]
                [FoldoutGroup("ActiveSkill/애니메이션/캐스팅 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetCasting;
                [FoldoutGroup("ActiveSkill/애니메이션/캐스팅-이동 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetCastingInMove;
                [FoldoutGroup("ActiveSkill/애니메이션/캐스팅-정지 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetCastingInStop;
                [FoldoutGroup("ActiveSkill/애니메이션/공격 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetAttack;
                [FoldoutGroup("ActiveSkill/애니메이션/공격-이동 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetAttackInMove;
                [FoldoutGroup("ActiveSkill/애니메이션/공격-정지 애니메이션", true), HideLabel]
                public AnimationTrackSet[] animSetAttackInStop;

                // 현재 스킬이 기본공격인지
                [FoldoutGroup("ActiveSkill"), DisableIf("@true")]
                [LabelText("기본공격인가?")]
                public bool isDefaultAttack;
                [FoldoutGroup("ActiveSkill"), DisableIf("isDefaultAttack"), Min(0.0f)]
                [LabelText("쿨타임(초)"), Tooltip("스킬의 재사용 대기 시간")]
                public float coolTime;
                [FoldoutGroup("ActiveSkill"), Min(1)]
                [LabelText("최대 충전"), Tooltip("최대 충전 가능한 스킬의 개수")]
                public int maxCharge = 1;
                [FoldoutGroup("ActiveSkill"), Min(1), MaxValue("maxCharge")]
                [LabelText("기본 충전"), Tooltip("기본으로 주어지는 충전량")]
                public int baseCharge = 1;
                [FoldoutGroup("ActiveSkill"), DisableIf("@maxCharge == 1"), Min(0.0f)]
                [LabelText("충전 시간(초)"), Tooltip("스킬의 충전 시간")]
                public float chargeTime;

                [FoldoutGroup("ActiveSkill"), Min(0)]
                [LabelText("마나 비용"), Tooltip("스킬 사용시 소모되는 마나 비용")]
                public int manaCost;

#if UNITY_EDITOR
                [ShowIf("PP_UseCombo")]
#endif
                [FoldoutGroup("ActiveSkill"), Min(1)]
                [LabelText("최대 콤보"), Tooltip("스킬 시전 후 몇번 더 시전 할 수 있는지\n각 시전마다의 효과는 스킬마다 다를 수 있음")]
                public int maxCombo = 1;

                // 캐스팅 또는 공격 중 이동속도 변화량
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetCasting == null || animSetCasting.Length == 0")]
                [LabelText("캐스팅 중 이동속도 변화량")]
                public float moveSpdInCasting = 0.0f;
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetAttack == null || animSetAttack.Length == 0")]
                [LabelText("공격 중 이동속도 변화량")]
                public float moveSpdInAttack = 0.0f;

                // 스킬 도중에 캔슬 가능한지 (스킬중 다른 입력이 들어오면 캔슬하고 다른 입력으로 전환 가능한지)
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetCasting == null || animSetCasting.Length == 0")]
                [LabelText("캐스팅 중 캔슬 가능한가?")]
                public bool bCanCancelInCasting;
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetAttack == null || animSetAttack.Length == 0")]
                [LabelText("공격 중 캔슬 가능한가?")]
                public bool bCanCancelInAttack;
            }
        }
    }
}