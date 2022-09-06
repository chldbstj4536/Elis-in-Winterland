using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            protected abstract class PCurse : PassiveSkill
            {
                protected float totalTakeDmgRate = 1.0f;
                protected float totalDmgRate = 1.0f;
                public PCurse(PCurseData data, Unit skillOwner) : base(data, skillOwner)
                {
                    totalTakeDmgRate = data.totalTakeDmgRate;
                    totalDmgRate = data.totalDmgRate;
                }

                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.TotalTakeDmgRate += totalTakeDmgRate;
                    unit.TotalDmgRate += totalDmgRate;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.TotalTakeDmgRate -= totalTakeDmgRate;
                    unit.TotalDmgRate -= totalDmgRate;
                }
            }

            protected abstract class PCurseData : PassiveSkillData
            {
                [BoxGroup("����", true, true)]
                [LabelText("����� ���ϴ� ���� ���ҷ�")]
                public float totalTakeDmgRate = 1.0f;
                [BoxGroup("����", true, true)]
                [LabelText("����� �޴� ���� ������")]
                public float totalDmgRate = 1.0f;

                protected PCurseData()
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