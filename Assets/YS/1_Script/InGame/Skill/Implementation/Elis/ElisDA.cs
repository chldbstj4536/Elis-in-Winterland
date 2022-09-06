using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AElisDA : VectorSkill
            {
                #region Field
                #endregion

                public AElisDA(AElisDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    caster.projectileDAPrefab.SetAttackOnArrive(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
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

                    Projectile p = caster.projectileDAPrefab.Instantiate();
                    Vector3 sp = caster.transform.position;
                    dest.y = 0.0f;
                    sp.y = 1.0f;
                    p.SetProjectile(this, sp, dest, TargetLayerMask, TargetType);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return caster.TotalMagicPower + PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class AElisDAData : VectorSkillData
            {
                public AElisDAData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = true;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AElisDA(this, owner);
                }
            }
        }
    }
}