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

                            // 가장 가까운 대상의 위치를 목적지로 설정
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
                [BoxGroup("VectorSkill/플레이어 전용", true, true)]
                [LabelText("범위 타입"), Tooltip("시전자의 위치를 기준으로 어떻게 범위를 정의할지에 대한 값\nCIRCLE : 스킬 범위가 반지름이고 원의 중심이 시전자의 위치인 원 타입의 범위\nX_AXIS : 시전자의 위치를 기준으로 +-스킬 범위")]
                public SKILL_RANGE_TYPE rangeType;
#if UNITY_EDITOR
                [ShowIf("PP_UsePlayerOnly")]
#endif
                [BoxGroup("VectorSkill/플레이어 전용")]
                [LabelText("방향 벡터 스킬"), Tooltip("true : 플레이어로부터 마우스 포인터 방향을 향한 최대 사거리가 목적지\nfalse : 마우스 포인터 위치가 목적지")]
                public bool isDirVector;
            }
        }
    }
}