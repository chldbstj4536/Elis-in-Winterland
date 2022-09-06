using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

namespace Moru.UI
{
    public partial class UIViewer : MonoBehaviour
    {
        #region #Ÿ�� ������ ���ε�

        public void InitTowerData_Bind()
        {
            StartCoroutine(Tower_Bind());

        }
        IEnumerator Tower_Bind()
        {
            yield return null;
            m_Context["CurrentTowerPreset"] = userData.Current_Preset_Adress+1;     //������ �ѹ��� ������Ʈ
            var Data = userData.GetCurrentTowerDB.Data;
            var level = UserData.GetCurrentTowerDB.Level;

            m_Context["GetCurrentTowerDB/Name"] = Data.name;                                      //ĳ���� �̸�
            m_Context["GetCurrentTowerDB/Skeleton"] = UserData.GetCurrentTowerDB.skeleton;                                   //ĳ���� ���̷��� ������
            m_Context["GetCurrentTowerDB/Desc"] = Data.desc;                                   //ĳ���� ���̷��� ������
            m_Context["GetCurrentTowerDB/Level"] = level;                                   //ĳ���� ���̷��� ������

            string LevelupGold = UserData.GetCurrentTowerDB.Level < UserData.playerLevel ?
                $"{ UserData.GetCurrentTowerDB.GoldCost.ToString() }" : "<color=#A90000>�Ұ���</color>";
            m_Context["GetCurrentTowerDB/GoldCost"] = LevelupGold;                                   //�������� �ʿ��� ���




            #region #Ÿ�� ���� ������ ������Ʈ
            m_Context["GetCurrentTowerDB/BaseHP"] = Data.baseMaxHP;                                                         //ĳ���� �⺻ü��
            m_Context["GetCurrentTowerDB/BaseHP_PerLV"] = Data.incHpPerLv;                                                  //ĳ���� ������ ü�� ������
            m_Context["GetCurrentTowerDB/HP"] = Data.baseMaxHP
                                            + Data.incHpPerLv * level;                           //ĳ���� ���� ü��

            m_Context["GetCurrentTowerDB/HP+1"] = Data.baseMaxHP
                                            + Data.incHpPerLv * (level+1);                           //���� 1�� �� ü��������




            m_Context["GetCurrentTowerDB/BaseHP_Regen"] = Data.baseHPRegen;                       //ĳ���� �⺻ü�������
            m_Context["GetCurrentTowerDB/BaseHP_Regen_PerLV"] = Data.incHPRegenPerLv;             //ĳ���� �⺻ü������� ������ ������
            m_Context["GetCurrentTowerDB/HP_Regen"] = Data.baseHPRegen
                                                    + Data.incHPRegenPerLv * level; //ĳ���� ���� ü�������

            m_Context["GetCurrentTowerDB/HP_Regen+1"] = Data.baseHPRegen
                                                    + Data.incHPRegenPerLv * (level+1);                 //���� 1�� �� ü��������




            m_Context["GetCurrentTowerDB/BaseMP"] = Data.baseMaxMP;                               //ĳ���� �⺻����
            m_Context["GetCurrentTowerDB/BaseMP_PerLV"] = Data.incMpPerLv;                        //ĳ���� ������ ���� ������
            m_Context["GetCurrentTowerDB/MP"] = Data.baseMaxMP
                                            + Data.incMpPerLv * level;            //ĳ���� ���� ����

            m_Context["GetCurrentTowerDB/MP+1"] = Data.baseMaxMP
                                            + Data.incMpPerLv * (level+1);            //ĳ���� ���� ����




            m_Context["GetCurrentTowerDB/BaseMP_Regen"] = Data.baseMPRegen;                       //ĳ���� �⺻���������
            m_Context["GetCurrentTowerDB/BaseHP_Regen_PerLV"] = Data.incMPRegenPerLv;             //ĳ���� �⺻��������� ������ ������
            m_Context["GetCurrentTowerDB/MP_Regen"] = Data.baseMPRegen
                                                    + Data.incMPRegenPerLv * level; //ĳ���� ���� ���������

            m_Context["GetCurrentTowerDB/MP_Regen+1"] = Data.baseMPRegen
                                        + Data.incMPRegenPerLv * (level+1); //ĳ���� ���� ���������



            m_Context["GetCurrentTowerDB/Base_Armor"] = Data.armor * 100;                       //ĳ���� �⺻ ����
            m_Context["GetCurrentTowerDB/Base_ArmorInc_PerLV"] = Data.incArmorPerLv;             //ĳ���� �⺻��������� ������ ������
            m_Context["GetCurrentTowerDB/Armor"] = (Data.armor
                                                    + Data.incArmorPerLv * level) * 100; //ĳ���� ���� ���������

            m_Context["GetCurrentTowerDB/Armor+1"] = (Data.armor
                                        + Data.incArmorPerLv * (level+1)) * 100; //ĳ���� ���� ���������




            m_Context["GetCurrentTowerDB/Base_PhysicsPower"] = Data.physxPower;                       //ĳ���� �⺻ �������ݷ�
            m_Context["GetCurrentTowerDB/Base_PhysicsPowerInc_PerLV"] = Data.incPhysxPowerPerLv;      //ĳ���� �⺻ �������ݷ� ������ ������
            m_Context["GetCurrentTowerDB/PhysicsPower"] = Data.physxPower
                                                    + Data.incPhysxPowerPerLv * level; //ĳ���� ���� �������ݷ�

            m_Context["GetCurrentTowerDB/PhysicsPower+1"] = Data.physxPower
                                                    + Data.incPhysxPowerPerLv * (level+1); //ĳ���� ���� �������ݷ�




            m_Context["GetCurrentTowerDB/Base_ArmorPnt"] = Data.armorPnt * 100;                             //ĳ���� �⺻ ������� (%)
            m_Context["GetCurrentTowerDB/Base_ArmorPntInc_PerLV"] = Data.incArmorPntPerLv * 100;            //ĳ���� �⺻ ������� ������ ������ (%)
            m_Context["GetCurrentTowerDB/ArmorPnt"] = (Data.armorPnt
                                                    + Data.incArmorPntPerLv * level) * 100; //ĳ���� ���� �������

            m_Context["GetCurrentTowerDB/ArmorPnt+1"] = (Data.armorPnt
                                                    + Data.incArmorPntPerLv * (level+1)) * 100; //ĳ���� ���� �������




            m_Context["GetCurrentTowerDB/Base_CriticalRate"] = Data.criticalRate * 100;                             //ĳ���� �⺻ ġ��Ÿ��(%)
            m_Context["GetCurrentTowerDB/Base_CriticalRateInc_PerLV"] = Data.incCriticalRatePerLv * 100;            //ĳ���� �⺻ ġ��Ÿ�� ������ ������ (%)
            m_Context["GetCurrentTowerDB/CriticalRate"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * level) * 100; //ĳ���� ���� ġ��Ÿ��

            m_Context["GetCurrentTowerDB/CriticalRate+1"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * (level+1)) * 100; //ĳ���� ���� ġ��Ÿ��




            m_Context["GetCurrentTowerDB/Base_BloodSteel"] = Data.criticalRate * 100;                             //ĳ���� �⺻ ������(%)
            m_Context["GetCurrentTowerDB/Base_BloodSteelInc_PerLV"] = Data.incCriticalRatePerLv * 100;            //ĳ���� �⺻ ������ ������ ������ (%)
            m_Context["GetCurrentTowerDB/BloodSteel"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * level) * 100; //ĳ���� ���� ������

            m_Context["GetCurrentTowerDB/BloodSteel+1"] = (Data.criticalRate
                                                    + Data.incCriticalRatePerLv * (level+1)) * 100; //ĳ���� ���� ������




            if (UserData.GetCurrentTowerDB.Data.defaultAttackData != null)
            {
                // ���� �ڵ�
                //m_Context["GetCurrentTowerDB/AttackRange"] = Data.defaultAttackData.SkillData.activeSkillData.range;  //ĳ���� �⺻���� ��Ÿ�
                // ���� �ڵ�
                var attackData = Data.defaultAttackData.SkillData.activeSkillData[0];
                m_Context["GetCurrentTowerDB/AttackRange"] = attackData != null ? Data.defaultAttackData.SkillData.activeSkillData[0].range : 0;  //ĳ���� �⺻���� ��Ÿ�
            }
            else { m_Context["GetCurrentTowerDB/AttackRange"] = 0; }

            m_Context["GetCurrentTowerDB/Base_AttackSpeed"] = Data.atkSpd;                                //ĳ���� �⺻ ���ݼӵ�
            m_Context["GetCurrentTowerDB/Base_AttackSpeedInc_PerLV"] = Data.incAtkSpdPerLv;               //ĳ���� �⺻ ���ݼӵ� ������ ������ (%)
            m_Context["GetCurrentTowerDB/AttackSpeed"] = (Data.atkSpd
                                                    + Data.incAtkSpdPerLv * level);      //ĳ���� ���� ���ݼӵ�

            m_Context["GetCurrentTowerDB/AttackSpeed+1"] = (Data.atkSpd
                                                    + Data.incAtkSpdPerLv * (level+1));      //ĳ���� ���� ���ݼӵ�




            m_Context["GetCurrentTowerDB/Base_MagicPower"] = Data.magicPower;                             //ĳ���� �⺻ �ֹ���
            m_Context["GetCurrentTowerDB/Base_MagicPowerInc_PerLV"] = Data.incMagicPowerPerLv;            //ĳ���� �⺻ �ֹ��� ������ ������ (%)
            m_Context["GetCurrentTowerDB/MagicPower"] = (Data.magicPower
                                                    + Data.incMagicPowerPerLv * level);   //ĳ���� ���� �ֹ���

            m_Context["GetCurrentTowerDB/MagicPower+1"] = (Data.magicPower
                                                    + Data.incMagicPowerPerLv * (level+1));   //ĳ���� ���� �ֹ���




            m_Context["GetCurrentTowerDB/Base_Cool_Reduce"] = Data.cooldownReduction * 100;                             //ĳ���� �⺻ ��Ÿ�Ӱ�����(%)
            m_Context["GetCurrentTowerDB/Base_Cool_Reduce_Inc_PerLV"] = Data.incCdrPerLv * 100;            //ĳ���� �⺻ ��Ÿ�Ӱ����� ������ ������ (%)
            m_Context["GetCurrentTowerDB/Cool_Reduce"] = (Data.cooldownReduction
                                                    + Data.incCdrPerLv * level) * 100;   //ĳ���� ���� ��Ÿ�Ӱ�����(%)

            m_Context["GetCurrentTowerDB/Cool_Reduce+1"] = (Data.cooldownReduction
                                                    + Data.incCdrPerLv * (level+1)) * 100;   //ĳ���� ���� ��Ÿ�Ӱ�����(%)




            m_Context["GetCurrentTowerDB/Base_MoveSpeed"] = Data.moveSpeed;                             //ĳ���� �⺻ �̵��ӵ�
            m_Context["GetCurrentTowerDB/Base_MoveSpeed_Inc_PerLV"] = Data.incMoveSpeed;            //ĳ���� �⺻ �̵��ӵ� ������ ������ (%)
            m_Context["GetCurrentTowerDB/MoveSpeed"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level);   //ĳ���� ���� �̵��ӵ�

            m_Context["GetCurrentTowerDB/MoveSpeed+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1));   //ĳ���� ���� �̵��ӵ�




            m_Context["GetCurrentTowerDB/Base_DashSpeed"] = Data.moveSpeed;                             //ĳ���� �⺻ ��üӵ�
            m_Context["GetCurrentTowerDB/Base_DashSpeed_Inc_PerLV"] = Data.incMoveSpeed;            //ĳ���� �⺻ ��üӵ� ������ ������ (%)
            m_Context["GetCurrentTowerDB/DashSpeed"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level);   //ĳ���� ���� ��üӵ� 

            m_Context["GetCurrentTowerDB/DashSpeed+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1));   //ĳ���� ���� ��üӵ� 




            m_Context["GetCurrentTowerDB/Base_DashCool"] = Data.moveSpeed * 100;                             //ĳ���� �⺻ ��üӵ�
            m_Context["GetCurrentTowerDB/Base_DashCool_Inc_PerLV"] = Data.incMoveSpeed * 100;            //ĳ���� �⺻ ��üӵ� ������ ������ (%)
            m_Context["GetCurrentTowerDB/DashCool"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * level) * 100;   //ĳ���� ���� ��üӵ�

            m_Context["GetCurrentTowerDB/DashCool+1"] = (Data.moveSpeed
                                                    + Data.incMoveSpeed * (level+1)) * 100;   //ĳ���� ���� ��üӵ�




            m_Context["Icon_TempVar"] = UserSaveData.BindingSO.Icon_Temperature;                                                //�µ���ȭ�� ������
            m_Context["GetCurrentTowerDB/TempVar"] = Data.tempVar;                             //ĳ���� �⺻ �µ����ҷ�



            m_Context["Icon_SpawnCost"] = UserSaveData.BindingSO.Icon_Silver;                                                //��ȯ��� ������
            m_Context["GetCurrentTowerDB/SpawnCost"] = Data.spawnCost;                                   //��ȯ���


            #endregion

            //Ÿ�� ������ ���� ������Ʈ
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
                    m_Context[$"TowerSlot_{i}/Name"] = "�������";
                    m_Context[$"TowerSlot_{i}/Level"] = 0;
                }
            }
        }
        #endregion

    }
}
