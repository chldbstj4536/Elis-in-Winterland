//using UnityEngine;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public struct Orb
//        {
//            // 오브 참조 번호
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


//            // 공전하는 오브젝트는 플레이어에 부착된 게임오브젝트를 활성화/비활성화시키는 방식으로 생각 중
//            #region Field
//            private ORB_INDEX index;
//            private Player owner;
//            private Skill skill;

//            // 아이템 정보들
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