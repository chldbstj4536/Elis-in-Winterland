using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SSnowball : SoulCardEffect
            {
                public SSnowball(SSnowballData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    ASnowball baseSkill = null;

                    foreach (var skill in skillOwner.skills)
                    {
                        if (skill.ASkill is ASnowball)
                        {
                            baseSkill = skill.ASkill as ASnowball;
                            break;
                        }
                    }

                    baseSkill.snowballPrefab.Release();
                    baseSkill.snowballPrefab = data.enhancedSnowball.Instantiate(false) as SnowballProjectile;
                    baseSkill.isEnhanced = true;
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SSnowballData : SoulCardEffectData
            {
                [BoxGroup("스노우볼 소울카드", true, true)]
                [LabelText("강화 스노우볼 프리팹")]
                public SnowballProjectile enhancedSnowball;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SSnowball(this, owner as PlayableUnit);
                }
            }
        }
    }
}