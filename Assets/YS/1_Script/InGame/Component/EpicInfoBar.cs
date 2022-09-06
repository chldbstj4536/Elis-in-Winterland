using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace YS
{
    public class EpicInfoBar : MonoBehaviour
    {
        public TMP_Text tmp_Name;
        public TMP_Text tmp_HPRatio;
        public Image img_HPBar;

        private GameManager gm;

        private void Awake()
        {
            gm = GameManager.Instance;
        }
        public void AddEpicMonster(Monster epic)
        {
            epic.OnChangedBaseMaxHP += OnUpdateHPBar;
            epic.OnChangedCurrentHP += OnUpdateHPBar;
            OnUpdateHPBar();
        }
        private void OnUpdateHPBar()
        {
            float totalCurHP = 0.0f, totalMaxHP = 0.0f;

            foreach (var epic in gm.GetEpicMonsters())
            {
                totalCurHP += epic.CurrentHP;
                totalMaxHP += epic.MaxHP;
            }

            img_HPBar.fillAmount = totalCurHP / totalMaxHP;
            tmp_HPRatio.text = $"{(int)totalCurHP} / {(int)totalMaxHP}";
        }
    }
}