using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class TestSSOHitBox : MonoBehaviour
    {
        public SkillSO sso;
        public bool checkActiveSkill;
        public bool[] checkPassiveSkill;
        public float sphereSize = 0.1f;
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (checkActiveSkill)
                sso.SkillData.activeSkillData[0].DrawGizmos(transform.position, transform.rotation);
            for (int i = 0; i < sso.SkillData.passiveSkillDatas.Count; ++i)
                if (checkPassiveSkill[i])
                    sso.SkillData.passiveSkillDatas[i][0].DrawGizmos(transform.position, transform.rotation);

            List<RaycastHit> hits = new List<RaycastHit>();
            foreach (var hitbox in sso.SkillData.activeSkillData[0].inRangeHitboxes)
                hits.AddRange(hitbox.Sweep(transform.position, transform.rotation, true, (int)LAYER_MASK.TEAM1));

            Gizmos.color = Color.cyan;

            foreach (var hit in hits)
                Gizmos.DrawSphere(hit.point, sphereSize);
        }
#endif
    }
}