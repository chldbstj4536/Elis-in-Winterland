using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class UnitInfoBar : MonoBehaviour
    {
        public Unit unit;
        public Sprite frameUI;
        public Sprite barUI;
        public Transform hpBar;
        private bool isVisible;

        private GameManager gm;

        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                gameObject.SetActive(isVisible);
            }
        }

        [Button]
        private void SetXAxisCenter()
        {
            Vector3 pos = transform.position;
            pos.x = barUI.rect.width / -200;
            transform.position = pos;
        }

        private void Awake()
        {
            gm = GameManager.Instance;

            unit.OnChangedBaseMaxHP += OnUpdateHPBar;
            unit.OnChangedCurrentHP += OnUpdateHPBar;
            OnUpdateHPBar();
        }

        private void OnUpdateHPBar()
        {
            if (!isVisible)
                return;

            if (gm.Setting.invisibleInFullHP)
            {
                if ((int)unit.CurrentHP == unit.MaxHP)
                {
                    gameObject.SetActive(false);
                    return;
                }
                else
                    gameObject.SetActive(true);
            }

            if (unit.CurrentHP <= 0.0f)
            {
                gameObject.SetActive(false);
                return;
            }
            else
            {
                gameObject.SetActive(true);
                Vector3 scale = hpBar.localScale;
                scale.x = unit.CurrentHP / unit.MaxHP;
                hpBar.localScale = scale;
            }
        }

        public void SetVisible(bool isVisible)
        {
            this.isVisible = isVisible;

            gameObject.SetActive(isVisible);
        }
    }
}