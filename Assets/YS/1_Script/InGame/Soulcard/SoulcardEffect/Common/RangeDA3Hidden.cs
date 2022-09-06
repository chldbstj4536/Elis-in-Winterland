using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SRangeDA3Hidden : SoulCardEffect
            {
                private readonly List<int> stunChance;
                private readonly List<float> stunDuration;

                public SRangeDA3Hidden(SRangeDA3HiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    restrictionFlag = RESTRICTION_FLAG.MOVE | RESTRICTION_FLAG.ATTACK;

                    stunChance = data.stunChance;
                    stunDuration = data.stunDuration;

                    caster.defaultAttack.activeSkill.OnAfterAttackEvent += ActiveSkill_OnAfterAttackEvent;
                }

                private void ActiveSkill_OnAfterAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                {
                    if (Random.Range(0, 100) < stunChance[CurrentStack])
                        AddPassiveSkillToUnit(victim);
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect()
                {
                    Duration = stunDuration[CurrentStack];
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SRangeDA3HiddenData : SoulCardEffectData
            {
                [BoxGroup("원거리 기본 공격 III - 히든", true, true), Range(0, 100)]
                [LabelText("스턴 확률"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<int> stunChance = new List<int>();
                [BoxGroup("원거리 기본 공격 III - 히든"), Min(0.0f)]
                [LabelText("스턴 시간"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> stunDuration = new List<float>();

                public SRangeDA3HiddenData()
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
                    maxStack = 1;
                }
                public override BaseSkill Instantiate(Unit owner)
                {
                    return new SRangeDA3Hidden(this, owner as PlayableUnit);
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
                    if (stunChance.Count > maxStack)
                    {
                        stunChance.RemoveRange(maxStack, stunChance.Count - maxStack);
                        stunDuration.RemoveRange(maxStack, stunDuration.Count - maxStack);
                    }
                    else if (stunChance.Count < maxStack)
                    {
                        for (int i = stunChance.Count; i < maxStack; ++i)
                        {
                            stunChance.Add(0);
                            stunDuration.Add(0.0f);
                        }
                    }
                }
            }
        }
    }
}