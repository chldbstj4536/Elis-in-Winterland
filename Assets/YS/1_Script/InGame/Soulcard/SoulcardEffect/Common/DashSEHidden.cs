using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SDashSEHidden : SoulCardEffect
            {
                public SDashSEHidden(SDashSEHiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    skillOwner.dashSkill.activeSkill.OnBeginAttackEvent += () => { AddPassiveSkillToUnit(caster); };
                    Duration = data.invincibleTime;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.IsInvincible = true;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.IsInvincible = false;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SDashSEHiddenData : SoulCardEffectData
            {
                [BoxGroup("대쉬기 히든", true, true), Min(0.0f)]
                [LabelText("무적 시간")]
                public float invincibleTime;

                public SDashSEHiddenData()
                {
#if UNITY_EDITOR
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = true;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
                    PP_AllowModifyStack = false;
#endif
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SDashSEHidden(this, owner as PlayableUnit);
                }
            }
        }
    }
}