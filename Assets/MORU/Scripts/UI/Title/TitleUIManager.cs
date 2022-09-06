using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YS;
using Moru;
using Sirenix.OdinInspector;
using System;


namespace Moru.UI
{
    public class TitleUIManager : SingleToneMono<TitleUIManager>
    {
        [SerializeField] public MasterySO MasterySO;
        public UserInfo userInfo;

        protected override void Awake()
        {
            base.Awake();
            userInfo = FindObjectOfType<UserInfo>(true);
            if (userInfo == null)
            {
                var userInfo = new GameObject("UserInfo");
                userInfo.transform.SetParent(this.transform);
                this.userInfo = userInfo.AddComponent<UserInfo>();
            }
            DontDestroyOnLoad(this);
        }


        public void Initialized()
        {
            var characterDB = userInfo.Userinfo.GetCurrentCharacterDB;
            var db = characterDB.Data;
        }

        public static void GameOver(string stageName, bool isClear)
        {
            var instance = TitleUIManager.instance;
            instance.userInfo.GameOver(stageName, isClear);
        }

        public void StageSetup()
        {
            var obj = FindObjectOfType<GameResult>();
            GameOver(obj.stage, obj.result);
            Destroy(obj.gameObject);
        }
    }


}