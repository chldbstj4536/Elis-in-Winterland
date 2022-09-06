using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moru
{
    [CreateAssetMenu(fileName = "PlayerEXP_SO", menuName = "Moru/EXP_SO")]
    public class ExpSO : ScriptableObject
    {
        public List<int> Target_Levelup_EXP = new List<int>(99);
    }
}