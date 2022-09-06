namespace YS
{
    public abstract partial class Unit
    {
        public abstract class BaseUnitAI : FSM
        {
            public State stateIdle;
            public State stateDie;
            public State stateSelectingTarget;
            public State stateCasting;
            public State stateAttack;

            protected Unit unit;

            /// <summary>
            /// 기본적인 AI 초기화
            /// </summary>
            /// <param name="unit"></param>
            protected virtual void Initialize(Unit unit)
            {
                this.unit = unit;

                unit.UpdateEvent += Update;
                unit.OnDisableEvent += OnRelease;

                stateIdle = new State(STATE_INDEX.IDLE, OnIdleEnter, OnIdleUpdate, OnIdleExit);
                stateDie = new State(STATE_INDEX.DIE, OnDieEnter, OnDieUpdate, OnDieExit);
                stateSelectingTarget = new State(STATE_INDEX.SELECTING_TARGET);
                stateCasting = new State(STATE_INDEX.CASTING);
                stateAttack = new State(STATE_INDEX.ATTACK);
            }
            public abstract void Start();
            protected virtual void OnIdleEnter()
            {
                unit.moveAnimPlayer.SetAnimationSets(unit.AnimationSet_Move, unit.mc.IsMoving, unit.mc.MoveSpeedCoef);
                unit.stopAnimPlayer.SetAnimationSets(unit.AnimationSet_Idle, !unit.mc.IsMoving);
            }
            protected virtual void OnIdleUpdate()
            {
                // 도발 상태이상에 걸렸다면
                if (unit.IsTaunt)
                {
                    switch (CastSkill(unit.defaultAttack))
                    {
                        case SKILL_ERROR_CODE.INVALID_SKILL:
                        case SKILL_ERROR_CODE.NO_TARGET:
                            if (unit.IsMovable)
                                unit.mc.MoveToTarget(unit.tauntUnit.transform);
                            break;

                        default:
                            // 움직이는 중이라면 정지
                            if (unit.mc.IsMoving)
                                unit.mc.Stop();
                            break;
                    }
                }
            }
            protected virtual void OnIdleExit(State newState) { }
            protected virtual void OnDieEnter()
            {
                unit.mc.Stop();

                unit.mainAnimPlayer.SetAnimationSets(unit.AnimationSet_Die, true);
                unit.mainAnimPlayer.Complete += () =>
                {
                    ChangeState(State.Empty);
                };
            }
            protected virtual void OnDieUpdate() { }
            protected virtual void OnDieExit(State newState)
            {
                unit.mainAnimPlayer.Stop();
                unit.moveAnimPlayer.Stop();
                unit.stopAnimPlayer.Stop();
                unit.Release();
            }

            protected SKILL_ERROR_CODE CastSkill(Skill skill)
            {
                if (skill == null || skill.ASkill == null)
                    return SKILL_ERROR_CODE.INVALID_SKILL;

                return skill.ASkill.CastSkill();
            }
            protected void TauntState()
            {

            }
        }
    }
}