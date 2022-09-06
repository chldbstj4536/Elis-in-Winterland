using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SRangeDA1Hidden : SoulCardEffect
            {
                private readonly List<float> projectileSpeedCoef;

                public SRangeDA1Hidden(SRangeDA1HiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    projectileSpeedCoef = data.projectileSpeedCoef;
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    if (CurrentStack == 0)
                        caster.projectileDAPrefab.SpeedCoef *= projectileSpeedCoef[CurrentStack];
                    else
                        caster.projectileDAPrefab.SpeedCoef *= projectileSpeedCoef[CurrentStack] / projectileSpeedCoef[CurrentStack - 1];
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SRangeDA1HiddenData : SoulCardEffectData
            {
                [BoxGroup("원거리 기본 공격 I - 히든", true, true)]
                [LabelText("투사체 속도 증가"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> projectileSpeedCoef = new List<float>();

#if UNITY_EDITOR
                public SRangeDA1HiddenData()
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
                    return new SRangeDA1Hidden(this, owner as PlayableUnit);
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
                    if (projectileSpeedCoef.Count > maxStack)
                        projectileSpeedCoef.RemoveRange(maxStack, projectileSpeedCoef.Count - maxStack);
                    else if (projectileSpeedCoef.Count < maxStack)
                        for (int i = projectileSpeedCoef.Count; i < maxStack; ++i)
                            projectileSpeedCoef.Add(0.0f);
                }
            }
        }
    }
}