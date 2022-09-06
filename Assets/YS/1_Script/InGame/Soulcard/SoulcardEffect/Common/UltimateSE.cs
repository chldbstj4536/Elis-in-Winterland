using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            public partial class ActiveSkill
            {
                private class SUltimateSE : SoulCardEffect
                {
                    private List<uint> addMaxChargeCount = new List<uint>();

                    public SUltimateSE(SUltimateSEData data, PlayableUnit skillOwner) : base(data, skillOwner)
                    {
                        addMaxChargeCount = data.addMaxChargeCount;
                    }

                    protected override void FindUnitsInEffectRange() { }
                    protected override void InstantEffect()
                    {
                        if (CurrentStack == 0)
                            caster.ultimateSkill.activeSkill.CurrentCharge += (int)addMaxChargeCount[CurrentStack];
                        else
                            caster.ultimateSkill.activeSkill.CurrentCharge += (int)(addMaxChargeCount[CurrentStack] - addMaxChargeCount[CurrentStack - 1]);
                    }
                    protected override int GetTotalDamage(Unit victim)
                    {
                        throw new System.NotImplementedException();
                    }
                }
                [System.Serializable]
                private class SUltimateSEData : SoulCardEffectData
                {
                    [BoxGroup("±Ã±Ø±â Ä«µå", true, true)]
                    [LabelText("±Ã±Ø±â »ç¿ëÈ½¼ö Áõ°¡"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                    public List<uint> addMaxChargeCount = new List<uint>();

                    public SUltimateSEData()
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
                        return new SUltimateSE(this, owner as PlayableUnit);
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
                        if (addMaxChargeCount.Count > maxStack)
                            addMaxChargeCount.RemoveRange(maxStack, addMaxChargeCount.Count - maxStack);
                        else if (addMaxChargeCount.Count < maxStack)
                            for (int i = addMaxChargeCount.Count; i < maxStack; ++i)
                                addMaxChargeCount.Add(0);
                    }
                }
            }
        }
    }
}