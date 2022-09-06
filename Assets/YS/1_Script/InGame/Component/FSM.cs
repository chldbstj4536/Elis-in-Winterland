namespace YS
{
    public abstract class FSM : PoolingObject
    {
        public delegate void StateEvent();
        public delegate void StateExitEvent(State newState);

        public enum STATE_INDEX
        {
            EMPTY,
            IDLE,
            DIE,
            SELECTING_TARGET,
            CASTING,
            ATTACK,
            SPAWN,
            RUNAWAY
        }
        public struct State
        {
            public State(STATE_INDEX id = STATE_INDEX.EMPTY, StateEvent enterEvent = null, StateEvent updateEvent = null, StateExitEvent exitEvent = null)
            {
                this.id = id;
                OnEnter = enterEvent;
                Update = updateEvent;
                OnExit = exitEvent;
            }

            public static readonly State Empty = new State();
            public readonly STATE_INDEX id;

            public StateEvent OnEnter;
            public StateEvent Update;
            public StateExitEvent OnExit;
        }

        #region Field
        private State curState;
        #endregion

        #region Properties
        public STATE_INDEX CurrentStateIndex => curState.id;
        #endregion

        #region Methods
        protected virtual void Update()
        {
            curState.Update?.Invoke();
        }

        protected virtual void OnRelease()
        {
            Release();
        }
        public void ChangeState(State newState, bool allowRecursion = false)
        {
            if (!allowRecursion && curState.id == newState.id) return;

            //Debug.Log("State Change : " + curState.id + " -> " + newState.id);

            curState.OnExit?.Invoke(newState);
            curState = newState;
            curState.OnEnter?.Invoke();
        }
        #endregion
    }
}