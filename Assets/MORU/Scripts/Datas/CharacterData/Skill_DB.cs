using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YS;
using Moru.UI;
using Spine.Unity;
using Sirenix.OdinInspector;

namespace Moru
{
    public partial class UserSaveData
    {




        public partial class Character_DB
        {
            #region Varis

            public Stack<StackSkill> SkillLv_Action = new Stack<StackSkill>();  



            [SerializeField, ShowInInspector, LabelText("ĳ������ ��ų��")] Dictionary<int, Skill_DB> character_Skills = new Dictionary<int, Skill_DB>();
            /// <summary>
            /// ĳ���Ͱ� ������ �ִ� ��ų���Դϴ�.
            /// </summary>
            public Dictionary<int, Skill_DB> Character_Skills => character_Skills;


            [SerializeField, ShowInInspector, LabelText("���� ������ ĳ������ ��ų")] Skill_DB current_Selected_Skill_DB;
            /// <summary>
            /// �÷��̾ �����Ͽ� �������� ǥ�õ� ���� ���õ� ��ų�� �����Դϴ�.
            /// </summary>
            public Skill_DB Current_Selected_Skill_DB
            {
                get => current_Selected_Skill_DB; set => current_Selected_Skill_DB = value;
            }

            /// <summary>
            /// �÷��̾ �ִ�� ���� �� �ִ� ��ų �������� �����Դϴ�.
            /// </summary>
            private const int MaxPresetCount = 3;    //0~2
            /// <summary>
            /// ���� ������ �ѹ��Դϴ�.
            /// </summary>
            private int current_preset_Adress = 0;
            public int Current_preset_Adress { get => current_preset_Adress; set => current_preset_Adress = value; }

            /// <summary>
            /// �ش� �����¿� �ش��ϴ� ��ų ����, ��ų�� ������ �޶����ϴ�.
            /// </summary>
            [SerializeField, ShowInInspector, LabelText("���� Ÿ�� ��ų����")] Dictionary<Define.Slot, Skill_DB> skill_Slots;

            /// <summary>
            /// ��ų����Ʈ�� ������ ����Ʈ, ��ų�� �����̳� ��ų����Ʈ ���� �޶����ϴ�.
            /// </summary>
            [SerializeField, ShowInInspector, LabelText("��ų ������")]
            Dictionary<int, Dictionary<Define.Slot, Skill_DB>> skill_Preset = new Dictionary<int, Dictionary<Define.Slot, Skill_DB>>();
            public Dictionary<int, Dictionary<Define.Slot, Skill_DB>> Skill_Preset => skill_Preset;



            /// <summary>
            /// �����´� �ܿ� ��ų����Ʈ�Դϴ�.
            /// </summary>
            [SerializeField] public List<int> current_Skill_Point = new List<int>();

            /// <summary>
            /// �ʱ�ȭ �� �ش簪���� ��ų ����Ʈ�� ���ư��ϴ�.
            /// </summary>
            [SerializeField] private int max_Skill_Point;
            [SerializeField] public int Max_Skill_Point { get => max_Skill_Point; set => max_Skill_Point = UserInfo.instance.Userinfo.PlayerLevel; }
            /// <summary>
            /// 0���� (1����) �� ���� ��ų����Ʈ �������Դϴ�.
            /// </summary>
            [SerializeField] private int init_Skill_Point = 0;


            #endregion

            [System.Serializable]
            public class Skill_DB
            {
                [SerializeField] private List<int> preset_level = new List<int>();
                public List<int> Preset_Level => preset_level;
                [SerializeField] private int base_Cost = 1;
                [SerializeField] private int Inc_Velocity = 1; //������
                public int Inc_Point()
                {
                    int value = 
                    base_Cost + Inc_Velocity * preset_level[UserInfo.instance.Userinfo.GetCurrentCharacterDB.current_preset_Adress]+1;
                    return value;
                }
                public int Dec_Point()
                {
                    int value =
                    base_Cost + Inc_Velocity * preset_level[UserInfo.instance.Userinfo.GetCurrentCharacterDB.current_preset_Adress];
                    return value;
                }
                    

                /// <summary>
                /// ��ų�� ���� �������Դϴ�.
                /// </summary>
                public SkillSO Data;

                /// <summary>
                /// ��ų ������ ������, ������ �Ҵ� �� ��ų���� �ʱ�ȭ
                /// </summary>
                /// <param name="_data"></param>
                public Skill_DB(SkillSO _data)
                {
                    Data = _data;
                    for (int i = 0; i < 3; i++)
                    {
                        preset_level.Add(0);
                    }
                }
            }

            #region Methods

