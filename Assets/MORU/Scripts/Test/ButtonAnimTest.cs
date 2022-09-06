using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Moru;

public class ButtonAnimTest //: BaseBehaviour
{
    //[SerializeField] private Button startButton;
    //[SerializeField] private Button twoButton;
    //[SerializeField] private Button threeButton;
    //[SerializeField] private Button fourButton;

    //[SerializeField] private Transform startContainer;
    //[SerializeField] private Transform twoContainer;
    //[SerializeField] private Transform threeContainer;
    //[SerializeField] private Transform fourContainer;

    //private List<Button> buttons = new List<Button>();
    //private List<Sequence> animation_sequences = new List<Sequence>();

    //public UI_CustomAnimate uianim;

    //public float initDelay = 1f;
    //public float delayBetween = 0.3f;

    //private void Awake()
    //{
    //    //startButton?.onClick.AddListener(OnStartPressed);
    //    //twoButton?.onClick.AddListener(OnHighscorePressed);
    //    //threeButton?.onClick.AddListener(OnShopPressed);
    //    //fourButton?.onClick.AddListener(OnSettingPressed);

    //    //buttons.Add(startButton);
    //    //buttons.Add(twoButton);
    //    //buttons.Add(threeButton);
    //    //buttons.Add(fourButton);

    //    //AnimateButtons();
    //    //uianim.ShowAnim();
    //}

    //private void OnDestroy()
    //{
    //    startButton?.onClick.RemoveListener(OnStartPressed);
    //    twoButton?.onClick.RemoveListener(OnHighscorePressed);
    //    threeButton?.onClick.RemoveListener(OnShopPressed);
    //    fourButton?.onClick.RemoveListener(OnSettingPressed);
    //}


    //private void AnimateButtons()
    //{
    //    for(int i = 0; i < 4; i ++)
    //    {
    //        buttons[i].transform.localScale = Vector3.zero;
    //        AnimateButton(i, initDelay + delayBetween * i);
    //    }

    //}

    //private void AnimateButton(int index, float delay)
    //{
    //    if(animation_sequences.Count >= index)
    //    {
    //        animation_sequences.Add(DOTween.Sequence());
    //    }
    //    else
    //    {
    //        if(animation_sequences[index].IsPlaying())
    //        {
    //            animation_sequences[index].Kill(true);
    //        }
    //    }

    //    var seq = animation_sequences[index];
    //    var button = buttons[index];
        
    //    //button.transform.DORotate(new Vector3(0, 0, 572), 5, RotateMode.Fast);
    //    //button.transform.DOScale(new Vector3(1, 1, 1), 3);
    //    seq.Join(button.transform.DOScale(new Vector3(3, 3, 3),3));
    //    seq.Join(button.transform.DORotate(new Vector3(0,0,572),5,RotateMode.Fast));
    //    //seq.Append(button.transform.DOPunchScale(Vector3.one * 0.6f, 0.8f, 6, 0.3f)).SetEase(Ease.OutCirc);
    //    seq.Append(button.transform.DOPunchScale(new Vector3(50, 0, 0), 1));
       
    //   // seq.PrependInterval(delay);
    //    //seq.Complete();
    //}

    //private void OnSettingPressed()
    //{

    //}

    //private void OnShopPressed()
    //{
    //}

    //private void OnHighscorePressed()
    //{
    //}

    //private void OnStartPressed()
    //{
    //}
}
