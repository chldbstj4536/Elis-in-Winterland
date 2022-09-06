using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace Moru
{
    [System.Serializable]
    public class UI_CustomAnimate
    {
        public RectTransform target;
        public List<Sequence> seq = new List<Sequence>();

        public UIAnim_Sequence[] sequence;

        public void Init()
        {
            Animation();
        }

        public void ShowAnim(float preperend_Delay = 0, bool isAnimation = true)
        {
            if (isAnimation)
            {
                Animation();
                for (int i = 0; i < 5; i++)
                {
                    var seq_element = seq[i];
                    //seq_element.Restart();
                }
                Debug.Log("애니메이션 리스타트");
            }
        }

        public void HideAnim(GameObject hiddenObject, bool isAnimation = true)
        {
            Animation();
            for (int i = 0; i < 5; i++)
            {
                var seq_element = seq[i];
                //seq_element.Restart();
            }
            Debug.Log("애니메이션 리스타트");
            foreach (var seq_element in seq)
            {
                seq_element.AppendCallback(() => target.gameObject.SetActive(false));
                //seq_element.Rewind();
            }
            seq[0].AppendCallback(() => hiddenObject.gameObject.SetActive(false));
        }

        private void Animation()
        {
            if (seq.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    var seq_element = DOTween.Sequence();
                    seq_element.SetAutoKill(false);
                    seq.Add(seq_element);
                }
                for (int i = 0; i < sequence.Length; i++)
                {
                    var _seq = sequence[i];
                    if (i == 0)
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            seq[j].PrependInterval(_seq.delay);
                        }
                    }

                    #region Animation
                    if (_seq.isTweening && _seq.tweening.isFrom)
                    {
                        seq[0].Append(

                        target.transform
                            .DOMove(_seq.tweening.fromValue, _seq.duration)
                            .SetEase(_seq.tweening.ease)
                            .From(false)
                            .SetRelative(true)

                            )
                            ;
                    }
                    else if (_seq.isTweening && !_seq.tweening.isFrom)
                    {
                        seq[0].Append(

                        target.transform
                            .DOMove(_seq.tweening.fromValue, _seq.duration)
                            .SetEase(_seq.tweening.ease)
                            .SetRelative(true)


                            )
                            ;
                    }
                    else { seq[0].AppendInterval(_seq.duration); }

                    if (_seq.isRotating)
                    {
                        seq[1].Append(target.transform.DORotate(_seq.rotating.startValue, 0));
                        seq[1].Append(
                        target.transform
                            .DORotate(_seq.rotating.endValue, _seq.duration)
                            .SetEase(_seq.rotating.ease)
                            )
                            ;
                    }
                    else { seq[1].AppendInterval(_seq.duration); }

                    if (_seq.isScaleVelocity)
                    {
                        seq[2].Append(target.transform.DOScale(_seq.scaling.startValue, 0));
                        seq[2].Append(
                        target.transform
                            .DOScale(_seq.scaling.endValue, _seq.duration)
                            .SetEase(_seq.scaling.ease)
                            )
                            ;
                    }
                    else { seq[2].AppendInterval(_seq.duration); }

                    if (_seq.isPunched)
                    {
                        switch (_seq.punched.punchType)
                        {
                            case UIAnim_Sequence.Punched.PunchType.Position:
                                seq[3].Append(
                                target.transform.DOPunchPosition(_seq.punched.punchVector, _seq.duration, _seq.punched.vibrato, _seq.punched.elasticity)
                                );
                                break;
                            case UIAnim_Sequence.Punched.PunchType.Rotation:
                                seq[3].Append(
                                target.transform.DOPunchRotation(_seq.punched.punchVector, _seq.duration, _seq.punched.vibrato, _seq.punched.elasticity)
                                );
                                break;
                            case UIAnim_Sequence.Punched.PunchType.Scale:
                                seq[3].Append(
                                target.transform.DOPunchScale(_seq.punched.punchVector, _seq.duration, _seq.punched.vibrato, _seq.punched.elasticity)
                                );
                                break;
                        }
                    }
                    else { seq[3].AppendInterval(_seq.duration); }

                    if (_seq.isOpacity)
                    {
                        var canvasGroup = target.GetComponent<CanvasGroup>();
                        if (canvasGroup == null)
                        { canvasGroup = target.gameObject.AddComponent<CanvasGroup>(); }
                        seq[4].Append(
                        canvasGroup.DOFade(_seq.opacity.targetValue, _seq.duration).SetEase(_seq.opacity.ease)
                        );
                    }
                    else { seq[4].AppendInterval(_seq.duration); }

                    #endregion
                }
                Debug.Log("애니메이션 쇼잉");
            }

            //else
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        var seq_element = seq[i];
            //        seq_element.Pause();
            //    }
            //    Debug.Log("애니메이션 리스타트");
            //}
        }

        [System.Serializable]
        public struct UIAnim_Sequence
        {
            [LabelText("딜레이 길이"), Tooltip("애님이션 딜레이를 결정")]
            public float delay;
            public float duration;
            public bool isTweening;
            [BoxGroup("Tweening")]
            [ShowIf("isTweening")]
            public Tweening tweening;
            [System.Serializable]
            public struct Tweening
            {
                public Vector2 fromValue;
                public bool isFrom;
                public Ease ease;
            }


            public bool isRotating;
            [BoxGroup("Rotating")]
            [ShowIf("isRotating")]
            public Rotating rotating;
            [System.Serializable]
            public struct Rotating
            {
                public Vector3 startValue;
                public Vector3 endValue;
                public Ease ease;
                public RotateMode rotateMode;
            }

            public bool isScaleVelocity;
            [BoxGroup("ScaleVelocity")]
            [ShowIf("isScaleVelocity")]
            public Scaling scaling;
            [System.Serializable]
            public struct Scaling
            {
                public Vector3 startValue;
                public Vector3 endValue;
                public Ease ease;
                public RotateMode rotateMode;
            }

            public bool isPunched;
            [BoxGroup("Punched")]
            [ShowIf("isPunched")]
            public Punched punched;
            [System.Serializable]
            public struct Punched
            {
                public PunchType punchType;
                public enum PunchType
                { Position, Rotation, Scale }
                public Vector3 punchVector;
                public int vibrato;
                public float elasticity;
            }

            public bool isOpacity;
            [BoxGroup("Opacity")]
            [ShowIf("isOpacity")]
            public Opacity opacity;
            [System.Serializable]
            public struct Opacity
            {
                public float targetValue;
                public float startValue;
                public Ease ease;
            }
        }



    }

}

