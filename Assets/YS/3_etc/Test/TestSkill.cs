using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class TestSkill : MonoBehaviour
    {
        [SerializeReference]
        public Unit.Skill.BaseSkillData skill;
        public float sphereSize = 0.1f;
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            skill.DrawGizmos(transform.position, transform.rotation);

            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (var hitbox in skill.inRangeHitboxes)
                hits.AddRange(hitbox.Sweep(transform.position, transform.rotation, true, (int)LAYER_MASK.TEAM1));

            Gizmos.color = Color.cyan;

            foreach (var hit in hits)
                Gizmos.DrawSphere(hit.point, sphereSize);
        }
#endif
    }
}