            /// <summary>
            /// �ش� ĳ������ ��ų���� �����͸� �ʱ�ȭ�մϴ�.
            /// </summary>
            private void INIT_Skill()
            {
                var Skill_Recourses = Resources.LoadAll<SkillSO>(("Datas/Skills/Character/" + Index.ToString()));
                List<Skill_DB> _skillList = new List<Skill_DB>();
                //�迭 ����Ʈȭ �� �⺻���� ���͸�
                foreach (var skill in Skill_Recourses)
                {
                    // ���� �ڵ�
                    //if (skill.SkillData.activeSkillData.isDefaultAttack)
                    // ���� �ڵ�
                    if (skill.SkillData.activeSkillData[0].isDefaultAttack)
                    { continue; }
                    if( skill.SkillData.icon == null)
                    { continue; }
                    var _skill_DB = new Skill_DB(skill);
                    _skillList.Add(_skill_DB);
                }

                string DebugString = "ĳ���� ��ų �����ð�� \n";
                //ĳ������ ��ų 9������ (0~8) �⺻������ �Ҵ�
                for (int i = 0; i < 9; i++)
                {
                    if (_skillList.Count > i)
                    {
                        character_Skills.Add(i, _skillList[i]);
                        DebugString +=
                        $"Ÿ�� ĳ���� : {Index} // ��ų�ε��� �� ��ų�̸� : {i}-{_skillList[i].Data.SkillData.skillName}\n";
                    }
                    else
                    {
                        character_Skills.Add(i, null);
                        DebugString +=
                        $"Ÿ�� ĳ���� : {Index} // �ش� ��ų�ε����� ���� ����ֽ��ϴ�. : {i}\n";
                    }
                }
                //��ų ����� ���
                Debug.LogWarning(DebugString);

                //��ų �����°� ��ų����Ʈ ���� �� ��ų �Ҵ�
                for (int i = 0; i < MaxPresetCount; i++)
                {
                    var preset_value = new Dictionary<Define.Slot, Skill_DB>();
                    skill_Preset.Add(i, preset_value);
                    current_Skill_Point.Add(init_Skill_Point);
                    //�ش� �����¿� ���� ��ų �⺻�� �Ҵ�
                    for (int j = 0; j < (int)Define.Slot.MAX; j++)
                    {
                        if (character_Skills.TryGetValue(j, out Skill_DB value))
                        {
                            preset_value.Add((Define.Slot)j, value);
                        }
                        else
                        {
                            preset_value.Add((Define.Slot)j, null);
                        }
                    }
                }
            }

            /// <summary>
            /// ��ų�� �Ҵ��� ��� ������Ʈ�մϴ�.
            /// </summary>
            /// <param name="slot_Index"></param>
            /// <param name="skill_DB"></param>
            public void UpdateSkill(Define.Slot slot_Index, Skill_DB skill_DB)
            {
                var Prest_SkillSlot = skill_Preset[current_preset_Adress];

                //�ߺ�ó��
                if (Prest_SkillSlot.ContainsValue(skill_DB))
                {
                    var PastSlotValue = Prest_SkillSlot[slot_Index];
                    Define.Slot PastValue_Slot = Define.Slot.A;
                    for (int i = 0; i < (int)Define.Slot.MAX; i++)
                    {
                        if (Prest_SkillSlot[(Define.Slot)i] == skill_DB)
                        {
                            PastValue_Slot = (Define.Slot)i;
                            break;
                        }
                    }
                    Prest_SkillSlot[PastValue_Slot] = PastSlotValue;
                }

                Prest_SkillSlot[slot_Index] = skill_DB;
                TitleUI_Events.Del_CharacterSKill_Click?.Invoke();

                //�ߺ���ų�̳� ��ų�ڸ��ٲٱ⿡ ���� ó��
            }

            public static void SkillLevelup(int updateLv)
            {
                var characterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
                var adress = characterDB.Current_preset_Adress;
                var curPoint = characterDB.current_Skill_Point[adress];
                var curSkill = characterDB.Current_Selected_Skill_DB;
                var curPresetNum = characterDB.Current_preset_Adress;
                var curLv = curSkill.Preset_Level[curPresetNum];
                int cost = 0;

                if (curLv + updateLv < 0) { return; }       //�ּҷ��� ����
                else if (curLv + updateLv > curSkill.Data.SkillData.maxLv) { return; }   //�ִ뷹�� ����

                if (updateLv > 0) //���� �� ��ų����Ʈ�� �Ҹ�
                {
                    cost = curSkill.Inc_Point();
                    Debug.Log($"��ų �������� {cost}");
                    if (curPoint > cost)
                    {
                        characterDB.Current_Selected_Skill_DB.Preset_Level[curPresetNum] += updateLv;
                        characterDB.current_Skill_Point[adress] -= cost;
                        characterDB.SkillLv_Action.Push(new StackSkill(curPresetNum, cost, updateLv, curSkill));
                    }
                    else
                    {
                        //��ų����Ʈ ���ڶ�
                    }
                }

                else if(updateLv < 0) //���� �� ��ų����Ʈ ȯ��
                {
                    cost = curSkill.Dec_Point();
                    Debug.Log($"��ų ���� ���� {cost}");
                    characterDB.Current_Selected_Skill_DB.Preset_Level[curPresetNum] += updateLv;
                    characterDB.current_Skill_Point[adress] += cost;
                    characterDB.SkillLv_Action.Push(new StackSkill(curPresetNum, cost, updateLv, curSkill));
                }
            }
            #endregion
        }

        public struct StackSkill
        {
            int adress;
            int skillpoint;
            int level;
            Moru.UserSaveData.Character_DB.Skill_DB skillDB;

            public StackSkill(int _preset_Adress, int _skillPoint, int _level, Moru.UserSaveData.Character_DB.Skill_DB _DB)
            {
                adress = _preset_Adress;
                skillpoint = _skillPoint;
                level = _level;
                skillDB = _DB;

            }

            public void Pop()
            {
                skillDB.Preset_Level[adress] -= level;
                var characterDB = UserInfo.instance.Userinfo.GetCurrentCharacterDB;
                characterDB.current_Skill_Point[adress] += skillpoint;
            }
        }
    }
}