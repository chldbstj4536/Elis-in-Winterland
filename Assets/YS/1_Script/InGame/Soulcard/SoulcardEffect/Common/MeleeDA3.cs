using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public abstract partial class BaseSkill
            {
                private class SMeleeDA3 : SoulCardEffect
                {
                    private readonly List<uint> criticalStack;
                    private int curCriticalStack;

                    public delegate void OnMaxStack(Unit victim);
                    public event OnMaxStack OnMaxStackEvent;

                    public SMeleeDA3(SMeleeDA3Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                    {
                        criticalStack = data.criticalStack;
                        curCriticalStack = 0;

                        caster.defaultAttack.activeSkill.OnBeforeAttackEvent += ActiveSkill_OnBeforeAttackEvent;
                    }
                    private void ActiveSkill_OnBeforeAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                    {
                        if (++curCriticalStack == criticalStack[CurrentStack])
                        {
                            caster.defaultAttack.activeSkill.criticalHit = true;
                            curCriticalStack = 0;
                        }
                    }

                    protected override void FindUnitsInEffectRange() { }
                    protected override void InstantEffect() { }
                    protected override int GetTotalDamage(Unit victim)
                    {
                        throw new System.NotImplementedException();
                    }
                }
                [System.Serializable]
                private class SMeleeDA3Data : SoulCardEffectData
                {
                    [BoxGroup("근거리 기본 공격 III", true, true)]
                    [LabelText("치명타 중첩 횟수"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                    public List<uint> criticalStack = new List<uint>();
                    public SMeleeDA3Data()
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
                        return new SMeleeDA3(this, owner as PlayableUnit);
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
                        if (criticalStack.Count > maxStack)
                            criticalStack.RemoveRange(maxStack, criticalStack.Count - maxStack);
                        else if (criticalStack.Count < maxStack)
                            for (int i = criticalStack.Count; i < maxStack; ++i)
                                criticalStack.Add(0);
                    }
                }
            }
        }
    }
}