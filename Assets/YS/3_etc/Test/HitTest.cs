using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class HitTest : SerializedMonoBehaviour
    {
        public IHitBox hitbox;
        public LAYER_MASK layer;
        public float sphereSize = 0.1f;

        // Update is called once per frame
        void Update()
        {
            hitbox.Sweep(transform.position, transform.rotation, true, (int)layer);

            Debug.Log(hitbox);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            hitbox.DrawGizmos(transform.position, transform.rotation);

            var hits = hitbox.Sweep(transform.position, transform.rotation, false, (int)layer);

            Gizmos.color = Color.cyan;

            foreach (var hit in hits)
                Gizmos.DrawSphere(hit.point, sphereSize);
        }
#endif
    }
}