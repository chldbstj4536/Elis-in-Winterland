using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SBlizzard : SoulCardEffect
            {
                // 해당 스킬에 필요한 변수들 선언 (이펙트, 피해량 등)
                // ★ SBlizzard_DATA와 같아야 한다 (그래야 DATA에 저장된 정보로 초기화 할 수 있다)
                //private List<type> field1;
                // ...

                public SBlizzard(SBlizzardData data, PlayableUnit skillOwner) : base(data, skillOwner)
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
            private class SBlizzardData : SoulCardEffectData
            {
                //[BoxGroup("NAME", true, true)]
                //[LabelText("FIELD_NAME")]
                //public ;

                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SBlizzard(this, owner as PlayableUnit);
                }
            }
        }
    }
}