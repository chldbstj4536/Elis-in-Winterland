using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ACrossbowTowerDA : VectorSkill
            {
                #region Field
                private readonly LinearProjectile projectilePrefab;
                #endregion

                public ACrossbowTowerDA(ACrossbowTowerDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as LinearProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                }

                protected override void OnBeginAttack()
                {
                    base.OnBeginAttack();

                    CooltimeStart();

                    Trigger();
                }

                protected override void Trigger()
                {
                    base.Trigger();

                    LinearProjectile projectile = PrefabPool.GetObject(projectilePrefab).GetComponent<LinearProjectile>();
                    Vector3 dest, spos;

                    spos = dest = caster.transform.position + (Vector3.up * 0.5f);
                    dest.x = caster.IsLookingLeft ? -30.0f : 30.0f;

                    projectile.SetProjectile(this, spos, dest, TargetLayerMask, TargetType);
                    projectile.SetAttackOnHit(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            public class ACrossbowTowerDAData : VectorSkillData
            {
                [BoxGroup("석궁 타워", true, true)]
                [LabelText("투사체 프리팹")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("석궁 타워")]
                [LabelText("투사체 속도")]
                public float projectileSpeed;

                public ACrossbowTowerDAData()
                {
                    // 기본공격여부
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ACrossbowTowerDA(this, owner);
                }
            }
        }
    }
}