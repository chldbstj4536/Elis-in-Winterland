using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SRageOfFrost : SoulCardEffect
            {
                private LinearProjectile projectilePrefab;
                private SphereHitcast hitbox = new SphereHitcast();

                public SRageOfFrost(SRageOfFrostData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    restrictionFlag = RESTRICTION_FLAG.MOVE | RESTRICTION_FLAG.ATTACK;

                    Duration = data.stunDuration;
                    projectilePrefab = data.projectilePrefab;

                    foreach (var skill in skillOwner.Skills)
                    {
                        if (skill.activeSkill is ARageOfFrost rf)
                        {
                            rf.OnFinishEvent += (Vector3 spawnPos, float radius) =>
                            {
                                Vector3 dest = spawnPos;
                                dest.y = 0.0f;
                                
                                Projectile p = projectilePrefab.Instantiate();
                                p.transform.localScale = Vector3.one * (radius * 10);
                                p.SetProjectile(this, spawnPos, dest, 0, 0);
                                p.OnArrive += (Vector3 arrivePos) =>
                                {
                                    hitbox.offset = Vector3.zero;
                                    hitbox.direction = Vector3.down;
                                    hitbox.maxDistance = 1.0f;
                                    hitbox.radius = radius;

                                    var hits = Utility.SweepUnit(hitbox, dest, Quaternion.identity, true, rf.TargetLayerMask, rf.TargetType);

                                    foreach (var hit in hits)
                                    {
                                        Unit unit = hit.transform.GetComponent<Unit>();
                                        AddPassiveSkillToUnit(unit);
                                        //Attack(unit, DAMAGE_TYPE.NORMAL, unit.transform.position);
                                    }
                                };
                            };
                            break;
                        }
                    }
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SRageOfFrostData : SoulCardEffectData
            {
                [BoxGroup("혹한의 분노 소울카드", true, true)]
                [LabelText("스턴 지속시간"), Min(0.0f)]
                public float stunDuration;
                [BoxGroup("혹한의 분노 소울카드")]
                [LabelText("투사체 프리팹")]
                public LinearProjectile projectilePrefab;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SRageOfFrost(this, owner as PlayableUnit);
                }
            }
        }
    }
}