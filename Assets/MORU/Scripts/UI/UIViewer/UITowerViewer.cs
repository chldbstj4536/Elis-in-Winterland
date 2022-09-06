using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru.UI
{
    public partial class UIViewer : MonoBehaviour
    {
        #region #타워 데이터 바인딩

        public void InitTowerData_Bind()
        {
            StartCoroutine(Tower_Bind());

        }
        IEnumerator Tower_Bind()
        {
            yield return null;
            m_Context["CurrentTowerPreset"] = userData.Current_Preset_Adress+1;     //프리셋 넘버링 업데이트
            var Data = userData.GetCurrentTowerDB.Data;
            var level = UserData.GetCurrentTowerDB.Level;

            m_Context["GetCurrentTowerDB/Name"] = Data.name;                                      //캐릭터 이름
            m_Context["GetCurrentTowerDB/Skeleton"] = UserData.GetCurrentTowerDB.skeleton;                                   //캐릭터 스켈레톤 데이터
            m_Context["GetCurrentTowerDB/Desc"] = Data.desc;                                   //캐릭터 스켈레톤 데이터
            m_Context["GetCurrentTowerDB/Level"] = level;                                   //캐릭터 스켈레톤 데이터

            string LevelupGold = UserData.GetCurrentTowerDB.Level < UserData.playerLevel ?
                $"{ UserData.GetCurrentTowerDB.GoldCost.ToString() }" : "<color=#A90000>불가능</color>";
            m_Context["GetCurrentTowerDB/GoldCost"] = LevelupGold;                                   //레벨업에 필요한 골드




            #region #타워 스텟 데이터 업데이트
            m_Context["GetCurrentTowerDB/BaseHP"] = Data.baseMaxHP;                                                         //캐릭터 기본체력
            m_Context["GetCurrentTowerDB/BaseHP_PerLV"] = Data.incHpPerLv;                                                  //캐릭터 레벨당 체력 증가량
            m_Context["GetCurrentTowerDB/HP"] = Data.baseMaxHP
                                            + Data.incHpPerLv * level;                           //캐릭터 최종 체력

            m_Context["GetCurrentTowerDB/HP+1"] = Data.baseMaxHP
                                            + Data.incHpPerLv * (level+1);                           //레벨 1업 시 체력증가량




            m_Context["GetCurrentTowerDB/BaseHP_Regen"] = Data.baseHPRegen;                       //캐릭터 기본체력재생량
            m_Context["GetCurrentTowerDB/BaseHP_Regen_PerLV"] = Data.incHPRegenPerLv;             //캐릭터 기본체력재생량 레벨당 증가량
            m_Context["GetCurrentTowerDB/HP_Regen"] = Data.baseHPRegen
                                                    + Data.incHPRegenPerLv * level; //캐릭터 최종 체력재생량

            m_Context["GetCurrentTowerDB/HP_Regen+1"] = Data.baseHPRegen
                                                    + Data.incHPRegenPerLv * (level+1);                 //레벨 1업 시 체젠증가량




            m_Context["GetCurrentTowerDB/BaseMP"] = Data.baseMaxMP;                               //캐릭터 기본마나
            m_Context["GetCurrentTowerDB/BaseMP_PerLV"] = Data.incMpPerLv;                        //캐릭터 레벨당 마나 증가량
            m_Context["GetCurrentTowerDB/MP"] = Data.baseMaxMP
                                            + Data.incMpPerLv * level;            //캐릭터 최종 마나

            m_Context["GetCurrentTowerDB/MP+1"] = Data.baseMaxMP
                                            + Data.incMpPerLv * (level+1);            //캐릭터 최종 마나




            m_Context["GetCurrentTowerDB/BaseMP_Regen"] = Data.baseMPRegen;                       //캐릭터 기본마나재생량
            m_Context["GetCurrentTowerDB/BaseHP_Regen_PerLV"] = Data.incMPRegenPerLv;             //캐릭터 기본마나재생량 레벨당 증가량
            m_Context["GetCurrentTowerDB/MP_Regen"] = Data.baseMPRegen
                                                    + Data.incMPRegenPerLv * level; //캐릭터 최종 마나재생량

            m_Context["GetCurrentTowerDB/MP_Regen+1"] = Data.baseMPRegen
                                        + Data.incMPRegenPerLv * (level+1); //캐릭터 최종 마나재생량



            m_Context["GetCurrentTowerDB/Base_Armor"] = Data.armor * 100;                       //캐릭터 기본 방어력
            m_Context["GetCurrentTowerDB/Base_ArmorInc_PerLV"] = Data.incArmorPerLv;             //캐릭터 기본마나재생량 레벨당 증가량
            m_Context["GetCurrentTowerDB/Armor"] = (Data.armor
                                                    + Data.incArmorPerLv * level) * 100; //캐릭터 최종 마나재생량

            m_Context["GetCurrentTowerDB/Armor+1"] = (Data.armor
                                        + Data.incArmorPerLv * (level+1)) * 100; //캐릭터 최종 마나재생량




            m_Context["GetCurrentTowerDB/Base_PhysicsPower"] = Data.physxPower;                       //캐릭터 기본 물리공격력
            m_Context["GetCurrentTowerDB/Base_PhysicsPowerInc_PerLV"] = Data.incPhysxPowerPerLv;      //캐릭터 기본 물리공격력 레벨당 증가량
            m_Context["GetCurrentTowerDB/PhysicsPower"] = Data.physxPower
                                                    + Data.incPhysxPowerPerLv * level; //캐릭터 최종 물리공격력

            m_Context["GetCurrentTowerDB/PhysicsPower+1"] = Data.physxPower
                                                    + Data.incPhysxPowerPerLv * (level+1); //캐릭터 최종 물리공격력




            m_Context["GetCurrentTowerDB/Base_ArmorPnt"] = Data.armorPnt * 100;                             //캐릭터 기본 방어관통력 (%)
            m_Context["GetCurrentTowerDB/Base_ArmorPntInc_PerLV"] = Data.incArmorPntPerLv * 100;            //캐릭터 기본 방어관통력 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/ArmorPnt"] = (Data.armorPnt
                                                    + Data.incArmorPntPerLv * level) * 100; //캐릭터 최종 방어관통력

            m_Context["GetCurrentTowerDB/ArmorPnt+1"] = (Data.armorPnt
                                                    + Data.incArmorPntPerLv * (level+1)) * 100; //캐릭터 최종 방어관통력




            m_Context["GetCurrentTowerDB/Base_CriticalRate"] = Data.criticalRate * 100;                             //캐릭터 기본 치명타율(%)
            m_Context["GetCurrentTowerDB/Base_CriticalRateInc_PerLV"] = Data.incCriticalRatePerLv * 100;            //캐릭터 기본 치명타율 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/CriticalRate"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * level) * 100; //캐릭터 최종 치명타율

            m_Context["GetCurrentTowerDB/CriticalRate+1"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * (level+1)) * 100; //캐릭터 최종 치명타율




            m_Context["GetCurrentTowerDB/Base_BloodSteel"] = Data.criticalRate * 100;                             //캐릭터 기본 흡혈율(%)
            m_Context["GetCurrentTowerDB/Base_BloodSteelInc_PerLV"] = Data.incCriticalRatePerLv * 100;            //캐릭터 기본 흡혈율 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/BloodSteel"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * level) * 100; //캐릭터 최종 흡혈율

            m_Context["GetCurrentTowerDB/BloodSteel+1"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * (level+1)) * 100; //캐릭터 최종 흡혈율




            if (UserData.GetCurrentTowerDB.Data.defaultAttackData != null)
            {
                // 기존 코드
                //m_Context["GetCurrentTowerDB/AttackRange"] = Data.defaultAttackData.SkillData.activeSkillData.range;  //캐릭터 기본공격 사거리
                // 수정 코드
                var attackData = Data.defaultAttackData.SkillData.activeSkillData[0];
                m_Context["GetCurrentTowerDB/AttackRange"] = attackData != null ? Data.defaultAttackData.SkillData.activeSkillData[0].range : 0;  //캐릭터 기본공격 사거리
            }
            else { m_Context["GetCurrentTowerDB/AttackRange"] = 0; }

            m_Context["GetCurrentTowerDB/Base_AttackSpeed"] = Data.atkSpd;                                //캐릭터 기본 공격속도
            m_Context["GetCurrentTowerDB/Base_AttackSpeedInc_PerLV"] = Data.incAtkSpdPerLv;               //캐릭터 기본 공격속도 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/AttackSpeed"] = (Data.atkSpd
                                                    + Data.incAtkSpdPerLv * level);      //캐릭터 최종 공격속도

            m_Context["GetCurrentTowerDB/AttackSpeed+1"] = (Data.atkSpd
                                                    + Data.incAtkSpdPerLv * (level+1));      //캐릭터 최종 공격속도




            m_Context["GetCurrentTowerDB/Base_MagicPower"] = Data.magicPower;                             //캐릭터 기본 주문력
            m_Context["GetCurrentTowerDB/Base_MagicPowerInc_PerLV"] = Data.incMagicPowerPerLv;            //캐릭터 기본 주문력 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/MagicPower"] = (Data.magicPower
                                                    + Data.incMagicPowerPerLv * level);   //캐릭터 최종 주문력

            m_Context["GetCurrentTowerDB/MagicPower+1"] = (Data.magicPower
                                                    + Data.incMagicPowerPerLv * (level+1));   //캐릭터 최종 주문력




            m_Context["GetCurrentTowerDB/Base_Cool_Reduce"] = Data.cooldownReduction * 100;                             //캐릭터 기본 쿨타임감소율(%)
            m_Context["GetCurrentTowerDB/Base_Cool_Reduce_Inc_PerLV"] = Data.incCdrPerLv * 100;            //캐릭터 기본 쿨타임감소율 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/Cool_Reduce"] = (Data.cooldownReduction
                                                    + Data.incCdrPerLv * level) * 100;   //캐릭터 최종 쿨타임감소율(%)

            m_Context["GetCurrentTowerDB/Cool_Reduce+1"] = (Data.cooldownReduction
                                                    + Data.incCdrPerLv * (level+1)) * 100;   //캐릭터 최종 쿨타임감소율(%)




            m_Context["GetCurrentTowerDB/Base_MoveSpeed"] = Data.moveSpeed;                             //캐릭터 기본 이동속도
            m_Context["GetCurrentTowerDB/Base_MoveSpeed_Inc_PerLV"] = Data.incMoveSpeed;            //캐릭터 기본 이동속도 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/MoveSpeed"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level);   //캐릭터 최종 이동속도

            m_Context["GetCurrentTowerDB/MoveSpeed+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1));   //캐릭터 최종 이동속도




            m_Context["GetCurrentTowerDB/Base_DashSpeed"] = Data.moveSpeed;                             //캐릭터 기본 대시속도
            m_Context["GetCurrentTowerDB/Base_DashSpeed_Inc_PerLV"] = Data.incMoveSpeed;            //캐릭터 기본 대시속도 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/DashSpeed"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level);   //캐릭터 최종 대시속도 

            m_Context["GetCurrentTowerDB/DashSpeed+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1));   //캐릭터 최종 대시속도 




            m_Context["GetCurrentTowerDB/Base_DashCool"] = Data.moveSpeed * 100;                             //캐릭터 기본 대시속도
            m_Context["GetCurrentTowerDB/Base_DashCool_Inc_PerLV"] = Data.incMoveSpeed * 100;            //캐릭터 기본 대시속도 레벨당 증가량 (%)
            m_Context["GetCurrentTowerDB/DashCool"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level) * 100;   //캐릭터 최종 대시속도

            m_Context["GetCurrentTowerDB/DashCool+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1)) * 100;   //캐릭터 최종 대시속도




            m_Context["Icon_TempVar"] = UserSaveData.BindingSO.Icon_Temperature;                                                //온도변화량 아이콘
            m_Context["GetCurrentTowerDB/TempVar"] = Data.tempVar;                             //캐릭터 기본 온도감소량



            m_Context["Icon_SpawnCost"] = UserSaveData.BindingSO.Icon_Silver;                                                //소환비용 아이콘
            m_Context["GetCurrentTowerDB/SpawnCost"] = Data.spawnCost;                                   //소환비용


            #endregion

            //타워 슬로팅 정보 업데이트
            for (int i = 0; i < 6; i++)
            {
                var Preset_TowerSlots = userData.Tower_Preset[userData.Current_Preset_Adress];
                if (Preset_TowerSlots[(Define.Slot)i] != null)
                {
                    m_Context[$"TowerSlot_{i}/Icon"] = Preset_TowerSlots[(Define.Slot)i].Data.icon;
                    m_Context[$"TowerSlot_{i}/Name"] = Preset_TowerSlots[(Define.Slot)i].Data.name;
                    m_Context[$"TowerSlot_{i}/Level"] = Preset_TowerSlots[(Define.Slot)i].Level;
                }
                else
                {
                    m_Context[$"TowerSlot_{i}/Icon"] = UserSaveData.BindingSO.Icon_Null;
                    m_Context[$"TowerSlot_{i}/Name"] = "비어있음";
                    m_Context[$"TowerSlot_{i}/Level"] = 0;
                }
            }
        }
        #endregion

    }
}
