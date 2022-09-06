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
                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
                // �� SBlizzard_DATA�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
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