using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SOffenceTower1 : SoulCardEffect
            {
                private readonly List<float> physxCoefs;
                private readonly List<float> atkSpdCoefs;

                public SOffenceTower1(SOffenceTower1Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    physxCoefs = data.physxCoefs;
                    atkSpdCoefs = data.atkSpdCoefs;
                }

                protected override void FindUnitsInEffectRange()
                {
                    foreach (var unit in caster.gm.GetAlignUnits())
                        if (unit.UnitType == UNIT_TYPE.TOWER)
                            newUnits.Add(unit);
                }
                protected override void InstantEffect()
                {
                    if (CurrentStack > 1)
                    {
                        foreach (var unit in unitsInEffect.Values)
                        {
                            unit.PhysicsPowerCoef *= physxCoefs[CurrentStack] / physxCoefs[CurrentStack - 1];
                            unit.AttackSpeedCoef *= atkSpdCoefs[CurrentStack] / atkSpdCoefs[CurrentStack - 1];
                        }
                    }
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    unit.PhysicsPowerCoef *= physxCoefs[CurrentStack];
                    unit.AttackSpeedCoef *= atkSpdCoefs[CurrentStack];
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    unit.PhysicsPowerCoef /= physxCoefs[CurrentStack];
                    unit.AttackSpeedCoef /= atkSpdCoefs[CurrentStack];
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SOffenceTower1Data : SoulCardEffectData
            {
                [BoxGroup("공격 타워 (1)", true, true)]
                [LabelText("공격력 상승 수치"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> physxCoefs = new List<float>();
                [BoxGroup("공격 타워 (1)")]
                [LabelText("공격속도 상승 수치"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> atkSpdCoefs = new List<float>();

                public SOffenceTower1Data()
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
                    PP_UseDuration = false;
#endif
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SOffenceTower1(this, owner as PlayableUnit);
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