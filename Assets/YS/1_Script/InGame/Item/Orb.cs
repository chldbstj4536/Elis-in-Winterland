//using UnityEngine;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public struct Orb
//        {
//            // ���� ���� ��ȣ
//            public enum ORB_INDEX
//            {
//                ORB_1,
//                ORB_2,
//                MAX
//            }
//            public enum ORB_GRADE
//            {
//                COMMON,
//                RARE,
//                HERO,
//                LEGEND
//            }


//            // �����ϴ� ������Ʈ�� �÷��̾ ������ ���ӿ�����Ʈ�� Ȱ��ȭ/��Ȱ��ȭ��Ű�� ������� ���� ��
//            #region Field
//            private ORB_INDEX index;
//            private Player owner;
//            private Skill skill;

//            // ������ ������
//            static private readonly OrbData items = ResourceManager.GetResource<OrbData>("Datas/OrbData");
//            #endregion

//            #region Properties
//            public Skill Skill => skill;
//            #endregion

//            #region Methods
//            public Orb(ORB_INDEX index, Player owner)
//            {
//                this.index = index;
//                this.owner = owner;
//                skill = Skill.CreateSkill(items[index].skillIndex, owner);
//            }
//            public void Destroy()
//            {
//                skill.Release();
//            }
//            #endregion
//        }
//    }
//}