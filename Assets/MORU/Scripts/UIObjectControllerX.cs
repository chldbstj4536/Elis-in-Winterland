using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Sirenix.OdinInspector;

public class UIObjectControllerX : MonoBehaviour
{
    public List<AnimationTarget> targets = new List<AnimationTarget>();


    [System.Serializable]
    public struct AnimationTarget
    {
        public RectTransform default_Position;
        public GameObject go;
        public enum EAnimationType
        { SlideAndFadeIn = 0 }

        DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> anim;
        //public bool isUpAnimation;
        public float moveDistance;
        public float duration;
        public Ease ease;
        public float delay;

        public void SlideAndFadeIn()
        {
            CanvasGroup canvas;

            if (go.GetComponent<CanvasGroup>())
                canvas = go.GetComponent<CanvasGroup>();
            else
                canvas = go.AddComponent<CanvasGroup>();
            if (anim == null || !anim.IsPlaying())
            {
                anim =
                go.transform
                    .DOMoveX(moveDistance, duration)
                    .SetEase(ease)
                    .From(true)
                    .SetRelative(true)
                    .SetDelay(delay);


                canvas
                    .DOFade(1f, duration)
                    .From(0f)
                    .SetDelay(delay);
            }
        }

    }

    private void Awake()
    {
        targets.ForEach(x =>
        {
        });
    }

    private void OnEnable()
    {
        targets.ForEach(x =>
        {
            x.SlideAndFadeIn();
        });


        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }



}
