using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SBreathOfFrost : SoulCardEffect
            {
                public SBreathOfFrost(SBreathOfFrostData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    //field1 = data.field1;
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SBreathOfFrostData : SoulCardEffectData
            {
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SBreathOfFrost(this, owner as PlayableUnit);
                }
            }
        }
    }
}