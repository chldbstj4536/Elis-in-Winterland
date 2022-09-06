using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
            /// (�ʱ�ȭ�� ASKILL_NAME_DATA���� ó��)
            /// </summary>
            private class AIceCatapultDA : VectorSkill
            {
                #region Field
                private readonly BezierProjectile projectilePrefab;
                private readonly int physxDmg;
                #endregion

                public AIceCatapultDA(AIceCatapultDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as BezierProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                    physxDmg = data.physxDmg;
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

                    var p = projectilePrefab.Instantiate(true);
                    p.SetProjectile(this, caster.transform.position, dest, TargetLayerMask, TargetType);
                    p.SetAttackOnArrive(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);
                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(physxDmg, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class AIceCatapultDAData : VectorSkillData
            {
                [BoxGroup("���������� �⺻����", true, true)]
                [LabelText("����� ����ü ������"), Tooltip("���� �� ������ ����ü ������")]
                public BezierProjectile projectilePrefab;
                [BoxGroup("���������� �⺻����"), Min(0.0001f)]
                [LabelText("����� ����ü �ӵ�"), Tooltip("����ü�� �ӵ�")]
                public float projectileSpeed = 1.0f;
                [BoxGroup("���������� �⺻����"), Min(0.0001f)]
                [LabelText("�������ط�"), Tooltip("����ü�� ������ �������ط�")]
                public int physxDmg;

                public AIceCatapultDAData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new AIceCatapultDA(this, owner);
                }
            }
        }
    }
}