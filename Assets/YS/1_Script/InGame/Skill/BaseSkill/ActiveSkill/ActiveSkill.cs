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
                
                // �ִϸ��̼� ����
                protected AnimationTrackSet[] animSetCasting;
                protected AnimationTrackSet[] animSetCastingInMove;
                protected AnimationTrackSet[] animSetCastingInStop;
                protected AnimationTrackSet[] animSetAttack;
                protected AnimationTrackSet[] animSetAttackInMove;
                protected AnimationTrackSet[] animSetAttackInStop;

                // ���� ��ų�� �⺻��������
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

                // ĳ���� �Ǵ� ���� �� �̵��ӵ� ��ȭ��
                protected float moveSpdInCasting = 0.0f;
                protected float moveSpdInAttack = 0.0f;


                // ��ų ���߿� ĵ�� �������� (��ų�� �ٸ� �Է��� ������ ĵ���ϰ� �ٸ� �Է����� ��ȯ ��������)
                protected bool bCanCancelInCasting;
                protected bool bCanCancelInAttack;

                // �����ڰ� �÷��̾�����
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
                /// ��ų �ߵ��Լ�
                /// ��ų �ߵ��� �����ϸ� ���ο� ���� �����ڵ带 ��ȯ
                /// </summary>
                public virtual SKILL_ERROR_CODE CastSkill()
                {
                    SKILL_ERROR_CODE err;

                    if (IsInCombo)
                        err = CastCombo();

                    else
                    {
                        // ��ų ��� �غ� �Ǿ����� üũ
                        err = IsReady();

                        // �ߵ� �����ϴٸ� ��ų�� ���� FSM ����
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
                /// ���� ��ų Ÿ�� ���� ���̶�� ĳ����/�������� �ٲٱ�
                /// </summary>
                public void CompleteSelectingTarget()
                {
                    if (animSetCasting == null || animSetCasting.Length == 0)
                        caster.fsm.ChangeState(caster.fsm.stateAttack);
                    else
                        caster.fsm.ChangeState(caster.fsm.stateCasting);
                }
                /// <summary>
                /// ���� ��ų ĳ���� ���̶�� ĳ���� �Ϸ�� �ٲٱ�
                /// </summary>
                public void CompleteCasting()
                {
                    if (skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        caster.fsm.ChangeState(caster.fsm.stateAttack);
                }
                /// <summary>
                /// ���� ���� ��ų ���̶�� ���� ��ų ������
                /// </summary>
                public void CompleteTickAttack()
                {
                    if (skillCastingType == SKILL_CASTING_TYPE.TICK)
                        caster.mainAnimPlayer.ExitLoopAllTrack();
                }
                /// <summary>
                /// ��ų�� ��� ������ �����ΰ�?
                /// �Ұ����ϴٸ� ���ο� ���� �����ڵ带 ��ȯ
                /// </summary>
                protected virtual SKILL_ERROR_CODE IsReady()
                {
                    // AI��� ��ų�� ���� ���� ���� �����ϴ��� üũ�� ���� ��ų �ߵ� ���� ���θ� ����
                    if (!isPlayer)
                        if (!TargetCheck(Utility.SweepUnit(inRangeHitboxes, caster.transform.position, InRangeHitBoxesRot, true, TargetLayerMask, TargetType)))
                            return SKILL_ERROR_CODE.NO_TARGET;

                    // ��Ÿ�� ���ΰ�
                    if (IsInCoolTime || (IsUsingCharge && curCharge == 0))
                        return SKILL_ERROR_CODE.COOLTIME;
                    // ������ �����Ѱ�
                    if (caster.curMP < manaCost)
                        return SKILL_ERROR_CODE.NOT_ENOUGH_MANA;
                    // ���� �Ұ����� �����̻� �ɷȴ°�
                    if (!caster.IsAttackable)
                        return SKILL_ERROR_CODE.IN_CC;
                    // ĵ�� �Ұ����� ��ų�ΰ�
                    if (caster.curSkill != null && !CanCancel)
                        return SKILL_ERROR_CODE.CANT_CANCEL;

                    return SKILL_ERROR_CODE.NO_ERR;
                }
                protected virtual void Trigger()
                {
                    OnTriggerEvent?.Invoke();
                }

                /// <summary>
                /// [AI �����Լ�]
                /// ���� ���� �����ȿ� ���Ե� ���ֵ��� ��ų�� Ÿ�� ���ǿ� �´��� Ȯ���ϴ� �Լ�.
                /// �����Ѵٸ� Ÿ��(��ġ)�������� ���� ����
                /// ex) ������� ���͸� ������� �ϴ� ��ų�� ���, targets�� ����Ʈ�� ������� ���Ͱ� �ִ��� üũ
                /// </summary>
                /// <param name="targets">���� ���� ������ ���Ե� ���ֵ�</param>
                /// <returns>Ÿ�� ���� ����</returns>
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
                /// ��ų ��Ÿ�� ���� �Լ�
                /// </summary>
                protected void CooltimeStart()
                {
                    // �⺻ �����̶�� �⺻���ݿ� �°Բ� ��Ÿ�� ����
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
                // �� �̺�Ʈ���� ��� ��ų�� ����� ������� ����
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

                    // ���� ���°� ������ �ƴ϶��, ĵ�� �� ����̹Ƿ� curSkill�� null�� �ʱ�ȭ
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

                    // ĳ���� �ܰ迡�� ���콺��ġ�� Ư�� ��� ���� ��ġ�� �����ϴٸ� �ʿ��� ��ų���� �� �Լ��� �������̵��� ����
                }
                protected virtual void OnUpdateCasting()
                {
                    OnUpdateCastingEvent?.Invoke();
                    // ĳ���õ��� ������ ������ ���� �������� ȿ���� �ְ�ʹٸ� �� �Լ��� �������̵��� ����
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

                    // ���� ���°� ������ �ƴ϶��, ĵ�� �� ����̹Ƿ� curSkill�� null�� �ʱ�ȭ
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
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("�޺� ���")]
                protected bool PP_UseCombo = true;
                [BoxGroup("������Ƽ ����"), ShowIf("ShowPPSetting"), SerializeField]
                [LabelText("�÷��̾� ����")]
                protected bool PP_UsePlayerOnly = true;
                [FoldoutGroup("ActiveSkill", 4), DisableIf("@true")]
                [LabelText("��ų ĳ���� ����"), Tooltip("TOGGLE : ��ųŰ�� ������ �ߵ�\nCASTING : ��ųŰ�� ������������ �ߵ�\nCHARGING_ACTION: ��ų Ű�� ���� �� ���� �ߵ�\nCASTING_ACTION : ��ų Ű�� ������ ��ȣ�ۿ�� �ߵ�")]
                public SKILL_CASTING_TYPE skillCastingType;

                // �ִϸ��̼� ����
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�", false)]
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/ĳ���� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetCasting;
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/ĳ����-�̵� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetCastingInMove;
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/ĳ����-���� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetCastingInStop;
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/���� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetAttack;
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/����-�̵� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetAttackInMove;
                [FoldoutGroup("ActiveSkill/�ִϸ��̼�/����-���� �ִϸ��̼�", true), HideLabel]
                public AnimationTrackSet[] animSetAttackInStop;

                // ���� ��ų�� �⺻��������
                [FoldoutGroup("ActiveSkill"), DisableIf("@true")]
                [LabelText("�⺻�����ΰ�?")]
                public bool isDefaultAttack;
                [FoldoutGroup("ActiveSkill"), DisableIf("isDefaultAttack"), Min(0.0f)]
                [LabelText("��Ÿ��(��)"), Tooltip("��ų�� ���� ��� �ð�")]
                public float coolTime;
                [FoldoutGroup("ActiveSkill"), Min(1)]
                [LabelText("�ִ� ����"), Tooltip("�ִ� ���� ������ ��ų�� ����")]
                public int maxCharge = 1;
                [FoldoutGroup("ActiveSkill"), Min(1), MaxValue("maxCharge")]
                [LabelText("�⺻ ����"), Tooltip("�⺻���� �־����� ������")]
                public int baseCharge = 1;
                [FoldoutGroup("ActiveSkill"), DisableIf("@maxCharge == 1"), Min(0.0f)]
                [LabelText("���� �ð�(��)"), Tooltip("��ų�� ���� �ð�")]
                public float chargeTime;

                [FoldoutGroup("ActiveSkill"), Min(0)]
                [LabelText("���� ���"), Tooltip("��ų ���� �Ҹ�Ǵ� ���� ���")]
                public int manaCost;

#if UNITY_EDITOR
                [ShowIf("PP_UseCombo")]
#endif
                [FoldoutGroup("ActiveSkill"), Min(1)]
                [LabelText("�ִ� �޺�"), Tooltip("��ų ���� �� ��� �� ���� �� �� �ִ���\n�� ���������� ȿ���� ��ų���� �ٸ� �� ����")]
                public int maxCombo = 1;

                // ĳ���� �Ǵ� ���� �� �̵��ӵ� ��ȭ��
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetCasting == null || animSetCasting.Length == 0")]
                [LabelText("ĳ���� �� �̵��ӵ� ��ȭ��")]
                public float moveSpdInCasting = 0.0f;
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetAttack == null || animSetAttack.Length == 0")]
                [LabelText("���� �� �̵��ӵ� ��ȭ��")]
                public float moveSpdInAttack = 0.0f;

                // ��ų ���߿� ĵ�� �������� (��ų�� �ٸ� �Է��� ������ ĵ���ϰ� �ٸ� �Է����� ��ȯ ��������)
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetCasting == null || animSetCasting.Length == 0")]
                [LabelText("ĳ���� �� ĵ�� �����Ѱ�?")]
                public bool bCanCancelInCasting;
                [FoldoutGroup("ActiveSkill"), DisableIf("@animSetAttack == null || animSetAttack.Length == 0")]
                [LabelText("���� �� ĵ�� �����Ѱ�?")]
                public bool bCanCancelInAttack;
            }
        }
    }
}