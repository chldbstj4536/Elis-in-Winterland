using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AProjectileLRAttack : VectorSkill
            {
                #region Field
                private LinearProjectile projectilePrefab;
                #endregion

                public AProjectileLRAttack(AProjectileLRAttackData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as LinearProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    Trigger();
                    CooltimeStart();
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    Vector3 sp = caster.transform.position;
                    sp.y = 0.5f;

                    bool bLeft = dest.x - sp.x < 0.0f;

                    dest = new Vector3(bLeft ? -GameManager.HalfSizeField_X : GameManager.HalfSizeField_X, 0.5f, sp.z);
                    var p = PrefabPool.GetObject(projectilePrefab).GetComponent<LinearProjectile>();
                    p.SetProjectile(this, sp, dest, TargetLayerMask, TargetType);
                    p.SetAttackOnHit(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);

                    caster.Flip(bLeft);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return caster.TotalMagicPower + PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class AProjectileLRAttackData : VectorSkillData
            {
                [BoxGroup("좌우 투사체 공격 스킬", true, true)]
                [LabelText("투사체 프리팹"), Tooltip("공격 시 생성할 투사체 프리팹을 설정합니다.")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("좌우 투사체 공격 스킬")]
                [LabelText("투사체 속도"), Tooltip("공격 시 생성할 투사체 프리팹을 설정합니다.")]
                public float projectileSpeed;

                public AProjectileLRAttackData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AProjectileLRAttack(this, owner);
                }
            }
        }
    }
}