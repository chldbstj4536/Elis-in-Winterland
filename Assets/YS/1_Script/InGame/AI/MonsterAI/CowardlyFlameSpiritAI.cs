using UnityEngine;

namespace YS
{
    public partial class Monster
    {
        public class CowardlyFlameSpiritAI : BaseMonsterAI
        {
            protected new Monster unit;
            
            private State stateRunaway;

            private const float backstepRange = 2.5f;
            private const float backstepSpeed = 1.5f;
            private SphereHitcast hitbox = new SphereHitcast(Vector3.up * 20.0f, Vector3.down, 100.0f, backstepRange);

            public override void Initialize(Monster unit)
            {
                base.Initialize(unit);

                this.unit = unit;

                stateRunaway = new State(STATE_INDEX.RUNAWAY, OnRunawayEnter, OnRunawayUpdate, OnRunawayExit);
            }

            protected override void OnIdleUpdate()
            {
                base.OnIdleUpdate();

                if (unit.IsTaunt)   return;

                bool attackable = false;

                // z값을 벗어나면 이동속도의 5배의 속도로 원래 z위치로 이동
                if (unit.mc.IsMoving)
                {
                    Vector3 unitPos = unit.transform.position;
                    float zOffset = unitPos.z - unit.zLane;
                    float moveAmount = unit.mc.TotalMoveSpeed * 5.0f;
                    if (zOffset != 0.0f)
                    {
                        if (Mathf.Abs(zOffset) < moveAmount)
                            unitPos.z = unit.zLane;
                        else
                        {
                            if (zOffset < 0) unitPos.z += moveAmount;
                            else unitPos.z -= moveAmount;
                        }

                        unit.transform.position = unitPos;
                    }
                }

                hitbox.radius = backstepRange;
                var hits = hitbox.Sweep(unit.transform.position, Quaternion.identity, true, (int)LAYER_MASK.TEAM1);
                if (hits.Length != 0)
                {
                    ChangeState(stateRunaway);
                    return;
                }

                attackable = attackable || CastSkill(unit.defaultAttack) != SKILL_ERROR_CODE.NO_TARGET;
                foreach (Skill skill in unit.skills)
                    attackable = attackable || CastSkill(skill) != SKILL_ERROR_CODE.NO_TARGET;

                // 공격대상을 공격할 수 있고
                if (attackable)
                {
                    // 움직이는 중이라면 정지
                    if (unit.mc.IsMoving)
                        unit.mc.Stop();
                }
                // 아니라면 목적지로 이동
                else
                    unit.mc.MoveToDestination(Vector3.forward * unit.zLane);
            }
            protected void OnRunawayEnter()
            {
                unit.mc.MoveToDestination(new Vector3(unit.isLeftSide ? -GameManager.HalfSizeField_X : GameManager.HalfSizeField_X, 0.0f, unit.zLane));
                unit.mc.MoveSpeedCoef += backstepSpeed;
            }
            protected void OnRunawayUpdate()
            {
                // z값을 벗어나면 이동속도의 5배의 속도로 원래 z위치로 이동
                if (unit.mc.IsMoving)
                {
                    Vector3 unitPos = unit.transform.position;
                    float zOffset = unitPos.z - unit.zLane;
                    float moveAmount = unit.mc.TotalMoveSpeed * 5.0f;
                    if (zOffset != 0.0f)
                    {
                        if (Mathf.Abs(zOffset) < moveAmount)
                            unitPos.z = unit.zLane;
                        else
                        {
                            if (zOffset < 0) unitPos.z += moveAmount;
                            else unitPos.z -= moveAmount;
                        }

                        unit.transform.position = unitPos;
                    }
                }

                hitbox.radius = backstepRange;
                var hits = hitbox.Sweep(unit.transform.position, Quaternion.identity, true, (int)LAYER_MASK.TEAM1);
                if (hits.Length == 0)
                {
                    ChangeState(stateIdle);
                    return;
                }

                if (!unit.mc.IsMoving)
                    CastSkill(unit.defaultAttack);
                else
                    Utility.FlipUnitMoveDir(unit);
            }
            protected void OnRunawayExit(State newState)
            {
                unit.mc.MoveSpeedCoef -= backstepSpeed;
            }
        }
    }
}