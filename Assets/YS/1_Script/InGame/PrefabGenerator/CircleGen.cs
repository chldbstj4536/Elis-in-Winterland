using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public abstract class CircleGen : PrefabGenerator
    {
        public float radius;

        protected override Vector3 GetEmitPosition()
        {
            return transform.position + offset + Quaternion.Euler(rot) * Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.up) * new Vector3(Random.Range(0.0f, radius), 0.0f);
        }
        private void OnDrawGizmos()
        {
            Vector3 sp, ep;

            for (int i = 0; i < 60; ++i)
            {
                sp = transform.position + offset + Quaternion.Euler(rot) * Quaternion.AngleAxis(i * 6, Vector3.up) * Vector3.right * radius;
                ep = transform.position + offset + Quaternion.Euler(rot) * Quaternion.AngleAxis((i + 1) * 6, Vector3.up) * Vector3.right * radius;
                Gizmos.color = Color.red;
                Gizmos.DrawLine(sp, ep);
            }
        }
    }
}