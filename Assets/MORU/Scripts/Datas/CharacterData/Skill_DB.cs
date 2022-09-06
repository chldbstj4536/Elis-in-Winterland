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



            [SerializeField, ShowInInspector, LabelText("캐릭터의 스킬들")] Dictionary<int, Skill_DB> character_Skills = new Dictionary<int, Skill_DB>();
            /// <summary>
            /// 캐릭터가 가지고 있는 스킬들입니다.
            /// </summary>
            public Dictionary<int, Skill_DB> Character_Skills => character_Skills;


            [SerializeField, ShowInInspector, LabelText("현재 선택한 캐릭터의 스킬")] Skill_DB current_Selected_Skill_DB;
            /// <summary>
            /// 플레이어가 선택하여 상세정보에 표시될 현재 선택된 스킬의 정보입니다.
            /// </summary>
            public Skill_DB Current_Selected_Skill_DB
            {
                get => current_Selected_Skill_DB; set => current_Selected_Skill_DB = value;
            }

            /// <summary>
            /// 플레이어가 최대로 가질 수 있는 스킬 프리셋의 개수입니다.
            /// </summary>
            private const int MaxPresetCount = 3;    //0~2
            /// <summary>
            /// 현재 프리셋 넘버입니다.
            /// </summary>
            private int current_preset_Adress = 0;
            public int Current_preset_Adress { get => current_preset_Adress; set => current_preset_Adress = value; }

            /// <summary>
            /// 해당 프리셋에 해당하는 스킬 슬롯, 스킬의 슬롯이 달라집니다.
            /// </summary>
            [SerializeField, ShowInInspector, LabelText("현재 타겟 스킬슬롯")] Dictionary<Define.Slot, Skill_DB> skill_Slots;

            /// <summary>
            /// 스킬리스트의 프리셋 리스트, 스킬의 레벨이나 스킬포인트 등이 달라집니다.
            /// </summary>
            [SerializeField, ShowInInspector, LabelText("스킬 프리셋")]
            Dictionary<int, Dictionary<Define.Slot, Skill_DB>> skill_Preset = new Dictionary<int, Dictionary<Define.Slot, Skill_DB>>();
            public Dictionary<int, Dictionary<Define.Slot, Skill_DB>> Skill_Preset => skill_Preset;



            /// <summary>
            /// 프리셋당 잔여 스킬포인트입니다.
            /// </summary>
            [SerializeField] public List<int> current_Skill_Point = new List<int>();

            /// <summary>
            /// 초기화 시 해당값으로 스킬 포인트가 돌아갑니다.
            /// </summary>
            [SerializeField] private int max_Skill_Point;
            [SerializeField] public int Max_Skill_Point { get => max_Skill_Point; set => max_Skill_Point = UserInfo.instance.Userinfo.PlayerLevel; }
            /// <summary>
            /// 0레벨 (1레벨) 시 최초 스킬포인트 보유량입니다.
            /// </summary>
            [SerializeField] private int init_Skill_Point = 0;


            #endregion

            [System.Serializable]
            public class Skill_DB
            {
                [SerializeField] private List<int> preset_level = new List<int>();
                public List<int> Preset_Level => preset_level;
                [SerializeField] private int base_Cost = 1;
                [SerializeField] private int Inc_Velocity = 1; //더해짐
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
                /// 스킬의 실제 데이터입니다.
                /// </summary>
                public SkillSO Data;

                /// <summary>
                /// 스킬 데이터 생성자, 데이터 할당 및 스킬레벨 초기화
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
            /// 해당 캐릭터의 스킬관련 데이터를 초기화합니다.
            /// </summary>
            private void INIT_Skill()
            {
                var Skill_Recourses = Resources.LoadAll<SkillSO>(("Datas/Skills/Character/" + Index.ToString()));
                List<Skill_DB> _skillList = new List<Skill_DB>();
                //배열 리스트화 및 기본공격 필터링
                foreach (var skill in Skill_Recourses)
                {
                    // 기존 코드
                    //if (skill.SkillData.activeSkillData.isDefaultAttack)
                    // 수정 코드
                    if (skill.SkillData.activeSkillData[0].isDefaultAttack)
                    { continue; }
                    if( skill.SkillData.icon == null)
                    { continue; }
                    var _skill_DB = new Skill_DB(skill);
                    _skillList.Add(_skill_DB);
                }

                string DebugString = "캐릭터 스킬 슬로팅결과 \n";
                //캐릭터의 스킬 9개까지 (0~8) 기본데이터 할당
                for (int i = 0; i < 9; i++)
                {
                    if (_skillList.Count > i)
                    {
                        character_Skills.Add(i, _skillList[i]);
                        DebugString +=
                        $"타겟 캐릭터 : {Index} // 스킬인덱스 및 스킬이름 : {i}-{_skillList[i].Data.SkillData.skillName}\n";
                    }
                    else
                    {
                        character_Skills.Add(i, null);
                        DebugString +=
                        $"타겟 캐릭터 : {Index} // 해당 스킬인덱스의 값이 비어있습니다. : {i}\n";
                    }
                }
                //스킬 디버그 결과
                Debug.LogWarning(DebugString);

                //스킬 프리셋과 스킬포인트 생성 및 스킬 할당
                for (int i = 0; i < MaxPresetCount; i++)
                {
                    var preset_value = new Dictionary<Define.Slot, Skill_DB>();
                    skill_Preset.Add(i, preset_value);
                    current_Skill_Point.Add(init_Skill_Point);
                    //해당 프리셋에 대해 스킬 기본값 할당
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
            /// 스킬을 할당할 경우 업데이트합니다.
            /// </summary>
            /// <param name="slot_Index"></param>
            /// <param name="skill_DB"></param>
            public void UpdateSkill(Define.Slot slot_Index, Skill_DB skill_DB)
            {
                var Prest_SkillSlot = skill_Preset[current_preset_Adress];

                //중복처리
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

                //중복스킬이나 스킬자리바꾸기에 대한 처리
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

                if (curLv + updateLv < 0) { return; }       //최소레벨 도달
                else if (curLv + updateLv > curSkill.Data.SkillData.maxLv) { return; }   //최대레벨 도달

                if (updateLv > 0) //증가 시 스킬포인트를 소모
                {
                    cost = curSkill.Inc_Point();
                    Debug.Log($"스킬 레벨증가 {cost}");
                    if (curPoint > cost)
                    {
                        characterDB.Current_Selected_Skill_DB.Preset_Level[curPresetNum] += updateLv;
                        characterDB.current_Skill_Point[adress] -= cost;
                        characterDB.SkillLv_Action.Push(new StackSkill(curPresetNum, cost, updateLv, curSkill));
                    }
                    else
                    {
                        //스킬포인트 모자람
                    }
                }

                else if(updateLv < 0) //감소 시 스킬포인트 환급
                {
                    cost = curSkill.Dec_Point();
                    Debug.Log($"스킬 레벨 감소 {cost}");
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