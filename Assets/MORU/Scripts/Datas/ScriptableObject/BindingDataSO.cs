using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="BindingDataSO", menuName= "Moru/BindingDataSo")]
public class BindingDataSO : ScriptableObject
{
    [BoxGroup("�� ����Ʈ"), LabelText("������ ���")]
    public List<int> lv_Cost = new List<int>(100);
    #region Common

    #region Stats
    [BoxGroup("���� ������"), LabelText("ü�� ������")] public Sprite Icon_HP;
    [BoxGroup("���� ������"), LabelText("���� ������")] public Sprite Icon_MP;
    [BoxGroup("���� ������"), LabelText("���� ������")] public Sprite Icon_AMROR;
    [BoxGroup("���� ������"), LabelText("���ݷ� ������")] public Sprite Icon_PHYCIS_DMG;
    [BoxGroup("���� ������"), LabelText("������� ������")] public Sprite Icon_ARMOR_PNT;
    [BoxGroup("���� ������"), LabelText("ũ��Ƽ�� ������")] public Sprite Icon_CRITICAL_RATE;
    [BoxGroup("���� ������"), LabelText("���� ������")] public Sprite Icon_LIFE_STEEL;
    [BoxGroup("���� ������"), LabelText("���ݻ�Ÿ� ������")] public Sprite Icon_ATTACKRANGE;
    [BoxGroup("���� ������"), LabelText("���ݼӵ� ������")] public Sprite Icon_ATTACK_SPEED;
    [BoxGroup("���� ������"), LabelText("�ֹ��� ������")] public Sprite Icon_MAGICPOWER;
    [BoxGroup("���� ������"), LabelText("��Ÿ�Ӱ��� ������")] public Sprite Icon_COOLTIME_REDUCE;
    [BoxGroup("���� ������"), LabelText("�̵��ӵ� ������")] public Sprite Icon_MOVE_SPEED;
    [BoxGroup("���� ������"), LabelText("�뽬�ӵ� ������")] public Sprite Icon_DASH_SPEED;
    [BoxGroup("���� ������"), LabelText("�뽬��Ÿ�� ������")] public Sprite Icon_DASH_COOLTIME;
    #endregion

    #region Others
    [BoxGroup("�� �� ������"), LabelText("�����Ҹ� ������")] public Sprite Icon_ManaCost;
    [BoxGroup("�� �� ������"), LabelText("��Ÿ�� ������")] public Sprite Icon_CoolTime;
    [BoxGroup("�� �� ������"), LabelText("�ǹ� ������")] public Sprite Icon_Silver;
    [BoxGroup("�� �� ������"), LabelText("��� ������")] public Sprite Icon_Gold;
    [BoxGroup("�� �� ������"), LabelText("������� ������")] public Sprite Icon_Null;
    [BoxGroup("�� �� ������"), LabelText("�µ���ȭ�� ������")] public Sprite Icon_Temperature;
    #endregion

    #region
    public Color EnableColor;
    public Color DisableColor;
    public Color ZeroColor;
    #endregion

    #endregion
}
