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
                private class SMeleeDA3Hidden : SoulCardEffect
                {
                    private List<uint> criticalCount;
                    // Dictionary<대상의 ID, 대상 카운트>
                    private Dictionary<int, uint> countMap;

                    public SMeleeDA3Hidden(SMeleeDA3HiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                    {
                        (skillOwner.Soulcards[SOULCARD_INDEX.MELEE_DA_3].Effect as SMeleeDA3).OnMaxStackEvent += SMeleeDA3_OnMaxStack;
                        caster.defaultAttack.activeSkill.OnBeforeAttackEvent += ActiveSkill_OnBeforeAttackEvent;
                    }
                    private void ActiveSkill_OnBeforeAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                    {
                        if (IsUnitInPassive(victim))
                        {
                            attacker.criticalHit = true;

                            if (--countMap[victim.GetInstanceID()] == 0)
                                RemovePassiveSkillToUnit(victim);
                        }
                    }
                    private void SMeleeDA3_OnMaxStack(Unit victim)
                    {
                        if (!IsUnitInPassive(victim))
                            AddPassiveSkillToUnit(victim);
                        else
                            countMap[victim.GetInstanceID()] = criticalCount[CurrentStack];
                    }

                    protected override void FindUnitsInEffectRange() { }
                    protected override void InstantEffect() { }
                    protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                    {
                        base.OnBeginPassiveEffect(unit, areaStack);

                        countMap.Add(unit.GetInstanceID(), criticalCount[CurrentStack]);
                    }
                    protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                    {
                        base.OnEndPassiveEffect(unit, areaStack);

                        countMap.Remove(unit.GetInstanceID());
                    }
                    protected override int GetTotalDamage(Unit victim)
                    {
                        throw new System.NotImplementedException();
                    }
                }
                [System.Serializable]
                private class SMeleeDA3HiddenData : SoulCardEffectData
                {
                    [BoxGroup("근거리 기본 공격 III - 히든", true, true)]
                    [LabelText("치명타 횟수"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                    public List<uint> criticalCount = new List<uint>();
                    public SMeleeDA3HiddenData()
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
                        return new SMeleeDA3Hidden(this, owner as PlayableUnit);
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
                        if (criticalCount.Count > maxStack)
                            criticalCount.RemoveRange(maxStack, criticalCount.Count - maxStack);
                        else if (criticalCount.Count < maxStack)
                            for (int i = criticalCount.Count; i < maxStack; ++i)
                                criticalCount.Add(0);
                    }
                }
            }
        }
    }
}