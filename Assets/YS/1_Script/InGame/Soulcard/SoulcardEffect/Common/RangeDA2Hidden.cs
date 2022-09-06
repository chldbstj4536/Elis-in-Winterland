using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SRangeDA2Hidden : SoulCardEffect
            {
                private readonly List<float> dmgRatioDA;
                private readonly SphereHitcast hitbox;
                private int totalDmg;

                public SRangeDA2Hidden(SRangeDA2HiddenData data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    dmgRatioDA = data.dmgRatioDA;
                    hitbox = data.hitbox.Instantiate() as SphereHitcast;

                    caster.defaultAttack.activeSkill.OnAfterAttackEvent += ActiveSkill_OnAfterAttackEvent;
                }

                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }
                private void ActiveSkill_OnAfterAttackEvent(BaseSkill attacker, Unit victim, int totalDmg, DAMAGE_TYPE dmgType, Vector3 hitPos)
                {
                    this.totalDmg = (int)(totalDmg * dmgRatioDA[CurrentStack]);
                    var hits = Utility.SweepUnit(hitbox, hitPos, Quaternion.identity, true, attacker.TargetLayerMask, attacker.TargetType);
                    foreach (var hitUnit in hits)
                    {
                        victim = hitUnit.transform.GetComponent<Unit>();
                        Attack(victim, dmgType, hitPos);
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    return totalDmg;
                }
            }
            [System.Serializable]
            private class SRangeDA2HiddenData : SoulCardEffectData
            {
                [BoxGroup("원거리 기본 공격 II - 히든", true, true)]
                [LabelText("기본공격 대비 피해량 비율"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<float> dmgRatioDA = new List<float>();
                [BoxGroup("원거리 기본 공격 II - 히든/피해 범위")]
                [HideLabel]
                public SphereHitcast hitbox = new SphereHitcast();

#if UNITY_EDITOR
                public SRangeDA2HiddenData()
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
                    return new SRangeDA2Hidden(this, owner as PlayableUnit);
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
                    if (dmgRatioDA.Count > maxStack)
                        dmgRatioDA.RemoveRange(maxStack, dmgRatioDA.Count - maxStack);
                    else if (dmgRatioDA.Count < maxStack)
                        for (int i = dmgRatioDA.Count; i < maxStack; ++i)
                            dmgRatioDA.Add(0.0f);
                }
            }
        }
    }
}