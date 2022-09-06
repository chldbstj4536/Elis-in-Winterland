using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public partial class Core : Unit
    {
        private CORE_INDEX index;

        public override UNIT_SIZE Size => UNIT_SIZE.BUILDING;
        public override UNIT_ATTRIBUTE Attribute => UNIT_ATTRIBUTE.MECHANIC;
        public override string Description => "Core";
        public override Sprite Icon => ResourceManager.GetResource<Sprite>("Image/wonsuk");
        public override string Name => "Core";
        public override bool IsLookingLeft => true;
        public override AnimationTrackSet[] AnimationSet_Idle => throw new System.NotImplementedException();
        public override AnimationTrackSet[] AnimationSet_Move => throw new System.NotImplementedException();
        public override AnimationTrackSet[] AnimationSet_Die => throw new System.NotImplementedException();

        public override void Flip(bool isLeft)
        {

        }
        public override void OnInstantiate()
        {
            gm = GameManager.Instance;
            mc = GetComponent<MoveComponent>();
            unitInfoBar = transform.GetChild(1).GetComponent<UnitInfoBar>();

            type = UNIT_TYPE.CORE;
        }

        [System.Serializable]
        public class CoreData : UnitData
        {
            [HideInInspector]
            public CORE_INDEX index;

            public virtual Core Instantiate(int lv, Vector3 spawnPos)
            {
                Core core = base.Instantiate(lv, spawnPos, true) as Core;

                core.index = index;

                core.fsm = ObjectPool.GetObject<CoreAI>();
                (core.fsm as CoreAI).Initialize(core);
                core.fsm.Start();

                return core;
            }
        }
    }
}