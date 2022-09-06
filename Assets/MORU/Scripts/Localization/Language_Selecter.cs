using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Moru.Language
{
    public enum Language
    { Korean, English,None}

    public class Language_Selecter : SingleToneMono<Language_Selecter>
    {
        
        [SerializeField] 
        [OnValueChanged("OnLanguageChange")]
        public Language current_Language;

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
            
            //�÷��̾��� �⺻������������ �޾� ������Ʈ
        }

        
        public void OnLanguageChange()
        {
            var language = current_Language;
            FindObjectOfType<Translate_TextComponent>().SetLanguage(language);
        }
    }
}