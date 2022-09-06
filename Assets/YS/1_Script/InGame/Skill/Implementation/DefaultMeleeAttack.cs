using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class ADefaultMeleeAttack : TargetingSkill
            {
                #region Field
                private PoolingComponent hitFXPrefab;
                #endregion
                public ADefaultMeleeAttack(ADefaultMeleeAttackData data, Unit skillOwner) : base(data, skillOwner)
                {
                    hitFXPrefab = data.hitFXPrefab;
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

                    Attack(target.GetComponent<Unit>(), DAMAGE_TYPE.NORMAL, target.position);
                    if (hitFXPrefab != null)
                        PrefabPool.GetObject(hitFXPrefab).transform.position = target.position;
                    caster.Flip(target.position.x - caster.transform.position.x < 0.0f);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class ADefaultMeleeAttackData : TargetingSkillData
            {
                [BoxGroup("DefaultMeleeAttack", true, true), LabelText("피격 효과 프리팹")]
                public PoolingComponent hitFXPrefab;

                public ADefaultMeleeAttackData()
                {
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ADefaultMeleeAttack(this, owner);
                }
            }
        }
    }
}