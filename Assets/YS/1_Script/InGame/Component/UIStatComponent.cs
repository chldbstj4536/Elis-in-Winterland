using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace YS
{
    [RequireComponent(typeof(TMP_Text))]
    public class UIStatComponent : MonoBehaviour
    {
        private TMP_Text tmp;

        private void Awake()
        {
            tmp = GetComponent<TMP_Text>();
        }

        public void StatUpdate(string newStat)
        {
            tmp.text = newStat;
        }
    }
}