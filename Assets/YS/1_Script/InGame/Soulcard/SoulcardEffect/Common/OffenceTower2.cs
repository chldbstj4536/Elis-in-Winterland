using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SOffenceTower2 : SoulCardEffect
            {
                private readonly List<uint> maxKnockbackStack = new List<uint>();
                private Dictionary<int, uint> curKnockbackStackMap = new Dictionary<int, uint>();

                public SOffenceTower2(SOffenceTower2Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    maxKnockbackStack = data.maxKnockbackStack;
                }

                protected override void FindUnitsInEffectRange()
                {
                    foreach (var unit in caster.gm.GetAlignUnits())
                        if (unit.UnitType == UNIT_TYPE.TOWER)
                            newUnits.Add(unit);
                }
                protected override void InstantEffect()
                {
                }
                protected override void OnBeginPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnBeginPassiveEffect(unit, areaStack);

                    curKnockbackStackMap.Add(unit.GetInstanceID(), 0);
                    unit.defaultAttack.activeSkill.OnTriggerEvent += () =>
                    {
                        ++curKnockbackStackMap[unit.GetInstanceID()];
                    };
                    unit.defaultAttack.activeSkill.OnAfterAttackEvent += ActiveSkill_OnAfterAttackEvent;
                }
                protected override void OnEndPassiveEffect(Unit unit, int areaStack)
                {
                    base.OnEndPassiveEffect(unit, areaStack);

                    curKnockbackStackMap.Remove(unit.GetInstanceID());
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
                private void ActiveSkill_OnAfterAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                {
                    if (curKnockbackStackMap[attacker.Caster.GetInstanceID()] == maxKnockbackStack[CurrentStack])
                    {
                        curKnockbackStackMap[attacker.Caster.GetInstanceID()] = 0;
                        victim.mc.KnockBack((victim.transform.position - attacker.Caster.transform.position).normalized * 3.0f, 5.0f);
                    }
                }
            }
            [System.Serializable]
            private class SOffenceTower2Data : SoulCardEffectData
            {
                [BoxGroup("공격 타워 (2)", true, true)]
                [LabelText("최대 중첩"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<uint> maxKnockbackStack = new List<uint>();

                public SOffenceTower2Data()
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
                    return new SOffenceTower2(this, owner as PlayableUnit);
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
                    if (maxKnockbackStack.Count > maxStack)
                        maxKnockbackStack.RemoveRange(maxStack, maxKnockbackStack.Count - maxStack);
                    else if (maxKnockbackStack.Count < maxStack)
                        for (int i = maxKnockbackStack.Count; i < maxStack; ++i)
                            maxKnockbackStack.Add(0);
                }
            }
        }
    }
}