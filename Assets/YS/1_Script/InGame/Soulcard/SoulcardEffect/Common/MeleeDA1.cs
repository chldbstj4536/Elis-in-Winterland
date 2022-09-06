using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SMeleeDA1 : SoulCardEffect
            {
                private readonly List<float> physxCoefs;
                private readonly List<float> atkSpdCoefs;

                public SMeleeDA1(SMeleeDA1Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    physxCoefs = data.physxCoefs;
                    atkSpdCoefs = data.atkSpdCoefs;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    if (CurrentStack == 0)
                    {
                        caster.PhysicsPowerCoef *= physxCoefs[CurrentStack];
                        caster.AttackSpeedCoef *= atkSpdCoefs[CurrentStack];
                    }
                    else
                    {
                        caster.PhysicsPowerCoef *= physxCoefs[CurrentStack] / physxCoefs[CurrentStack - 1];
                        caster.AttackSpeedCoef *= atkSpdCoefs[CurrentStack] / atkSpdCoefs[CurrentStack - 1];
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SMeleeDA1Data : SoulCardEffectData
            {
                [BoxGroup("근접 기본 공격 I", true, true)]
                [LabelText("공격력 상승 수치"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> physxCoefs = new List<float>();
                [BoxGroup("근접 기본 공격 I")]
                [LabelText("공격속도 상승 수치"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> atkSpdCoefs = new List<float>();

                public SMeleeDA1Data()
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
#endif
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SMeleeDA1(this, owner as PlayableUnit);
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
                    if (physxCoefs.Count > maxStack)
                    {
                        int removeRange = physxCoefs.Count - maxStack;
                        physxCoefs.RemoveRange(maxStack, removeRange);
                        atkSpdCoefs.RemoveRange(maxStack, removeRange);
                    }
                    else if (physxCoefs.Count < maxStack)
                    {
                        for (int i = physxCoefs.Count; i < maxStack; ++i)
                        {
                            physxCoefs.Add(0.0f);
                            atkSpdCoefs.Add(0.0f);
                        }
                    }
                }
            }
        }
    }
}