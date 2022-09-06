using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Moru;
using Moru.UI;
using TMPro;
using Spine.Unity;

public class UI_LobbyCanvas : MonoBehaviour
{
    bool isfirstOn = true;
    public List<Image> TowerSlots;
    public SkeletonGraphic skeletonGraphic;
    public TextMeshProUGUI CharacterName;
    public TextMeshProUGUI myLevel;

    public string skinName;

    private void OnEnable()
    {
        if(TitleUIManager.instance.userInfo == null || isfirstOn)
        { isfirstOn = false; return; }
        Initialized();
    }

    public void Initialized()
    {
        var UserInfo = TitleUIManager.instance.userInfo.Userinfo;
        for (int i = 0; i < TowerSlots.Count; i++)
        {
            TowerSlots[i].sprite = UserInfo.towers_Info[(YS.TOWER_INDEX)i].Data.icon;
        }

        //skeletonGraphic.skeletonDataAsset = TitleUIManager.instance.skeletonSO.characterSkeleton[(int)UserInfo.Current_Load_Characters_Index];
        //skeletonGraphic.initialSkinName = UserInfo.characters_Info[UserInfo.Current_Load_Characters_Index].currentSkin;
        //skeletonGraphic.Skeleton.SetSkin(UserInfo.characters_Info[UserInfo.Current_Load_Characters_Index].currentSkin);
        //skinName = UserInfo.characters_Info[UserInfo.Current_Load_Characters_Index].currentSkin;
        //skeletonGraphic.Skeleton.SetToSetupPose();
        //CharacterName.text = UserInfo.Current_Load_Characters_Index.ToString();
        //myLevel.text = "Lv. " + UserInfo.playerLevel.ToString();
    }
}
