using System.Collections;
using UnityEngine;

namespace YS
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDeactiveParticle : MonoBehaviour
    {
        #region Field
        public bool bDestroy;
        private ParticleSystem ps;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            ps = GetComponent<ParticleSystem>();
            StartCoroutine(CheckParticleEnd());
        }
        #endregion

        #region Methods
        IEnumerator CheckParticleEnd()
        {
            WaitForSeconds wf50ms = CachedWaitForSeconds.Get(0.05f);

            while (ps.IsAlive(true))
                yield return wf50ms;

            if (bDestroy) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
        #endregion
    }
}