using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            /// <summary>
            /// 스킬에 필요한 변수 및 작동 방식만 정의
            /// (초기화는 ALaserTowerDA_DATA에서 처리)
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
                [BoxGroup("레이저 타워", true, true)]
                [LabelText("투사체 프리팹")]
                public LaserTowerProjectile projectilePrefab;
                [BoxGroup("레이저 타워")]
                [LabelText("레이저 속도")]
                public float projectileSpeed;
                [BoxGroup("레이저 타워"), Min(0.0f)]
                [LabelText("공격 후 다음 대상 탐색 범위")]
                public float nextTargetRange;

                public ALaserTowerDAData()
                {
                    // 액티브 스킬의 캐스팅 타입
                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
                    // 기본공격여부
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