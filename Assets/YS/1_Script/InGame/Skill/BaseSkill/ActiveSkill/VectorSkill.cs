using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class VectorSkill : ActiveSkill
            {
                #region Field
                protected Vector3 dest;
                protected SKILL_RANGE_TYPE rangeType;
                protected bool isDirVector;
                #endregion

                public Vector3 Destination => dest;

                #region Methods
                protected VectorSkill(VectorSkillData data, Unit skillOwner) : base(data, skillOwner)
                {
                    rangeType = data.rangeType;
                    isDirVector = data.isDirVector;
                }
                protected override SKILL_ERROR_CODE IsReady()
                {
                    var code = base.IsReady();
                    if (code != SKILL_ERROR_CODE.NO_ERR)
                        return code;

                    if (isPlayer && caster.gm.Setting.isUsingFastSkill && skillCastingType != SKILL_CASTING_TYPE.CHARGING)
                        SetDestinationForPlayer();

                    return code;
                }
                protected void SetDestinationForPlayer()
                {
                    Vector3 casterPos = caster.transform.position;
                    Utility.GetMouseWorldPosition(out dest, (int)(LAYER_MASK.FIELD | LAYER_MASK.OUT_OF_FIELD));
                    dest.y = 0.0f;

                    if (isDirVector)
                    {
                        Vector3 dir = (dest - casterPos).normalized;
                        dest = casterPos + dir * TotalRange;

                        if (TotalRange == INFINITE_RANGE || Mathf.Abs(dest.x) > GameManager.HalfSizeField_X || Mathf.Abs(dest.z) > GameManager.HalfSizeField_Z)
                        {
                            float dx, dz;
                            dx = (dir.x < 0.0f ? -GameManager.HalfSizeField_X : GameManager.HalfSizeField_X) - casterPos.x;
                            dz = (dir.z < 0.0f ? -GameManager.HalfSizeField_Z : GameManager.HalfSizeField_Z) - casterPos.z;

                            if (dir.x == 0.0f)
                                dest = casterPos + dz * dir;
                            else if (dir.z == 0.0f)
                                dest = casterPos + dx * dir;
                            else
                                dest = casterPos + Mathf.Min(dx / dir.x, dz / dir.z) * dir;
                        }
                    }
                    else
                    {
                        dest = Utility.GetPositionInRange(casterPos, dest, TotalRange, rangeType);
                        if (Mathf.Abs(dest.z) >= GameManager.HalfSizeField_Z)
                            dest.z = dest.z < 0.0f ? -GameManager.HalfSizeField_Z : GameManager.HalfSizeField_Z;
                    }
                }
                protected override bool TargetCheck(List<RaycastHit> targets)
                {
                    if (base.TargetCheck(targets))
                    {
                        if (caster.IsTaunt)
                            dest = caster.tauntUnit.transform.position;
                        else
                        {
                            float minDist = float.MaxValue;
                            float sqrDist;

                            // ���� ����� ����� ��ġ�� �������� ����
                            foreach (var hit in targets)
                            {
                                sqrDist = (hit.transform.position - caster.transform.position).sqrMagnitude;
                                if (minDist > sqrDist)
                                {
                                    minDist = sqrDist;
                                    dest = hit.transform.position;
                                }
                            }
                        }

                        return true;
                    }

                    return false;
                }

                #region FSM Event
                protected override void OnEndSelectingTarget(FSM.State newState)
                {
                    base.OnEndSelectingTarget(newState);

                    SetDestinationForPlayer();
                }
                protected override void OnUpdateCasting()
                {
                    if (isPlayer && skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        Utility.FlipUnit(caster, dest);
                }
                protected override void OnEndCasting(FSM.State newState)
                {
                    base.OnEndCasting(newState);

                    if (isPlayer && skillCastingType == SKILL_CASTING_TYPE.CHARGING)
                        SetDestinationForPlayer();
                }
                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Utility.FlipUnit(caster, dest);
                }
                #endregion
                #endregion
            }
            public abstract class VectorSkillData : ActiveSkillData
            {
                [FoldoutGroup("VectorSkill", 3)]

#if UNITY_EDITOR
                [ShowIf("PP_UsePlayerOnly")]
#endif
                [BoxGroup("VectorSkill/�÷��̾� ����", true, true)]
                [LabelText("���� Ÿ��"), Tooltip("�������� ��ġ�� �������� ��� ������ ���������� ���� ��\nCIRCLE : ��ų ������ �������̰� ���� �߽��� �������� ��ġ�� �� Ÿ���� ����\nX_AXIS : �������� ��ġ�� �������� +-��ų ����")]
                public SKILL_RANGE_TYPE rangeType;
#if UNITY_EDITOR
                [ShowIf("PP_UsePlayerOnly")]
#endif
                [BoxGroup("VectorSkill/�÷��̾� ����")]
                [LabelText("���� ���� ��ų"), Tooltip("true : �÷��̾�κ��� ���콺 ������ ������ ���� �ִ� ��Ÿ��� ������\nfalse : ���콺 ������ ��ġ�� ������")]
                public bool isDirVector;
            }
        }
    }
}