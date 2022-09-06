using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName ="BindingDataSO", menuName= "Moru/BindingDataSo")]
public class BindingDataSO : ScriptableObject
{
    [BoxGroup("값 리스트"), LabelText("레벨업 비용")]
    public List<int> lv_Cost = new List<int>(100);
    #region Common

    #region Stats
    [BoxGroup("스텟 아이콘"), LabelText("체력 아이콘")] public Sprite Icon_HP;
    [BoxGroup("스텟 아이콘"), LabelText("마나 아이콘")] public Sprite Icon_MP;
    [BoxGroup("스텟 아이콘"), LabelText("방어력 아이콘")] public Sprite Icon_AMROR;
    [BoxGroup("스텟 아이콘"), LabelText("공격력 아이콘")] public Sprite Icon_PHYCIS_DMG;
    [BoxGroup("스텟 아이콘"), LabelText("방어관통력 아이콘")] public Sprite Icon_ARMOR_PNT;
    [BoxGroup("스텟 아이콘"), LabelText("크리티컬 아이콘")] public Sprite Icon_CRITICAL_RATE;
    [BoxGroup("스텟 아이콘"), LabelText("흡혈 아이콘")] public Sprite Icon_LIFE_STEEL;
    [BoxGroup("스텟 아이콘"), LabelText("공격사거리 아이콘")] public Sprite Icon_ATTACKRANGE;
    [BoxGroup("스텟 아이콘"), LabelText("공격속도 아이콘")] public Sprite Icon_ATTACK_SPEED;
    [BoxGroup("스텟 아이콘"), LabelText("주문력 아이콘")] public Sprite Icon_MAGICPOWER;
    [BoxGroup("스텟 아이콘"), LabelText("쿨타임감소 아이콘")] public Sprite Icon_COOLTIME_REDUCE;
    [BoxGroup("스텟 아이콘"), LabelText("이동속도 아이콘")] public Sprite Icon_MOVE_SPEED;
    [BoxGroup("스텟 아이콘"), LabelText("대쉬속도 아이콘")] public Sprite Icon_DASH_SPEED;
    [BoxGroup("스텟 아이콘"), LabelText("대쉬쿨타임 아이콘")] public Sprite Icon_DASH_COOLTIME;
    #endregion

    #region Others
    [BoxGroup("그 외 아이콘"), LabelText("마나소모량 아이콘")] public Sprite Icon_ManaCost;
    [BoxGroup("그 외 아이콘"), LabelText("쿨타임 아이콘")] public Sprite Icon_CoolTime;
    [BoxGroup("그 외 아이콘"), LabelText("실버 아이콘")] public Sprite Icon_Silver;
    [BoxGroup("그 외 아이콘"), LabelText("골드 아이콘")] public Sprite Icon_Gold;
    [BoxGroup("그 외 아이콘"), LabelText("비어있음 아이콘")] public Sprite Icon_Null;
    [BoxGroup("그 외 아이콘"), LabelText("온도변화량 아이콘")] public Sprite Icon_Temperature;
    #endregion

    #region
    public Color EnableColor;
    public Color DisableColor;
    public Color ZeroColor;
    #endregion

    #endregion
}
