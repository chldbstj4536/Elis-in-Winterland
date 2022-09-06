using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(TMP_Text))]
    public class TMP_FadeEffect : MonoBehaviour
    {
        #region Field
        public float fadeInTime;
        public float remainTime;
        public float fadeOutTime;

        private TMP_Text tmp;
        private float t;
        private Color c;
        private bool bDone = true;

        private float fadeIn_remainTime;
        private float fadeIn_remain_fadeOutTime;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            tmp = GetComponent<TMP_Text>();
            c = tmp.color;
            c.a = 0.0f;
            tmp.color = c;
        }

        private void Update()
        {
            if (!bDone)
            {
                t += Time.deltaTime;

                if (t < fadeInTime)
                    c.a = t / fadeInTime;
                else if (t < fadeIn_remain_fadeOutTime)
                    c.a = 1 - (t - fadeIn_remainTime) / fadeOutTime;
                else if (fadeIn_remain_fadeOutTime < t)
                    bDone = true;

                tmp.color = c;
            }
        }
        #endregion

        #region Methods
        public void FadeEffect(string text)
        {
            tmp.text = text;
            t = 0.0f;
            fadeIn_remainTime = fadeInTime + remainTime;
            fadeIn_remain_fadeOutTime = fadeIn_remainTime + fadeOutTime;
            bDone = false;
        }
        #endregion
    }
}