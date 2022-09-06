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
                [BoxGroup("�¿� ����ü ���� ��ų", true, true)]
                [LabelText("����ü ������"), Tooltip("���� �� ������ ����ü �������� �����մϴ�.")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("�¿� ����ü ���� ��ų")]
                [LabelText("����ü �ӵ�"), Tooltip("���� �� ������ ����ü �������� �����մϴ�.")]
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