//using UnityEngine;
//using Sirenix.OdinInspector;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public partial class Skill
//        {
//            /// <summary>
//            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
//            /// (�ʱ�ȭ�� ASKILL_NAME_DATA���� ó��)
//            /// </summary>
//            private class ASKILL_NAME : TypeOfActiveSkill
//            {
//                #region Field
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� ASKILL_NAME_DATA�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                // type field1
//                // type field2
//                // ...
//                #endregion
//                public ASKILL_NAME(ASKILL_NAME_Data data, Unit skillOwner) : base(data, skillOwner)
//                {
//                    // �ش� ��ų�� ������ �������� ASKILL_DATA ������ �ʱ�ȭ
//                    // field1 = data.field1;
//                    // field2 = data.field2;
//                    // ...
//                }

//                //--------------------------------------//
//                // ��ų�� �ʿ��� �̺�Ʈ �������̵�           //
//                //--------------------------------------//
//                // On(Begin/Update/End)SelectingTarget  //
//                // On(Begin/Update/End)Casting          //
//                // On(Begin/Update/End)Attack           //
//                //--------------------------------------//

//                // ...

//                protected override void Trigger()
//                {
//                    // �ߵ� �� �۵��� ����
//                }
//                protected override void SetRange()
//                {
//                    base.SetRange();
//                }
//            }
//            protected class ASKILL_NAME_Data : TypeOfActiveSkillData
//            {
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� ASKILL_NAME�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                // type field1
//                // type field2
//                // ...
//                public ASKILL_NAME_Data()
//                {
//                    // ��Ƽ�� ��ų�� ĳ���� Ÿ��
//                    skillCastingType = SKILL_CASTING_TYPE.INSTANT;
//                    // �⺻���ݿ���
//                    isDefaultAttack = false;
//                }
//                public override BaseSkill Instantiate(Unit owner)
//                {
//                    return new ASKILL_NAME(this, owner);
//                }
//            }
//            /// <summary>
//            /// ��ų�� �ʿ��� ���� �� �۵� ��ĸ� ����
//            /// (�ʱ�ȭ�� PSKILL_NAME_DATA���� ó��)
//            /// </summary>
//            private class PSKILL_NAME : TypeOfPassiveSkill
//            {
//                #region Field
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� PSKILL_NAME_DATA�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                // type field1
//                // type field2
//                // ...
//                #endregion

//                public PSKILL_NAME(PSKILL_NAME_Data data, Unit skillOwner) : base(data, skillOwner)
//                {
//                    // �ش� ��ų�� ������ ������ �ʱ�ȭ
//                    // field1 = data.field1;
//                    // field2 = data.field2;
//                    // ...
//                }
//                protected override void OnBeginPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnEndPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnEmptyInEffect() { }
//                protected override void OnTickPassiveEffect(Unit unit, int tickStack) { }
//            }
//            protected class PSKILL_NAME_Data : TypeOfPassiveSkillData
//            {
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� PSKILL_NAME�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                // [BoxGroup("��ų �̸�", true, true)]
//                // [LabelText("���� �̸�"), Tooltip("���� ����")]
//                // type field1
//                // type field2
//                // ...
//                public PSKILL_NAME_Data()
//                {
//                }
//                public override BaseSkill Instantiate(Unit owner)
//                {
//                    return new PSKILL_NAME(this, owner);
//                }
//            }
//        }
//    }
//}