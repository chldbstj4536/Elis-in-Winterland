using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class ActivateByState : MonoBehaviour
    {
        [SerializeField]
        private TITLE_UI_INDEX[] activeState;


        [SerializeField, Tooltip("������Ʈ ��ȭ�� ���� ��Ƽ��� �� �ִϸ��̼��� �絿������ ���θ� �����մϴ�.")]
        private bool isRewinding;
        private StackUIManager manager;


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

            for (int i = 0; i < activeState.Length; ++i)
            {
                if (manager.StackTop == activeState[i])
                {
                    changedValue = true;
            
                    break;
                }
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