using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
using Sirenix.OdinInspector;
using static YS.Utility;

namespace YS
{
    public class Test : SerializedMonoBehaviour
    {
        public IHitBox hitbox;
        private void Start()
        {
            hitbox = ConvertColliderToHitbox(GetComponent<Collider>());
        }
        private void OnDrawGizmos()
        {
            hitbox.DrawGizmos(transform.position, transform.rotation);
        }
    }
}