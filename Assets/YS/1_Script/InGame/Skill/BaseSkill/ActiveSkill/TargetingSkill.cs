using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract class TargetingSkill : ActiveSkill
            {
                #region Field
                protected Transform target;
                protected SKILL_RANGE_TYPE rangeType;
                #endregion

                #region Properties
                public Transform TargetTransform => target;
                public SKILL_RANGE_TYPE RangeType => rangeType;
                #endregion

                #region Methods
                protected TargetingSkill(TargetingSkillData data, Unit skillOwner) : base(data, skillOwner)
                {
                    rangeType = data.rangeType;
                }
                protected override SKILL_ERROR_CODE IsReady()
                {
                    var code = base.IsReady();
                    if (code != SKILL_ERROR_CODE.NO_ERR)
                        return code;

                    // �÷��̾ ������ų����� ����ϰ� ��ųŸ���� ��¡��ų�� �ƴ϶��
                    if (isPlayer && caster.gm.Setting.isUsingFastSkill && skillCastingType != SKILL_CASTING_TYPE.CHARGING)
                        code = SetTargetForPlayer();

                    return code;
                }
                public SKILL_ERROR_CODE SetTargetForPlayer()
                {
                    target = null;

                    if (!Utility.GetUnitMouseRaycast(out var mouseHit, TargetLayerMask, TargetType))
                        return SKILL_ERROR_CODE.NO_TARGET;

                    Vector3 pos = mouseHit.transform.position;

                    switch (rangeType)
                    {
                        case SKILL_RANGE_TYPE.CIRCLE:
                            if ((pos - caster.transform.position).sqrMagnitude <= SqrTotalRange)
                                target = mouseHit.transform;
                            break;
                        case SKILL_RANGE_TYPE.X_AXIS:
                            if (Mathf.Abs((pos.x - caster.transform.position.x)) <= TotalRange)
                                target = mouseHit.transform;
                            break;
                    }

                    if (target == null)
                        return SKILL_ERROR_CODE.OUT_OF_RANGE;

                    return SKILL_ERROR_CODE.NO_ERR;
                }
                protected override bool TargetCheck(List<RaycastHit> targets)
                {
                    if (base.TargetCheck(targets))
                    {
                        if (caster.IsTaunt)
                            target = caster.tauntUnit.transform;
                        else
                        {
                            float minDist = float.MaxValue;
                            float sqrDist;

                            // ���� ����� ����� Ÿ������ ����
                            foreach (var hit in targets)
                            {
                                sqrDist = (hit.transform.position - caster.transform.position).sqrMagnitude;
                                if (minDist > sqrDist)
                                {
                                    minDist = sqrDist;
                                    target = hit.transform;
                                }
                            }
                        }
                        return true;
                    }

                    return false;
                }
                #region FSM Event
                protected override void OnBeginSelectingTarget()
                {
                    (caster as PlayableUnit).SetChoosingTarget(true, TargetLayerMask, TargetType);

                    base.OnBeginSelectingTarget();
                }
                protected override void OnEndSelectingTarget(FSM.State newState)
                {
                    (caster as PlayableUnit).SetChoosingTarget(false, 0, 0);

                    base.OnEndSelectingTarget(newState);
                }
                protected override void OnBeginCasting()
                {
                    if (isPlayer && skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        (caster as PlayableUnit).SetChoosingTarget(true, TargetLayerMask, TargetType);

                    base.OnBeginCasting();
                }
                protected override void OnEndCasting(FSM.State newState)
                {
                    if (isPlayer && skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        (caster as PlayableUnit).SetChoosingTarget(false, 0, 0);

                    base.OnEndCasting(newState);
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Utility.FlipUnit(caster, target.position);
                }
                #endregion
                #endregion
            }
            public abstract class TargetingSkillData : ActiveSkillData
            {
#if UNITY_EDITOR
                [ShowIf("PP_UsePlayerOnly")]
#endif
                [FoldoutGroup("TargetingSkill", 3)]
                [BoxGroup("TargetingSkill/�÷��̾� ����", true, true)]
                [LabelText("���� Ÿ��"), Tooltip("�������� ��ġ�� �������� ��� ������ ���������� ���� ��\nCIRCLE : ��ų ������ �������̰� ���� �߽��� �������� ��ġ�� �� Ÿ���� ����\nX_AXIS : �������� ��ġ�� �������� +-��ų ����")]
                public SKILL_RANGE_TYPE rangeType;
            }
        }
    }
}