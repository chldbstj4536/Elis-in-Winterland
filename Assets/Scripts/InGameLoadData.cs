using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;
using Moru.UI;

public class InGameLoadData : MonoBehaviour
{
    public string stage;
    public YS.CORE_INDEX coreIndex;
    public YS.PLAYABLE_UNIT_INDEX playerIndex;
    public int playerLv;
    public string playerSkin;
    public YS.EquipTowerInfo[] towerSet;
    public YS.EquipSkillInfo[] skillSet;
    public Moru.UserSaveData.Character_DB.Mastery_DB[] MasterySet;
    public YS.SOULCARD_INDEX[] deck;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        var userdata = TitleUIManager.instance.userInfo.Userinfo;
        stage = userdata.Selected_Stage;
        playerIndex = userdata.Current_PLAYABLE_UNIT_INDEX;
        playerLv = userdata.playerLevel;
        playerSkin = userdata.GetCurrentCharacterDB.current_Skin.Skin_Name;

        towerSet = new YS.EquipTowerInfo[6];
        for (int i = 0; i < towerSet.Length; i++)
        {
            userdata.Tower_Preset[userdata.Current_Preset_Adress].TryGetValue((Define.Slot)i, out UserSaveData.Tower_DB value);
            towerSet[i].ti = value.Data.index;
            towerSet[i].lv = value.Level;
        }

        skillSet = new YS.EquipSkillInfo[6];
        for(int i =0; i < skillSet.Length; i++)
        {
            userdata.GetCurrentCharacterDB.Skill_Preset[userdata.GetCurrentCharacterDB.Current_preset_Adress].TryGetValue((Define.Slot)i, out UserSaveData.Character_DB.Skill_DB value);
            if(value != null)
            {
                skillSet[i].sso = value.Data;
                skillSet[i].lv = value.Preset_Level[userdata.GetCurrentCharacterDB.Current_preset_Adress];

            }
        }
    }
}