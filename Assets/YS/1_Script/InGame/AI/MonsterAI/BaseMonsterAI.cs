using UnityEngine;

namespace YS
{
    public partial class Monster
    {
        public class BaseMonsterAI : BaseUnitAI
        {
            public Monster Monster => unit as Monster;

            public virtual void Initialize(Monster unit)
            {
                base.Initialize(unit);
            }
            public override void Start()
            {
                ChangeState(stateIdle);
            }
            protected override void OnIdleUpdate()
            {
                base.OnIdleUpdate();

                if (Monster.IsTaunt)   return;

                bool attackable = false;
                bool movable = !Utility.HasFlag((int)Monster.restrictionFlag, (int)RESTRICTION_FLAG.MOVE);

                if (Monster.defaultAttack != null)
                    attackable = CastSkill(Monster.defaultAttack) != SKILL_ERROR_CODE.NO_TARGET || attackable;
                foreach (Skill skill in Monster.skills)
                    attackable = CastSkill(skill) != SKILL_ERROR_CODE.NO_TARGET || attackable;

                // 공격대상을 공격할 수 있고
                if (attackable)
                {
                    // 움직이는 중이라면 정지
                    if (Monster.mc.IsMoving)
                        Monster.mc.Stop();
                }
                // 아니라면 목적지로 이동
                else if (movable)
                {
                    Monster.mc.MoveToDestination(Vector3.forward * Monster.zLane);

                    // z값을 벗어나면 이동속도의 5배의 속도로 원래 z위치로 이동
                    if (Monster.mc.IsMoving && !Monster.mc.IsInKnockBack)
                    {
                        Vector3 unitPos = Monster.transform.position;
                        float zOffset = unitPos.z - Monster.zLane;
                        float moveAmount = Monster.mc.TotalMoveSpeed * 5.0f * Time.deltaTime;
                        if (zOffset != 0.0f)
                        {
                            if (Mathf.Abs(zOffset) < moveAmount)
                                unitPos.z = Monster.zLane;
                            else
                            {
                                if (zOffset < 0) unitPos.z += moveAmount;
                                else unitPos.z -= moveAmount;
                            }

                            Monster.transform.position = unitPos;
                        }
                    }
                }
                else
                    Monster.mc.Stop();
            }
        }
    }
}