//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace YS
//{
//    [CreateAssetMenu(fileName = "OrbData", menuName = "ScriptableObjects/OrbData", order = 1)]
//    public class OrbData : ScriptableObject
//    {
//        [System.Serializable]
//        public struct Data
//        {
//            public Unit.Orb.ORB_GRADE grade;
//            public Sprite icon;
//            public SKILL_INDEX skillIndex;
//        }

//        [SerializeField]
//        private Data[] orbs;

//        public Data this[Unit.Orb.ORB_INDEX index]
//        {
//            get
//            {
//                return orbs[(int)index];
//            }
//        }
//    }
//}