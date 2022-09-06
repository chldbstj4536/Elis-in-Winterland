using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class DeactivateByState : MonoBehaviour
    {
        [SerializeField]
        private TITLE_UI_INDEX[] deactiveState;
        private StackUIManager manager;


        [SerializeField, Tooltip("������Ʈ ��ȭ�� ���� ��Ƽ��� �� �ִϸ��̼��� �絿������ ���θ� �����մϴ�.")] 
        private bool isRewinding;
        private bool isActiveInHierarchy => gameObject.activeInHierarchy;

        private void Awake()
        {
            manager = StackUIManager.Instance;
            manager.PushEvent += OnStackChanged;
            manager.PopEvent += OnStackChanged;
        }

        private void OnStackChanged()
        {
            bool changedValue = false;
            for (int i = 0; i < deactiveState.Length; ++i)
            {
                if (manager.StackTop == deactiveState[i])
                {
                    changedValue = false;
                    break;
                }
                else
                    changedValue = true;
            }
            
            if (changedValue)
            {
                if (TryGetComponent<StackUIComponent>(out StackUIComponent comp))
                {
                    if (isActiveInHierarchy && isRewinding)
                    {
                        comp.Show(true);
                        return;
                    }
                    comp.Show(false);
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }

            else
            {
                if (TryGetComponent<StackUIComponent>(out StackUIComponent comp2))
                {
                    comp2.Hide();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}