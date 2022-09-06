using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SBuffTower2 : SoulCardEffect
            {
                public SBuffTower2(SBuffTower2Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SBuffTower2Data : SoulCardEffectData
            {
#if UNITY_EDITOR
                public SBuffTower2Data()
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
                    return new SBuffTower2(this, owner as PlayableUnit);
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