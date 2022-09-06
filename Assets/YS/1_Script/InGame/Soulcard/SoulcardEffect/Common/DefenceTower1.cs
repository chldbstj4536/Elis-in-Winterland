using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SDefenceTower1 : SoulCardEffect
            {
                private float reflectionDamageRate;

                public SDefenceTower1(SDefenceTower1Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    reflectionDamageRate = data.reflectionDamageRate;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.OnAfterHitEvent += Unit_OnAfterHitEvent;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
                private void Unit_OnAfterHitEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                {
                    Attack(attacker.Caster, DAMAGE_TYPE.NORMAL, attacker.Caster.transform.position, false, (Unit victim) => { return (int)(totalDmg * reflectionDamageRate); });
                }
            }
            [System.Serializable]
            private class SDefenceTower1Data : SoulCardEffectData
            {
                [BoxGroup("방어 타워 (1)", true, true), Min(0.0f)]
                [LabelText("반사 피해 비율"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public float reflectionDamageRate;

#if UNITY_EDITOR
                public SDefenceTower1Data()
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
                    return new SDefenceTower1(this, owner as PlayableUnit);
                }

                private void BeginDrawListElement(int index)
                {
#if UNITY_EDITOR
                    Sirenix.Utilities.Editor.SirenixEditorGUI.BeginHorizontalPropertyLayout(new GUIContent($"Lv{index + 1}"), out Rect labelRect);
#endif
                }
                private void EndDrawListElement(int index)
                {
#if UNITY_EDITOR
                    Sirenix.Utilities.Editor.SirenixEditorGUI.EndHorizontalPropertyLayout();
#endif
                }

                protected override void OnChangedMaxStack()
                {
                }
            }
        }
    }
}