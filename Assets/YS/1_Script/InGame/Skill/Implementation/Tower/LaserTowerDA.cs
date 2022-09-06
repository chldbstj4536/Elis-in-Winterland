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
            /// (�ʱ�ȭ�� ALaserTowerDA_DATA���� ó��)
            /// </summary>
            private class ALaserTowerDA : TargetingSkill
            {
                #region Field
                private readonly LaserTowerProjectile projectilePrefab;
                private readonly float projectileSpeed;
                private readonly SphereHitcast hitbox = new SphereHitcast();
                #endregion
                public ALaserTowerDA(ALaserTowerDAData data, Unit skillOwner) : base(data, skillOwner)
                {
                    projectilePrefab = data.projectilePrefab.Instantiate(false) as LaserTowerProjectile;
                    projectilePrefab.Speed = data.projectileSpeed;
                    hitbox.radius = data.nextTargetRange;
                }
                protected override void Trigger()
                {
                    base.Trigger();

                    var p = projectilePrefab.Instantiate() as LaserTowerProjectile;
                    Vector3 start, dest;
                    float minDist = float.MaxValue;

                    start = dest = target.transform.position;

                    var secTargets = Utility.SweepUnit(hitbox, start, Quaternion.identity, true, TargetLayerMask, TargetType);
                    foreach (var secTarget in secTargets)
                    {
                        if (secTarget.transform == target)
                            continue;
                        
                        float sqrDist = (secTarget.transform.position - start).sqrMagnitude;

                        if (minDist > sqrDist)
                        {
                            minDist = sqrDist;
                            dest = secTarget.transform.position;
                        }
                    }

                    if (minDist == float.MaxValue)
                        dest += (caster.IsLookingLeft ? Vector3.left : Vector3.right) * hitbox.radius;

                    p.SetProjectile(this, start, dest, TargetLayerMask, TargetType);
                    p.SetLaserTowerProjectile(caster.transform.position + Vector3.up);
                    p.SetAttackOnHit(GetTotalDamage, DAMAGE_TYPE.NORMAL, TargetLayerMask, TargetType);

                    CooltimeStart();
                }
                protected override void SetRange()
                {
                    base.SetRange();
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return PhysicsDamageCalc(caster.TotalPhysicsPower, caster.TotalArmorPnt, victim.TotalArmor);
                }
            }
            protected class ALaserTowerDAData : TargetingSkillData
            {
                [BoxGroup("������ Ÿ��", true, true)]
                [LabelText("����ü ������")]
                public LaserTowerProjectile projectilePrefab;
                [BoxGroup("������ Ÿ��")]
                [LabelText("������ �ӵ�")]
                public float projectileSpeed;
                [BoxGroup("������ Ÿ��"), Min(0.0f)]
                [LabelText("���� �� ���� ��� Ž�� ����")]
                public float nextTargetRange;

                public ALaserTowerDAData()
                {
                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // �⺻���ݿ���
                    isDefaultAttack = true;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new ALaserTowerDA(this, owner);
                }
            }
        }
    }
}