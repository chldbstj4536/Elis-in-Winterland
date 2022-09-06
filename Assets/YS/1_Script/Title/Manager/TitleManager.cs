using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class TitleManager : SingleToneMono<TitleManager>
    {
        [SerializeField]
        private GameObject selectedUIObj;

        public static GameObject SelectedUIObject => Instance.selectedUIObj;
    }
}