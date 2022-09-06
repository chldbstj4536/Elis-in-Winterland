using System.Collections;
using UnityEngine;

namespace YS
{
    public class AutoReleaseParticlePrefab : PoolingComponent
    {
        #region Field
        private ParticleSystem ps;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
            if (ps == null) Destroy(this);
        }
        private void OnEnable()
        {
            StartCoroutine(CheckParticleEnd());
        }
        #endregion

        #region Methods
        IEnumerator CheckParticleEnd()
        {
            WaitForSeconds waitFor50ms = CachedWaitForSeconds.Get(0.05f);
            while (ps.IsAlive(true))
                yield return waitFor50ms;

            Release();
        }
        #endregion
    }
}