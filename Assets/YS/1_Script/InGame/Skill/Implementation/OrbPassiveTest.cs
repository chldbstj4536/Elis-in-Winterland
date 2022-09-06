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

//                    // ��ų ��Ÿ�
//                    range = 2.0f;
//                    // �ǰ� ��� ���̾�
//                    targetLayerMask = (int)LAYER_MASK.ENEMY;
//                    // ĳ���� �ִϸ��̼�
//                    animCasting = null;
//                    // ���� �ִϸ��̼�
//                    animAttack = ResourceManager.GetResource<AnimationReferenceAsset>("Unit/PlayerCharacter/Elis/ReferenceAssets/Move_Anticipation");
//                    // ��ų�� Ÿ��
//                    activeSkillType = ACTIVE_SKILL_TYPE.ATTACK;
//                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
//                    skillCastingType = SKILL_CASTING_TYPE.TOGGLE;
//                    // ��ų ��Ÿ��
//                    coolTime = 3.0f;
//                    // ��ų ���
//                    manaCost = 0;
//                    // ��ų ���� ĵ�� �������� ����
//                    bCanCancle = false;
//                    // ���� ��ų ����
//                    bRadialSkill = true;
//                    // ���� ��ų�̶�� ���� �߽����κ��� ���ط��� �޶�������
//                    bSplashSkill = false;
//                    // ĳ������ �̵��ӵ��� ��ȭ��
//                    moveSpdInCasting = 1.0f;
//                    // ������ �̵��ӵ��� ��ȭ��
//                    moveSpdInAttack = 1.0f;
//                }

//                protected override void Trigger()
//                {
//                    // �ߵ� �� �۵��� ����
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

//                    // ��ų ��Ÿ�
//                    range = INFINITE_RANGE;
//                    // �ǰ� ��� ���̾�
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
//                    // �нú� ��ų ��� ������ �� ȿ��
//                    Debug.Log(target.name + " : OrbPassiveTest's OnBeginPassiveEffect()");
//                }
//                protected override void OnEndPassiveEffect(Unit target)
//                {
//                    // �нú� ��ų ��󿡼� ��� �� ȿ��
//                    Debug.Log(target.name + " : OrbPassiveTest's OnEndPassiveEffect()");
//                }
//            }
//        }
//    }
//}