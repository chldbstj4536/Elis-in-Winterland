using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [RequireComponent(typeof(MoveComponent))]
    public class LinearProjectile : Unit.Skill.BaseSkill.Projectile
    {
        #region Field
        protected MoveComponent mc;
        #endregion

        #region Properties
        public override float Speed
        {
            get { return mc.MoveSpeed; }
            set { mc.MoveSpeed = value; }
        }
        public override float SpeedCoef
        {
            get { return mc.MoveSpeedCoef; }
            set { mc.MoveSpeedCoef = value; }
        }
        #endregion

        #region Unity Methods
        public override void OnInstantiate()
        {
            mc = GetComponent<MoveComponent>();
            mc.OnArrive += Arrive;
            mc.OnMovingEvent += OnMovingEvent;
        }
        #endregion

        #region Methods
        public override void SetProjectile(Unit.Skill.BaseSkill shooter, Vector3 startPos, Transform target, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
        {
            base.SetProjectile(shooter, startPos, target, layerMaskOnCol, unitFlagOnCol);

            mc.MoveToTarget(target);
        }
        public override void SetProjectile(Unit.Skill.BaseSkill shooter, Vector3 startPos, Vector3 dest, int layerMaskOnCol, UNIT_FLAG unitFlagOnCol)
        {
            base.SetProjectile(shooter, startPos, dest, layerMaskOnCol, unitFlagOnCol);

            mc.MoveToDestination(dest);
        }
        public virtual void OnMovingEvent(Vector3 lastPos, Vector3 curPos)
        {
            FlipMoveDir();
        }
        public void FlipMoveDir()
        {
            if (!mc.IsMoving)
                return;

            Flip(Vector3.Dot(Vector3.right, mc.Direction) <= 0.0f);
        }
        public override Unit.Skill.BaseSkill.Projectile Instantiate(bool active = true)
        {
            LinearProjectile lp = base.Instantiate(active) as LinearProjectile;

            return lp;
        }
        #endregion
    }
}