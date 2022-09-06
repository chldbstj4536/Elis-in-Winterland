using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SBuffTower1 : SoulCardEffect
            {
                private float additionalBuffRange;

                public SBuffTower1(SBuffTower1Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    additionalBuffRange = data.additionalBuffRange;
                }
                protected override void InstantEffect(){ }
                protected override void FindUnitsInEffectRange() { }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    foreach (var p in unit.defaultAttack.passiveSkills)
                        p.RangeCoefficient *= additionalBuffRange;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    foreach (var p in unit.defaultAttack.passiveSkills)
                        p.RangeCoefficient /= additionalBuffRange;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SBuffTower1Data : SoulCardEffectData
            {
                [BoxGroup("버프 타워 (1)", true, true), Min(0.0f)]
                [LabelText("버프 범위 증가 비율")]
                public float additionalBuffRange;

#if UNITY_EDITOR
                public SBuffTower1Data()
                {
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = false;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
                }
#endif
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SBuffTower1(this, owner as PlayableUnit);
                }
            }
        }
    }
}