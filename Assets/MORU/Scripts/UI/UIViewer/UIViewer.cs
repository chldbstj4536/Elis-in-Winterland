using System.Collections;
using System.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Moru.UI
{

    public partial class UIViewer : MonoBehaviour
    {
        private DataBindContext m_Context;
        private ObservableList m_Num;
        private Moru.UserSaveData userData;
        private Moru.UserSaveData UserData
        {
            get
            {
                if (userData == null) { userData = FindObjectOfType<UserInfo>(true).Userinfo; }
                return userData;
            }
        }


        #region binding Propertys

        /// <summary>
        /// 유저 경험치 퍼센트바
        /// </summary>
        public float User_EXP_Percent => UserData.EXP_Percent;




        #endregion



        #region UnityMethods
        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.3f);
            m_Context = FindObjectOfType<DataBindContext>(true);
            if (m_Context == null) { Debug.LogWarning($"DataBindContext가 없습니다."); }
            //m_Num = new ObservableList("UserLevel");
            //m_Context["UserLevel"] = m_Num;
            else
            {
                Init_Character_Bind();
                Init_UserInfo_Bind();
                InitSkillData_Bind();
                InitTowerData_Bind();
                InitMastery_Bind();
                ContentInitialize();
            }

            //델리게이트 등록
            TitleUI_Events.Del_CharacterSKill_Click += InitSkillData_Bind;
            TitleUI_Events.Del_Tower_Click += InitTowerData_Bind;
            TitleUI_Events.Del_UserInfo_Update += Init_UserInfo_Bind;
            TitleUI_Events.Del_CharacterClick += Init_Character_Bind;
            TitleUI_Events.Del_CharacterMastery_Click += InitMastery_Bind;



            //전체델리게이트
            TitleUI_Events.Del_BindAll += InitSkillData_Bind;
            TitleUI_Events.Del_BindAll += InitTowerData_Bind;
            TitleUI_Events.Del_BindAll += Init_UserInfo_Bind;
            TitleUI_Events.Del_BindAll += Init_Character_Bind;
            TitleUI_Events.Del_BindAll += InitMastery_Bind;


            StartCoroutine(BindUpdate());
        }

        /// <summary>
        /// 테스트용
        /// </summary>
        public IEnumerator BindUpdate()
        {
            var UpdateTime = new WaitForSecondsRealtime(0.01f);
            while (true)
            {
                if (m_Context != null)
                {
                    m_Context.BindAll();
                }
                yield return UpdateTime;
            }
        }

        #endregion

        public static void Member_Initialized()
        {
            var m_instance = FindObjectOfType<UIViewer>();
            m_instance.Init_Character_Bind();
        }




        [ContextMenu("Initialized")]
        public void Init_Character_Bind()
        {
            #region #2. 스테이지 관련
            {
                m_Context["SelectStage"] = UserData.Selected_Stage;
                if (UserData.Stages.TryGetValue(UserData.Selected_Stage, out UserSaveData.Stage_DB value))
                {
                    m_Context["CurrentStageName"] = value.StageName;
                    m_Context["CurrentStageScore"] = value.Score;
                    m_Context["CurrentStageStar"] = value.Star;
                }
            }
            #endregion

            #region #3. 캐릭터 관련

            #region #3-1 현재 선택된 캐릭터
            {
                ///////////////////////////////
                //현재 선택한 캐릭터의 기본정보//
                ///////////////////////////////
                m_Context["CurrentCharacter/Name"] = UserData.GetCurrentCharacterDB.Data.name;                                      //캐릭터 이름
                m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;                                   //캐릭터 스켈레톤 데이터
                m_Context["CurrentCharacter/Desc"] = UserData.GetCurrentCharacterDB.Data.desc;                                   //캐릭터 스켈레톤 데이터


                #region #3-2 스텟 데이터
                m_Context["Icon_HP"] = UserSaveData.BindingSO.Icon_HP;
                m_Context["CurrentCharacter/BaseHP"] = UserData.GetCurrentCharacterDB.Data.baseMaxHP;                               //캐릭터 기본체력
                m_Context["CurrentCharacter/BaseHP_PerLV"] = UserData.GetCurrentCharacterDB.Data.incHpPerLv;                        //캐릭터 레벨당 체력 증가량
                m_Context["CurrentCharacter/HP"] = UserData.GetCurrentCharacterDB.Data.baseMaxHP
                                                + UserData.GetCurrentCharacterDB.Data.incHpPerLv * UserData.playerLevel;            //캐릭터 최종 체력


                m_Context["Icon_MP"] = UserSaveData.BindingSO.Icon_MP;
                m_Context["CurrentCharacter/BaseHP_Regen"] = UserData.GetCurrentCharacterDB.Data.baseHPRegen;                       //캐릭터 기본체력재생량
                m_Context["CurrentCharacter/BaseHP_Regen_PerLV"] = UserData.GetCurrentCharacterDB.Data.incHPRegenPerLv;             //캐릭터 기본체력재생량 레벨당 증가량
                m_Context["CurrentCharacter/HP_Regen"] = UserData.GetCurrentCharacterDB.Data.baseHPRegen
                                                        + UserData.GetCurrentCharacterDB.Data.incHPRegenPerLv * UserData.playerLevel; //캐릭터 최종 체력재생량


                m_Context["CurrentCharacter/BaseMP"] = UserData.GetCurrentCharacterDB.Data.baseMaxMP;                               //캐릭터 기본마나
                m_Context["CurrentCharacter/BaseMP_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMpPerLv;                        //캐릭터 레벨당 마나 증가량
                m_Context["CurrentCharacter/MP"] = UserData.GetCurrentCharacterDB.Data.baseMaxMP
                                                + UserData.GetCurrentCharacterDB.Data.incMpPerLv * UserData.playerLevel;            //캐릭터 최종 마나


                m_Context["CurrentCharacter/BaseMP_Regen"] = UserData.GetCurrentCharacterDB.Data.baseMPRegen;                       //캐릭터 기본마나재생량
                m_Context["CurrentCharacter/BaseHP_Regen_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMPRegenPerLv;             //캐릭터 기본마나재생량 레벨당 증가량
                m_Context["CurrentCharacter/MP_Regen"] = UserData.GetCurrentCharacterDB.Data.baseMPRegen
                                                        + UserData.GetCurrentCharacterDB.Data.incMPRegenPerLv * UserData.playerLevel; //캐릭터 최종 마나재생량


                m_Context["Icon_AMROR"] = UserSaveData.BindingSO.Icon_AMROR;
                m_Context["CurrentCharacter/Base_Armor"] = UserData.GetCurrentCharacterDB.Data.armor * 100;                       //캐릭터 기본 방어력
                m_Context["CurrentCharacter/Base_ArmorInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incArmorPerLv;             //캐릭터 기본마나재생량 레벨당 증가량
                m_Context["CurrentCharacter/Armor"] = (UserData.GetCurrentCharacterDB.Data.armor
                                                        + UserData.GetCurrentCharacterDB.Data.incArmorPerLv * UserData.playerLevel) * 100; //캐릭터 최종 마나재생량


                m_Context["Icon_PHYCIS_DMG"] = UserSaveData.BindingSO.Icon_PHYCIS_DMG;
                m_Context["CurrentCharacter/Base_PhysicsPower"] = UserData.GetCurrentCharacterDB.Data.physxPower;                       //캐릭터 기본 물리공격력
                m_Context["CurrentCharacter/Base_PhysicsPowerInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incPhysxPowerPerLv;      //캐릭터 기본 물리공격력 레벨당 증가량
                m_Context["CurrentCharacter/PhysicsPower"] = UserData.GetCurrentCharacterDB.Data.physxPower
                                                        + UserData.GetCurrentCharacterDB.Data.incPhysxPowerPerLv * UserData.playerLevel; //캐릭터 최종 물리공격력


                m_Context["Icon_ARMOR_PNT"] = UserSaveData.BindingSO.Icon_ARMOR_PNT;
                m_Context["CurrentCharacter/Base_ArmorPnt"] = UserData.GetCurrentCharacterDB.Data.armorPnt * 100;                             //캐릭터 기본 방어관통력 (%)
                m_Context["CurrentCharacter/Base_ArmorPntInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incArmorPntPerLv * 100;            //캐릭터 기본 방어관통력 레벨당 증가량 (%)
                m_Context["CurrentCharacter/ArmorPnt"] = (UserData.GetCurrentCharacterDB.Data.armorPnt
                                                        + UserData.GetCurrentCharacterDB.Data.incArmorPntPerLv * UserData.playerLevel) * 100; //캐릭터 최종 방어관통력


                m_Context["Icon_CRITICAL_RATE"] = UserSaveData.BindingSO.Icon_CRITICAL_RATE;
                m_Context["CurrentCharacter/Base_CriticalRate"] = UserData.GetCurrentCharacterDB.Data.criticalRate * 100;                             //캐릭터 기본 치명타율(%)
                m_Context["CurrentCharacter/Base_CriticalRateInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incCriticalRatePerLv * 100;            //캐릭터 기본 치명타율 레벨당 증가량 (%)
                m_Context["CurrentCharacter/CriticalRate"] = (UserData.GetCurrentCharacterDB.Data.criticalRate
                                                        + UserData.GetCurrentCharacterDB.Data.incCriticalRatePerLv * UserData.playerLevel) * 100; //캐릭터 최종 치명타율


                m_Context["Icon_LIFE_STEEL"] = UserSaveData.BindingSO.Icon_LIFE_STEEL;
                m_Context["CurrentCharacter/Base_BloodSteel"] = UserData.GetCurrentCharacterDB.Data.criticalRate * 100;                             //캐릭터 기본 흡혈율(%)
                m_Context["CurrentCharacter/Base_BloodSteelInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incCriticalRatePerLv * 100;            //캐릭터 기본 흡혈율 레벨당 증가량 (%)
                m_Context["CurrentCharacter/BloodSteel"] = (UserData.GetCurrentCharacterDB.Data.criticalRate
                                                        + UserData.GetCurrentCharacterDB.Data.incCriticalRatePerLv * UserData.playerLevel) * 100; //캐릭터 최종 흡혈율


                m_Context["Icon_ATTACK_RANGE"] = UserSaveData.BindingSO.Icon_ATTACKRANGE;
                if (UserData.GetCurrentCharacterDB.Data.defaultAttackData != null)
                {
                    // 기존 코드
                    //m_Context["CurrentCharacter/AttackRange"] = UserData.GetCurrentCharacterDB.Data.defaultAttackData.SkillData.activeSkillData.range;  //캐릭터 기본공격 사거리
                    // 수정 코드
                    m_Context["CurrentCharacter/AttackRange"] = UserData.GetCurrentCharacterDB.Data.defaultAttackData.SkillData.activeSkillData[0].range;  //캐릭터 기본공격 사거리
                }
                else { m_Context["CurrentCharacter/AttackRange"] = 0; }

                m_Context["Icon_ATTACK_SPEED"] = UserSaveData.BindingSO.Icon_ATTACK_SPEED;
                m_Context["CurrentCharacter/Base_AttackSpeed"] = UserData.GetCurrentCharacterDB.Data.atkSpd;                                //캐릭터 기본 공격속도
                m_Context["CurrentCharacter/Base_AttackSpeedInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incAtkSpdPerLv;               //캐릭터 기본 공격속도 레벨당 증가량 (%)
                m_Context["CurrentCharacter/AttackSpeed"] = (UserData.GetCurrentCharacterDB.Data.atkSpd
                                                        + UserData.GetCurrentCharacterDB.Data.incAtkSpdPerLv * UserData.playerLevel);      //캐릭터 최종 공격속도


                m_Context["Icon_MAGICPOWER"] = UserSaveData.BindingSO.Icon_MAGICPOWER;
                m_Context["CurrentCharacter/Base_MagicPower"] = UserData.GetCurrentCharacterDB.Data.magicPower;                             //캐릭터 기본 주문력
                m_Context["CurrentCharacter/Base_MagicPowerInc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMagicPowerPerLv;            //캐릭터 기본 주문력 레벨당 증가량 (%)
                m_Context["CurrentCharacter/MagicPower"] = (UserData.GetCurrentCharacterDB.Data.magicPower
                                                        + UserData.GetCurrentCharacterDB.Data.incMagicPowerPerLv * UserData.playerLevel);   //캐릭터 최종 주문력


                m_Context["Icon_COOLTIME_REDUCE"] = UserSaveData.BindingSO.Icon_COOLTIME_REDUCE;
                m_Context["CurrentCharacter/Base_Cool_Reduce"] = UserData.GetCurrentCharacterDB.Data.cooldownReduction * 100;                             //캐릭터 기본 쿨타임감소율(%)
                m_Context["CurrentCharacter/Base_Cool_Reduce_Inc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incCdrPerLv * 100;            //캐릭터 기본 쿨타임감소율 레벨당 증가량 (%)
                m_Context["CurrentCharacter/Cool_Reduce"] = (UserData.GetCurrentCharacterDB.Data.cooldownReduction
                                                        + UserData.GetCurrentCharacterDB.Data.incCdrPerLv * UserData.playerLevel) * 100;   //캐릭터 최종 쿨타임감소율(%)


                m_Context["Icon_MOVESPEED"] = UserSaveData.BindingSO.Icon_MOVE_SPEED;
                m_Context["CurrentCharacter/Base_MoveSpeed"] = UserData.GetCurrentCharacterDB.Data.moveSpeed;                             //캐릭터 기본 이동속도
                m_Context["CurrentCharacter/Base_MoveSpeed_Inc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMoveSpeed;            //캐릭터 기본 이동속도 레벨당 증가량 (%)
                m_Context["CurrentCharacter/MoveSpeed"] = (UserData.GetCurrentCharacterDB.Data.moveSpeed
                                                        + UserData.GetCurrentCharacterDB.Data.incMoveSpeed * UserData.playerLevel);   //캐릭터 최종 이동속도


                m_Context["Icon_DASH_SPEED"] = UserSaveData.BindingSO.Icon_DASH_SPEED;
                m_Context["CurrentCharacter/Base_DashSpeed"] = UserData.GetCurrentCharacterDB.Data.moveSpeed;                             //캐릭터 기본 대시속도
                m_Context["CurrentCharacter/Base_DashSpeed_Inc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMoveSpeed;            //캐릭터 기본 대시속도 레벨당 증가량 (%)
                m_Context["CurrentCharacter/DashSpeed"] = (UserData.GetCurrentCharacterDB.Data.moveSpeed
                                                        + UserData.GetCurrentCharacterDB.Data.incMoveSpeed * UserData.playerLevel);   //캐릭터 최종 대시속도 


                m_Context["Icon_DASH_COOLTIME"] = UserSaveData.BindingSO.Icon_DASH_COOLTIME;
                m_Context["CurrentCharacter/Base_DashCool"] = UserData.GetCurrentCharacterDB.Data.moveSpeed * 100;                             //캐릭터 기본 대시속도
                m_Context["CurrentCharacter/Base_DashCool_Inc_PerLV"] = UserData.GetCurrentCharacterDB.Data.incMoveSpeed * 100;            //캐릭터 기본 대시속도 레벨당 증가량 (%)
                m_Context["CurrentCharacter/DashCool"] = (UserData.GetCurrentCharacterDB.Data.moveSpeed
                                                        + UserData.GetCurrentCharacterDB.Data.incMoveSpeed * UserData.playerLevel) * 100;   //캐릭터 최종 대시속도 
                #endregion

                #region #3-3 스킬 데이터


                InitSkillData_Bind();

                #endregion
                //m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;
                //m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;
                //m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;
                //m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;
                //m_Context["CurrentCharacter/Skeleton"] = UserData.GetCurrentCharacterDB.skeleton;



                //m_Context["CurrentCharacter/SkinData"] = UserData.GetCurrentCharacterDB.current_Skin.SkinData;
                //m_Context["CurrentCharacter/SkinName"] = UserData.GetCurrentCharacterDB.current_Skin.Skin_Name;
                //m_Context["CurrentCharacter/SkinName"] = UserData.GetCurrentCharacterDB.current_Skin.Skin_Name;
            }
            #endregion


            #endregion

            #region #. 부가 아이콘
            m_Context["Icon_ManaCost"] = UserSaveData.BindingSO.Icon_ManaCost;
            m_Context["Icon_CoolTime"] = UserSaveData.BindingSO.Icon_CoolTime;
            m_Context["Icon_Silver"] = UserSaveData.BindingSO.Icon_Silver;
            m_Context["Icon_Gold"] = UserSaveData.BindingSO.Icon_Gold;
            #endregion
        }


        /// <summary>
        /// 캐릭터, 타워 등의 리소스 항목들을 불러와 초기화합니다.
        /// </summary>
        public void ContentInitialize()
        {
            #region #1. 캐릭터 콘텐츠 초기화
            {
                var character_ContentBoxes_Parent = FindObjectsOfType<UI_Character_ContentBox>(true)[0].transform.parent;
                var character_ContentBoxes = character_ContentBoxes_Parent.GetComponentsInChildren<UI_Character_ContentBox>(true);
                for (int i = 0; i < character_ContentBoxes.Length; i++)
                {
                    if (i < (int)YS.PLAYABLE_UNIT_INDEX.MAX)
                    {
                        if (userData.characters_Info.TryGetValue((YS.PLAYABLE_UNIT_INDEX)i, out UserSaveData.Character_DB value))
                        {
                            character_ContentBoxes[i].Initialize(i, value);
                        }
                        else
                        {
                            character_ContentBoxes[i].Initialize(i, new UserSaveData.Character_DB(false));
                        }
                    }
                    else
                    {
                        character_ContentBoxes[i].Initialize(i, null);
                    }
                }
            }
            #endregion

            #region #2. 타워 콘텐츠 초기화
            {
                var Tower_ContentBoxs_Parent = FindObjectOfType<UI_Tower_ContentBox>(true).transform.parent;
                var Tower_ContentBoxes = Tower_ContentBoxs_Parent.GetComponentsInChildren<UI_Tower_ContentBox>(true);
                for (int i = 0; i < Tower_ContentBoxes.Length; i++)
                {
                    if (i < (int)YS.TOWER_INDEX.MAX)
                    {
                        if (userData.towers_Info.TryGetValue((YS.TOWER_INDEX)i, out UserSaveData.Tower_DB vlaue))
                        {
                            Tower_ContentBoxes[i].Initialized(i, vlaue);
                        }
                        else
                        {
                            Tower_ContentBoxes[i].Initialized(i, null);
                        }
                    }
                    else
                    {
                        Tower_ContentBoxes[i].Initialized(i, null);
                    }
                }
            }
            #endregion
        }
    }
}
