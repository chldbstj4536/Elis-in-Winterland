using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PTaunt : PassiveSkill
            {
                public PTaunt(PTauntData data, Unit skillOwner) : base(data, skillOwner)
                {
                    restrictionFlag = RESTRICTION_FLAG.TAUNT;
                }

                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.tauntUnit = caster;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.tauntUnit = null;
                }
                public override void AddPassiveSkillToUnit(Unit unit)
                {
                    foreach (var ps in unit.CurrentPSkills.Values)
                        if (ps.pSkill is PTaunt)
                            ps.pSkill.RemovePassiveSkillToUnit(unit);

                    base.AddPassiveSkillToUnit(unit);
                }
            }

            protected abstract class PTauntData : PassiveSkillData
            {
                protected PTauntData()
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
#endif
                }
            }
        }
    }
}