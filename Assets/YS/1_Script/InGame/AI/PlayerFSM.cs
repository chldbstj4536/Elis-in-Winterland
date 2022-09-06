namespace YS
{
    public partial class PlayableUnit
    {
        public class PlayerFSM : BaseUnitAI
        {
            public void Initialize(PlayableUnit unit)
            {
                base.Initialize(unit);
            }
            public override void Start()
            {
                ChangeState(stateIdle);
            }
        }
    }
}