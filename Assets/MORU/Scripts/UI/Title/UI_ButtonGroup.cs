using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_ButtonGroup : MonoBehaviour
{
    public Button[] buttons;

    public bool isColorChange;
    [ShowIf("isColorChange"), Title("��Ȱ��ȭ �÷��׷�")] public ColorBlock disableColors;
    [ShowIf("isColorChange"), Title("Ȱ��ȭ �÷��׷�")] public ColorBlock ActiveColors;


    [Title("��ư �̹��� ��Ʈ��")]
    public bool IsImage;
    [ShowIf("IsImage"), Title("�����̴�")]
    public ButtonGroup_Slider imageUpdater;


    [Title("�����̴� ��Ʈ��")]
    public bool isSliderAnim;
    [ShowIf("isSliderAnim"), Title("�����̴�")]
    public ButtonGroup_Slider buttonSlider;




    private void Awake()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            int id = i;
            buttons[i].onClick.AddListener(() => OnClick(id));
        }
        buttons[0].onClick.Invoke();
    }

    private void Update()
    {
        if (!isSliderAnim) return;
        if (buttonSlider.currentTarget != buttonSlider.nextTarget)
        {
            buttonSlider.currentTarget = buttonSlider.nextTarget;
            buttonSlider.Animation(buttonSlider.currentTarget);
        }
    }

    public void OnClick(int index)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (isColorChange)
            {
                if (i != index)
                {
                    buttons[i].colors = disableColors;
                }
                else
                {
                    buttons[i].colors = ActiveColors;
                }
            }
        }
    }

    public void OnMouseEnter_event(RectTransform button)
    {
        if (!isSliderAnim) return;
        buttonSlider.nextTarget = button;
    }

    public void OnMouseExit_event(RectTransform button)
    {
        if (!isSliderAnim) return;
        buttonSlider.nextTarget = buttonSlider.PastTarget;
    }


    [System.Serializable]
    public struct ButtonGroup_Slider
    {
        public AnimationCurve animCurve;    //�ִϸ��̼��� �ִϸ��̼� Ű Ŀ��


        public Image Animate_IMG;

        public RectTransform currentTarget;        //���� Anim�� ��ġ�� ��(��ġ�� ������ ��)
        public RectTransform nextTarget;        //������ �̵��ϰ��� �ϴ� ��
        public RectTransform PastTarget;           //������ ���õ��ִ� ��ư

        [Title("Control_XY")]
        public bool isX;
        public bool isY;

        public void Animation(RectTransform target)
        {
            Animate_IMG.StopAllCoroutines();
            Animate_IMG.StartCoroutine(Co_Slide(target));
        }

        IEnumerator Co_Slide(RectTransform target)
        {
            var target_Rect = target.GetComponent<RectTransform>();
            var targetX = target_Rect.anchoredPosition.x;
            var current_Rect = Animate_IMG.GetComponent<RectTransform>();
            var currentX = current_Rect.anchoredPosition.x;
            var currentY = current_Rect.anchoredPosition.y;
            animCurve.keys[0].value = current_Rect.anchoredPosition.x;
            animCurve.keys[1].value = target_Rect.anchoredPosition.x;

            var xmovement =
                target_Rect.anchoredPosition.x - current_Rect.anchoredPosition.x;
            var ymovement =
                target_Rect.anchoredPosition.y - current_Rect.anchoredPosition.y;

            float t = 0;
            while (t < animCurve.keys[1].time)
            {
                current_Rect.anchoredPosition =
                    new Vector2(
                        isX ?
                        currentX + animCurve.Evaluate(t) * xmovement : currentX,
                        isY ?
                        currentY + animCurve.Evaluate(t) * ymovement : currentY);
                t += Time.deltaTime;

                yield return null;
            }

            //Animate_IMG.transform.position = new Vector2(target.transform.position.x, Animate_IMG.transform.position.y);
        }
    }


}
