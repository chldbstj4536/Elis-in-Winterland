using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AIcicleDA : VectorSkill
            {
                #region Field
                private LinearProjectile projectilePrefab;
                #endregion

                public AIcicleDA(AIcicleDAData data, Unit skillOwner) : base(data, skillOwner)
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
                    Vector3 dest;

                    dest = caster.transform.position + (Vector3.up * 0.5f);
                    dest.x = caster.IsLookingLeft ? -30.0f : 30.0f;

                    projectile.SetProjectile(this, caster.transform.position + (Vector3.up * 0.5f), dest, TargetLayerMask, TargetType);
                    projectile.SetAttackOnHit(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return caster.TotalMagicPower + PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            public class AIcicleDAData : VectorSkillData
            {
                [BoxGroup("��帧 Ÿ��", true, true)]
                [LabelText("����ü ������")]
                public LinearProjectile projectilePrefab;
                [BoxGroup("��帧 Ÿ��")]
                [LabelText("����ü �ӵ�")]
                public float projectileSpeed;

                public AIcicleDAData()
                {
                    // �⺻���ݿ���
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AIcicleDA(this, owner);
                }
            }
        }
    }
}