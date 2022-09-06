using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Sirenix.OdinInspector;

namespace YS
{
    public class PoolingComponent : MonoBehaviour
    {
        [HideInInspector]
        public int prefabID;
        public event Delegate_NoRetVal_NoParam OnInstantiateEvent;
        public virtual void OnInstantiate() { OnInstantiateEvent?.Invoke(); }

        public bool Release()
        {
            return PrefabPool.ReleaseObject(prefabID, gameObject);
        }
    }
}