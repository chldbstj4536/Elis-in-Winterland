using UnityEngine;

namespace YS
{
    public partial class Tower
    {
        public class BaseTowerAI : BaseUnitAI
        {
            public State stateSpawn;

            public Tower Tower => unit as Tower;

            public virtual void Initialize(Tower unit)
            {
                base.Initialize(unit);

                stateSpawn = new State(STATE_INDEX.SPAWN, OnSpawnEnter , OnSpawnUpdate, OnSpawnExit);
            }
            public override void Start()
            {
                ChangeState(stateSpawn);
            }
            protected override void OnIdleUpdate()
            {
                base.OnIdleUpdate();

                if (Tower.IsTaunt)  return;

                CastSkill(Tower.defaultAttack);
                foreach (Skill skill in Tower.skills)
                    CastSkill(skill);
            }
            protected virtual void OnSpawnEnter()
            {
                Tower.IsInvincible = true;
                Tower.mainAnimPlayer.SetAnimationSets(Tower.AnimationSet_Spawn, true);
                Tower.mainAnimPlayer.Complete += () =>
                {
                    ChangeState(stateIdle);
                };
                Tower.OnTowerSpawnBeforeEvent?.Invoke(Tower);
            }
            protected virtual void OnSpawnUpdate()
            {

            }
            protected virtual void OnSpawnExit(State newState)
            {
                Tower.IsInvincible = false;
                Tower.OnTowerSpawnAfterEvent?.Invoke(Tower);
            }
        }
    }
}