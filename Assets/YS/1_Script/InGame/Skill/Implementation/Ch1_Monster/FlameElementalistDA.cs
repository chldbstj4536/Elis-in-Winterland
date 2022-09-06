using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class AFlameElementalistDA : TargetingSkill
            {
                #region Field
                private BezierProjectile projectilePrefab;
                #endregion

                #region Methods
                public AFlameElementalistDA(AFlameElementalistDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as BezierProjectile;
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

                    BezierProjectile p = PrefabPool.GetObject(projectilePrefab).GetComponent<BezierProjectile>();
                    Vector3 sp = caster.transform.position;
                    sp.y = 0.5f;
                    p.SetProjectile(this, sp, target, TargetLayerMask, TargetType);
                    p.SetAttackOnArrive(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return caster.TotalMagicPower + PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
                #endregion
            }
            protected class AFlameElementalistDAData : TargetingSkillData
            {
                [BoxGroup("ȭ�� ���ɼ��� �⺻����", true, true)]
                [LabelText("����� ����ü ������"), Tooltip("���� �� ������ ����ü �������� �����մϴ�.")]
                public BezierProjectile projectilePrefab;
                [BoxGroup("ȭ�� ���ɼ��� �⺻����"), Min(0.0001f)]
                [LabelText("����� ����ü �ӵ�")]
                public float projectileSpeed = 1.0f;

                public AFlameElementalistDAData()
                {
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    isDefaultAttack = false;
                }

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AFlameElementalistDA(this, owner);
                }
            }
        }
    }
}