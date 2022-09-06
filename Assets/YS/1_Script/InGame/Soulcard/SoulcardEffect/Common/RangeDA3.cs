using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract partial class Unit
    {
        public partial class Skill
        {
            private class SRangeDA3 : SoulCardEffect
            {
                private List<uint> projectileCount;
                private float angle;
                private VectorSkill vectorDA;
                private TargetingSkill targetDA;
                private SphereHitcast circleHitcast = new SphereHitcast();
                private BoxHitcast boxHitcast = new BoxHitcast();

                public SRangeDA3(SRangeDA3Data data, PlayableUnit skillOwner) : base(data, skillOwner)
                {
                    if (caster.defaultAttack.activeSkill is VectorSkill)
                    {
                        caster.defaultAttack.activeSkill.OnTriggerEvent += VectorSkill_OnTrigger;
                        vectorDA = caster.defaultAttack.activeSkill as VectorSkill;
                    }
                    else
                    {
                        caster.defaultAttack.activeSkill.OnTriggerEvent += TargetingSkill_OnTrigger;
                        targetDA = caster.defaultAttack.activeSkill as TargetingSkill;
                    }

                    projectileCount = data.projectileCount;
                    angle = data.angle;
                }
                protected override void FindUnitsInEffectRange() { }
                protected override void InstantEffect() { }

                private void VectorSkill_OnTrigger()
                {
                    float curAngle = 0.0f;
                    Vector3 casterPos = caster.transform.position;
                    Vector3 destVec = vectorDA.Destination - casterPos;

                    for (int i = 0; i < projectileCount[CurrentStack]; ++i)
                    {
                        if (i % 2 == 0) curAngle += angle;
                        curAngle = -curAngle;
                        Projectile p = caster.projectileDAPrefab.Instantiate();
                        p.SetProjectile(vectorDA, casterPos, casterPos + Quaternion.AngleAxis(curAngle, Vector3.up) * destVec, vectorDA.TargetLayerMask, vectorDA.TargetType);
                    }
                }
                private void TargetingSkill_OnTrigger()
                {
                    List<RaycastHit> hits = null;
                    uint count = projectileCount[CurrentStack];

                    switch (targetDA.RangeType)
                    {
                        case SKILL_RANGE_TYPE.CIRCLE:
                            circleHitcast.radius = targetDA.TotalRange;
                            hits = Utility.SweepUnit(circleHitcast, caster.transform.position, Quaternion.identity, true, targetDA.TargetLayerMask, targetDA.TargetType);
                            break;
                        case SKILL_RANGE_TYPE.X_AXIS:
                            boxHitcast.halfExtents = new Vector3(targetDA.TotalRange, 5.0f, GameManager.HalfSizeField_Z);
                            hits = Utility.SweepUnit(boxHitcast, caster.transform.position, Quaternion.identity, true, targetDA.TargetLayerMask, targetDA.TargetType);
                            break;
                    }

                    foreach (var hit in hits)
                    {
                        if (hit.transform == targetDA.TargetTransform)
                            continue;

                        if (--count < 0)
                            break;

                        Projectile p = caster.projectileDAPrefab.Instantiate();
                        p.SetProjectile(targetDA, caster.transform.position, hit.transform, targetDA.TargetLayerMask, targetDA.TargetType);
                    }
                }
                protected override int GetTotalDamage(Unit victim)
                {
                    throw new System.NotImplementedException();
                }
            }
            [System.Serializable]
            private class SRangeDA3Data : SoulCardEffectData
            {
                [BoxGroup("원거리 기본 공격 III", true, true)]
                [LabelText("투사체 수 증가량"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
                public List<uint> projectileCount = new List<uint>();
                [BoxGroup("원거리 기본 공격 III")]
                [LabelText("추가 투사체의 각도 변화량"), Range(0.0f, 360.0f)]
                public float angle;

#if UNITY_EDITOR
                public SRangeDA3Data()
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
                    return new SRangeDA3(this, owner as PlayableUnit);
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
                    if (projectileCount.Count > maxStack)
                        projectileCount.RemoveRange(maxStack, projectileCount.Count - maxStack);
                    else if (projectileCount.Count < maxStack)
                        for (int i = projectileCount.Count; i < maxStack; ++i)
                            projectileCount.Add(0);
                }
            }
        }
    }
}