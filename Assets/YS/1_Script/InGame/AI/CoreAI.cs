namespace YS
{
    public partial class Core
    {
        public class CoreAI : BaseUnitAI
        {
            public void Initialize(Core core)
            {
                unit = core;

                unit.UpdateEvent += Update;
                unit.OnDisableEvent += OnRelease;

                stateIdle = new State(STATE_INDEX.IDLE, OnIdleEnter, OnIdleUpdate, OnIdleExit);
                stateDie = new State(STATE_INDEX.DIE, OnDieEnter, OnDieUpdate, OnDieExit);
            }
            protected override void OnIdleEnter()
            {
            }

            protected override void OnIdleUpdate()
            {
            }
            protected override void OnIdleExit(State newState)
            {
            }
            protected override void OnDieEnter()
            {
                ChangeState(State.Empty);
            }
            protected override void OnDieUpdate()
            {
            }
            protected override void OnDieExit(State newState)
            {
                unit.Release();
            }

            public override void Start()
            {
                ChangeState(stateIdle);
            }
        }
    }
}