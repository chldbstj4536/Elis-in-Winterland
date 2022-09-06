using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SMeleeDA2 : SoulCardEffect
            {
                private readonly int extraHit;
                private int totalDmg;

                public SMeleeDA2(SMeleeDA2Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    extraHit = 1;
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    caster.defaultAttack.activeSkill.OnAfterAttackEvent += ActiveSkill_OnAfterAttackEvent;
                }
                private void ActiveSkill_OnAfterAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                {
                    this.totalDmg = totalDmg;

                    for (int i = 0; i < extraHit; ++i)
                        Attack(victim, dmgType, hitPos);
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return totalDmg;
                }
            }
            [System.Serializable]
            private class SMeleeDA2Data : SoulCardEffectData
            {
                public SMeleeDA2Data()
                {
#if UNITY_EDITOR
                    ShowPPSetting = false;
                    PP_UseAreaStack = false;
                    PP_UseDefaultTarget = false;
                    PP_UseInRangeHitboxes = false;
                    PP_UseRange = false;
                    PP_UseRemainTimeType = false;
                    PP_UseRestrictionFlag = false;
                    PP_UseTick = false;
                    PP_AllowModifyStack = false;
#endif
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SMeleeDA2(this, owner as PlayableUnit);
                }
            }
        }
    }
}