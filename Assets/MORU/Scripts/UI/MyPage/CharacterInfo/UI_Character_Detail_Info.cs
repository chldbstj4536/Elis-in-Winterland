using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YS;
using Sirenix.OdinInspector;
using Spine.Unity;


namespace Moru.UI
{

    public class UI_Character_Detail_Info : MonoBehaviour
    {
    //    //�� ����ĳ����, ���� ����ĳ���� �񱳿�
    //    private YS.PLAYABLE_UNIT_INDEX pastSelectedCharacter = PLAYABLE_UNIT_INDEX.MAX;

    //    ////////////////
    //    ////////////////
    //    //���� ���� �׸�
    //    ////////////////
    //    ////////////////
    //    [SerializeField] private Transform StatsContents;
    //    private List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI HP_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI MANA_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI DMG_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI DMG_PRT_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI CRT_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI LIFESTEEL_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI RANGE_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI ATTACKSPEED_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI MAGIC_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI COOLTIME_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI MOVESPEED_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI DASH_TEXT;
    //    //[SerializeField, BoxGroup("statComps")] private TextMeshProUGUI DASHCOOL_TEXT;

    //    ////////////////
    //    /// ////////////
    //    //��ų���� �׸�//
    //    ////////////////
    //    ////////////////
    //    [SerializeField] UI_Character_Skill skill_Detail_Info;
    //    public UI_Character_Skill Skill_Detail_Info => skill_Detail_Info;

    //    [SerializeField] private Transform SkillContents;
    //    private List<UI_Character_Skill.SkillSlot> Skill_Slot = new List<UI_Character_Skill.SkillSlot>();
    //    public UserSaveData.Skill_DB current_Selected_SKill;

    //    ////////////////
    //    /// ////////////
    //    //��ų���� �׸�//
    //    ////////////////
    //    ////////////////
    //    [SerializeField] UI_Character_Skin skin_Detail_Info;
    //    public UI_Character_Skin Skin_Detail_Info => skin_Detail_Info;


    //    ////////////////
    //    /// ////////////
    //    //��Ų���� �׸�//
    //    ////////////////
    //    ////////////////
    //    [SerializeField] public SkeletonGraphic Character_skeloton;


    //    public void Awake()
    //    {
    //        //������ �Ҵ�
    //        for (int i = 0; i < StatsContents.childCount; i++)
    //        {
    //            var TMPro = StatsContents.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();
    //            texts.Add(TMPro);
    //        }

    //        for (int i = 0; i < SkillContents.childCount; i++)
    //        {
    //            var content = SkillContents.GetChild(i);
    //            Skill_Slot.Add(content.gameObject.AddComponent<UI_Character_Skill.SkillSlot>());
    //        }
    //    }

    //    public void Initialized()
    //    {
    //        ///�������� ������Ʈ
    //        var level = TitleUIManager.instance.userInfo.Userinfo.playerLevel;
    //        var info = TitleUIManager.instance.userInfo.Userinfo.GetLoadCharacterInfo().data;
    //        texts[0].text = (info.baseMaxHP + info.incHpPerLv * level).ToString() + $" (+{ info.baseHPRegen + info.incHPRegenPerLv * level })";
    //        texts[1].text = (info.baseMaxMP + info.incMpPerLv * level).ToString() + $" (+{ info.baseMPRegen + info.incMPRegenPerLv * level })";
    //        texts[2].text = (info.physxPower + info.incPhysxPowerPerLv * level).ToString();
    //        texts[3].text = ((info.armorPnt + info.incArmorPntPerLv * level) * 100).ToString("F0") + "%";
    //        texts[4].text = ((info.criticalRate + info.incCriticalRatePerLv * level) * 100).ToString("F0") + "%";
    //        texts[5].text = ("null") + "%";
    //        texts[6].text = (info.defaultAttackData.SkillData.activeSkillData.range.ToString());
    //        texts[7].text = (info.atkSpd + info.incAtkSpdPerLv * level).ToString();
    //        texts[8].text = (info.magicPower + info.incMagicPowerPerLv * level).ToString();
    //        texts[9].text = ((info.cooldownReduction + info.incCdrPerLv * level) * 100).ToString("F0") + "%";
    //        texts[10].text = (info.moveSpeed + info.incMoveSpeed * level).ToString();
    //        //texts[11].text = (info.dashSpeed).ToString();
    //        texts[12].text = ("null");

    //        //��ų���� ������Ʈ
    //        var SKILLDB = TitleUIManager.instance.userInfo.Userinfo.GetLoadCharacterInfo().SKillDB;
    //        for (int i = 0; i < Skill_Slot.Count; i++)
    //        {
    //            if (SKILLDB == null) { Debug.Log("��ų���� ����"); }
    //            else if (SKILLDB.TryGetValue(i, out var skillValue))
    //            {
    //                Skill_Slot[i].Init(skillValue, this);
    //            }
    //            else
    //            {
    //                Skill_Slot[i].Init(new UserSaveData.Skill_DB(), this);
    //            }
    //        }
    //        //��ų�����õ� ������ ������Ʈ
    //        TitleUIManager.instance.userInfo.Userinfo.GetLoadCharacterInfo().UpdateSkillSet(Define.Slot.A, null);


    //        //���� ĳ���� ���������� ���� ĳ���� �����������ٸ���� �̺�Ʈ�� �ش� ���ǿ��� �ۼ�
    //        if (pastSelectedCharacter != TitleUIManager.instance.userInfo.Userinfo.Current_Load_Characters_Index)
    //        {
    //            pastSelectedCharacter = TitleUIManager.instance.userInfo.Userinfo.Current_Load_Characters_Index;
    //            Skill_Slot[0].OnClicked();
    //        }

    //        ///��Ų���� �ʱ�ȭ
    //        skin_Detail_Info.Initialized();
    //    }


    }


}