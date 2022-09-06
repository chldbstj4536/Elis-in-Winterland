//using UnityEngine;
//using Spine.Unity;
//using System.Collections.Generic;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public abstract partial class Skill
//        {
//            private class OrbPassiveTest : Skill
//            {
//                public OrbPassiveTest()
//                {
//                    activeSkill = new AOrbPassiveTest(this);
//                    passiveSkills.Add(new POrbPassiveTest(this));
//                }

//                public override bool Release()
//                {
//                    return Release<OrbPassiveTest>();
//                }
//            }
//            private class AOrbPassiveTest : ActiveSkill
//            {
//                #region Field
//                #endregion

//                public AOrbPassiveTest(Skill skillOwner) : base(skillOwner) { }

//                public override void Initialize(Unit caster)
//                {
//                    base.Initialize(caster);

//                    // 스킬 사거리
//                    range = 2.0f;
//                    // 피격 대상 레이어
//                    targetLayerMask = (int)LAYER_MASK.ENEMY;
//                    // 캐스팅 애니메이션
//                    animCasting = null;
//                    // 공격 애니메이션
//                    animAttack = ResourceManager.GetResource<AnimationReferenceAsset>("Unit/PlayerCharacter/Elis/ReferenceAssets/Move_Anticipation");
//                    // 스킬의 타입
//                    activeSkillType = ACTIVE_SKILL_TYPE.ATTACK;
//                    // 액티브 스킬의 캐스팅 타입
//                    skillCastingType = SKILL_CASTING_TYPE.TOGGLE;
//                    // 스킬 쿨타임
//                    coolTime = 3.0f;
//                    // 스킬 비용
//                    manaCost = 0;
//                    // 스킬 도중 캔슬 가능한지 여부
//                    bCanCancle = false;
//                    // 범위 스킬 인지
//                    bRadialSkill = true;
//                    // 범위 스킬이라면 공격 중심지로부터 피해량이 달라지는지
//                    bSplashSkill = false;
//                    // 캐스팅중 이동속도의 변화량
//                    moveSpdInCasting = 1.0f;
//                    // 공격중 이동속도의 변화량
//                    moveSpdInAttack = 1.0f;
//                }

//                protected override void Trigger()
//                {
//                    // 발동 시 작동할 로직
//                }
//            }
//            private class POrbPassiveTest : PassiveSkill
//            {
//                #region Field
//                #endregion

//                public POrbPassiveTest(Skill skillOwner) : base(skillOwner) { }

//                public override void Initialize(Unit caster)
//                {
//                    base.Initialize(caster);

//                    // 스킬 사거리
//                    range = INFINITE_RANGE;
//                    // 피격 대상 레이어
//                    targetLayerMask = (int)LAYER_MASK.ENEMY;
//                }
//                protected override bool ConditionOutOfPassive(Unit target)
//                {
//                    return skillOwner.activeSkill.IsCoolTime;
//                }
//                protected override List<Unit> GetUnitsInEffect()
//                {
//                    List<Unit> units = new List<Unit>();

//                    if (!skillOwner.activeSkill.IsCoolTime)
//                        units.Add(caster);

//                    return units;
//                }
//                protected override void OnBeginPassiveEffect(Unit target)
//                {
//                    // 패시브 스킬 대상에 진입할 때 효과
//                    Debug.Log(target.name + " : OrbPassiveTest's OnBeginPassiveEffect()");
//                }
//                protected override void OnEndPassiveEffect(Unit target)
//                {
//                    // 패시브 스킬 대상에서 벗어날 때 효과
//                    Debug.Log(target.name + " : OrbPassiveTest's OnEndPassiveEffect()");
//                }
//            }
//        }
//    }
//}