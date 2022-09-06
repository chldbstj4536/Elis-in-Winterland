using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YS
{
    [RequireComponent(typeof(Image))]
    public class UIBarComponent : MonoBehaviour
    {
        private Image img;

        private void Awake()
        {
            img = GetComponent<Image>();
        }

        public void BarUpdate(float ratio)
        {
            img.fillAmount = ratio;
        }
    }
}