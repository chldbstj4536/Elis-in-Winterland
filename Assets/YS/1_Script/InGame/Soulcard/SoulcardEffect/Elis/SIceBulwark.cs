using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SIceBulwark : SoulCardEffect
            {
                private readonly int damage;
                private readonly AutoReleaseParticlePrefab hitFXPrefab;
                private readonly IHitBox hitbox;

                private readonly AIceBulwark baseSkill;

                public SIceBulwark(SIceBulwarkData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    damage = data.damage;
                    hitFXPrefab = data.hitFXPrefab;
                    hitbox = data.hitbox.Instantiate();

                    foreach (var skill in skillOwner.Skills)
                    {
                        if (skill.ASkill is AIceBulwark)
                        {
                            baseSkill = skill.ASkill as AIceBulwark;
                            break;
                        }
                    }

                    baseSkill.OnDestroyEvent += (BaseShield shield) =>
                    {
                        var fx = PrefabPool.GetObject(hitFXPrefab);
                        fx.transform.position = shield.transform.position;
                        fx.transform.localScale = shield.transform.localScale;

                        var hits = Utility.SweepUnit(hitbox, shield.transform.position, Quaternion.identity, true, TargetLayerMask, TargetType);

                        foreach (var hit in hits)
                        {
                            Vector3 hitPos = hit.transform.position;
                            PrefabPool.GetObject(hitFXPrefab).transform.position = hitPos;
                            Attack(hit.transform.GetComponent<Unit>(), DAMAGE_TYPE.NORMAL, hitPos);
                        }
                    };
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override int GetTotalDamage(Unit victim)
                {
                    return damage;
                }
            }
            [System.Serializable]
            private class SIceBulwarkData : SoulCardEffectData
            {
                [BoxGroup("얼음 방벽 소울카드", true, true)]
                [LabelText("피해량")]
                public int damage;
                [BoxGroup("얼음 방벽 소울카드")]
                [LabelText("피해 이펙트")]
                public AutoReleaseParticlePrefab hitFXPrefab;
                [BoxGroup("얼음 방벽 소울카드/피격 범위", true, true), SerializeReference]
                [HideLabel]
                public IHitBox hitbox;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SIceBulwark(this, owner as PlayableUnit);
                }
            }
        }
    }
}