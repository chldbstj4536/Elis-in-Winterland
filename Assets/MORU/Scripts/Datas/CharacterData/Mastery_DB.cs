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

            [ShowInInspector, LabelText("�����͸� ������")]
            public MasterySO.Vertical_MasteryData MasteryData { 
                get 
                { 
                    return UserSaveData.MasterySO[Index]; 
                } 
            }


            [SerializeField, ShowInInspector, LabelText("���� ������ ĳ������ Ư��")]      private Mastery_DB current_Selected_Mastery;
            /// <summary>
            /// ���� ���õǾ��ִ� �����͸� �����͸� �ҷ����ų� �����մϴ�.
            /// </summary>
                                                                                        public Mastery_DB Current_Selected_Mastery
                                                                         { get => current_Selected_Mastery; set => current_Selected_Mastery = value; }

            public int Current_MasteryPoint;
            public int Max_MasteryPoint;
            #endregion

            [System.Serializable]
            public class Mastery_DB
            {
                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� �ּ�")] [ReadOnly] public int adress;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� �ܰ�")] [ReadOnly] public Define.MasteryLevel Mastery_Level;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� Ÿ��")] public Define.MasteryType Mastery_Type;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("���� Ư��")] public MasteryCondition[] Low_Mastery;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("���� Ư��"), SerializeReference, ReadOnly]  public Mastery_DB High_Mastery;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� ������")] public Sprite Icon;



                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� �̸�")] public string MasteryName;
                [BoxGroup("Ư�� �Ӽ�"), LabelText("Ư�� ����")] [TextArea] public string Desc;


                [BoxGroup("Ư�� ������"), LabelText("��밡���Ѱ�"), ReadOnly] public bool isUsable = false;
                [BoxGroup("Ư�� ������"), LabelText("�ִ뷹��")] [PropertyRange(1, 5)]              public int MaxLevel = 1;
                [BoxGroup("Ư�� ������"), LabelText("���緹��")]                                    public int CurrentLevel;
                [BoxGroup("Ư�� ������"), LabelText("Ư�� ������ ����")] [ShowInInspector] public YS.Unit.Skill.MasteryEffectData Data;
                [BoxGroup("Ư�� ������"), LabelText("���� Ư������ ���� �Ҹ�")] [ShowInInspector] public int LevelupCost => 1 + CurrentLevel;

                public Mastery_DB(int adress, Define.MasteryLevel level, Define.MasteryType type)
                {
                    this.adress = adress;
                    Mastery_Level = level;
                    Mastery_Type = type;
                }

                /// <summary>
                /// ����Ư�������͸� �ʱ�ȭ�մϴ�.
                /// </summary>
                /// <param name="highMastery"></param>
                public void Initialize()
                {
                    //�ڽ��� ����Ư���� �����Ϳ� ���� �ʱ�ȭ�մϴ�.
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
                /// ���������� ���� ����
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