using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SMeleeDA1Hidden : SoulCardEffect
            {
                private readonly float addRange;
                public SMeleeDA1Hidden(SMeleeDA1HiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    addRange = data.addRange;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    caster.defaultAttack.activeSkill.RangeAdditional += addRange;
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SMeleeDA1HiddenData : SoulCardEffectData
            {
                [BoxGroup("근접 기본 공격 I", true, true)]
                [LabelText("추가 사거리"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public float addRange;

                public SMeleeDA1HiddenData()
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
                    return new SMeleeDA1Hidden(this, owner as PlayableUnit);
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