using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Moru;

namespace YS
{
    public class StackUIComponent : MonoBehaviour
    {
        [SerializeField]
        private TITLE_UI_INDEX index;

        public TITLE_UI_INDEX Index => index;

        public List<UI_CustomAnimate> ShowAnimation;
        public List<UI_CustomAnimate> HideAnimation;

        private void Start()
        {
            if (StackUIManager.Instance == null) return;
            if (index != StackUIManager.Instance.StartIndex)
                gameObject.SetActive(false);

            foreach(var show in ShowAnimation)
            { show.Init(); }
            foreach (var hide in HideAnimation)
            { hide.Init(); }
        }

        public void Show(bool Animating = true)
        {
            if (StackUIManager.Instance.stackAnim.Count != 0)
            {
                var topAnim_Stack = StackUIManager.Instance.stackAnim.Pop();
                foreach (var Anim_ele in topAnim_Stack)
                {
                    foreach (var seq in Anim_ele.seq)
                    {
                        if (!seq.IsComplete())
                        {
                            seq.Complete();
                            seq.Rewind();
                        }
                    }
                }
            }

            gameObject.SetActive(true);
            if (Animating && ShowAnimation.Count != 0)
            {
                StackUIManager.Instance.stackAnim.Push(ShowAnimation);
                for (int i = 0; i < ShowAnimation.Count; i++)
                {
                    ShowAnimation[i].ShowAnim();
                   
                }
            }
        }

        public void Hide(bool Animating = true)
        {

            if (StackUIManager.Instance.stackAnim.Count != 0)
            {
                var topAnim_Stack = StackUIManager.Instance.stackAnim.Pop();
                foreach (var Anim_ele in topAnim_Stack)
                {
                    foreach (var seq in Anim_ele.seq)
                    {
                        if (!seq.IsComplete())
                        {
                            seq.Complete();
                            seq.Rewind();
                        }
                    }
                }
            }


            if (Animating && HideAnimation.Count != 0)
            {
                StackUIManager.Instance.stackAnim.Push(HideAnimation);
                for (int i = 0; i < HideAnimation.Count; i++)
                {
                    HideAnimation[i].HideAnim(this.gameObject);
                }
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

    }
}