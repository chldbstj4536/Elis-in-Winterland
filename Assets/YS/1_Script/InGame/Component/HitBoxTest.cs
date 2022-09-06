using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class HitBoxTest : SerializedMonoBehaviour
    {
        public IHitBox[] hitBoxes = new IHitBox[0];

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var hitbox in hitBoxes)
                hitbox.DrawGizmos(transform.position, transform.rotation);
        }
#endif
    }
}