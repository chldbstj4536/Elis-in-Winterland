using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Moru
{
    public partial class UserSaveData
    {
        public partial class Character_DB
        {
            #region Varis

            [ShowInInspector, LabelText("마스터리 데이터")]
            public MasterySO.Vertical_MasteryData MasteryData { 
                get 
                { 
                    return UserSaveData.MasterySO[Index]; 
                } 
            }


            [SerializeField, ShowInInspector, LabelText("현재 선택한 캐릭터의 특성")]      private Mastery_DB current_Selected_Mastery;
            /// <summary>
            /// 현재 선택되어있는 마스터리 데이터를 불러오거나 저장합니다.
            /// </summary>
                                                                                        public Mastery_DB Current_Selected_Mastery
                                                                         { get => current_Selected_Mastery; set => current_Selected_Mastery = value; }

            public int Current_MasteryPoint;
            public int Max_MasteryPoint;
            #endregion

            [System.Serializable]
            public class Mastery_DB
            {
                [BoxGroup("특성 속성"), LabelText("특성 주소")] [ReadOnly] public int adress;
                [BoxGroup("특성 속성"), LabelText("특성 단계")] [ReadOnly] public Define.MasteryLevel Mastery_Level;
                [BoxGroup("특성 속성"), LabelText("특성 타입")] public Define.MasteryType Mastery_Type;
                [BoxGroup("특성 속성"), LabelText("하위 특성")] public MasteryCondition[] Low_Mastery;
                [BoxGroup("특성 속성"), LabelText("상위 특성"), SerializeReference, ReadOnly]  public Mastery_DB High_Mastery;
                [BoxGroup("특성 속성"), LabelText("특성 아이콘")] public Sprite Icon;



                [BoxGroup("특성 속성"), LabelText("특성 이름")] public string MasteryName;
                [BoxGroup("특성 속성"), LabelText("특성 설명")] [TextArea] public string Desc;


                [BoxGroup("특성 상세정보"), LabelText("사용가능한가"), ReadOnly] public bool isUsable = false;
                [BoxGroup("특성 상세정보"), LabelText("최대레벨")] [PropertyRange(1, 5)]              public int MaxLevel = 1;
                [BoxGroup("특성 상세정보"), LabelText("현재레벨")]                                    public int CurrentLevel;
                [BoxGroup("특성 상세정보"), LabelText("특성 데이터 정보")] [ShowInInspector] public YS.Unit.Skill.MasteryEffectData Data;
                [BoxGroup("특성 상세정보"), LabelText("다음 특성레벨 증가 소모값")] [ShowInInspector] public int LevelupCost => 1 + CurrentLevel;

                public Mastery_DB(int adress, Define.MasteryLevel level, Define.MasteryType type)
                {
                    this.adress = adress;
                    Mastery_Level = level;
                    Mastery_Type = type;
                }

                /// <summary>
                /// 개별특성데이터를 초기화합니다.
                /// </summary>
                /// <param name="highMastery"></param>
                public void Initialize()
                {
                    //자신의 하위특성의 데이터에 대해 초기화합니다.
                    if(Low_Mastery != null)
                    {
                        for(int i = 0; i < Low_Mastery.Length; i++)
                        {
                            int _lowLevel_Adress = Low_Mastery[i].target_mastery;
                            var _lowMastery = Moru.MasterySO.Vertical_MasteryData.GetMastery
                                (
                                    UserInfo.instance.Userinfo.Current_PLAYABLE_UNIT_INDEX,
                                    Mastery_Type,
                                    Mastery_Level+1,
                                    _lowLevel_Adress
                                );
                            _lowMastery.High_Mastery = this;
                        }
                    }


                    Update_UsableMastery();
                }

                public void Update_UsableMastery()
                {
                    if(UserInfo.instance.Userinfo.playerLevel >= 
                        UserInfo.instance.Userinfo.GetCurrentCharacterDB.MasteryData.NeedPlayerLevelList[(int)Mastery_Level])
                    {
                        isUsable = true;
                    }
                    else
                    {
                        isUsable = false;
                    }

                }


                /// <summary>
                /// 상위레벨에 대한 조건
                /// </summary>
                [System.Serializable]
                public struct MasteryCondition
                {
                    [PropertyRange(0, 5)]
                    public int Need_Level;
                    [PropertyRange(0,3)] public int target_mastery;
                }
            }
        }
    }
}