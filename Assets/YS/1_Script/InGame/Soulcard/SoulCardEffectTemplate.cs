//using System.Collections.Generic;
//using UnityEngine;
//using Sirenix.OdinInspector;

//namespace YS
//{
//    public abstract partial class Unit
//    {
//        public partial class Skill
//        {
//            private class SNAME : SoulCardEffect
//            {
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� SNAMEData�� ���ƾ� �Ѵ� (�׷��� Data�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                //private List<type> field1;
//                // ...

//                public SNAME(SNAMEData data, PlayableUnit skillOwner) : base(data, skillOwner)
//                {
//                    //field1 = data.field1;
//                }
//                protected override void FindUnitsInEffectRange() { }
//                protected override void InstantEffect() { }
//                protected override void OnBeginPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnEndPassiveEffect(Unit unit, int areaStack) { }
//                protected override void OnTickPassiveEffect(Unit unit, int tickStack) { }
//                protected override void OnEmptyInEffect() { }
//                protected override int GetTotalDamage(Unit victim)
//                {
//                    throw new System.NotImplementedException();
//                }
//            }
//            [System.Serializable]
//            private class SNAMEData : SoulCardEffectData
//            {
//                // �ش� ��ų�� �ʿ��� ������ ���� (����Ʈ, ���ط� ��)
//                // �� SNAMEDATA�� ���ƾ� �Ѵ� (�׷��� DATA�� ����� ������ �ʱ�ȭ �� �� �ִ�)
//                //[BoxGroup("NAME", true, true)]
//                //[LabelText("FIELD_NAME"), ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true, OnBeginListElementGUI = nameof(BeginDrawListElement), OnEndListElementGUI = nameof(EndDrawListElement))]
//                //public List<type> field1 = new List<type>();

//                public override BaseSkill Instantiate(Unit owner)
//                {
//                    return new SNAME(this, owner as PlayableUnit);
//                }

//                private void BeginDrawListElement(int index)
//                {
//#if UNITY_EDITOR
//                    Sirenix.Utilities.Editor.SirenixEditorGUI.BeginHorizontalPropertyLayout(new GUIContent($"Lv{index + 1}"), out Rect labelRect);
//#endif
//                }
//                private void EndDrawListElement(int index)
//                {
//#if UNITY_EDITOR
//                    Sirenix.Utilities.Editor.SirenixEditorGUI.EndHorizontalPropertyLayout();
//#endif
//                }
//                protected override void OnChangedMaxStack()
//                {
//                    if (field1.Count > maxStack)
//                    {
//                        int removeRange = field1.Count - maxStack;
//                        field1.RemoveRange(maxStack, removeRange);
//                        field2.RemoveRange(maxStack, removeRange);
//                    }
//                    else if (field1.Count < maxStack)
//                    {
//                        for (int i = field1.Count; i < maxStack; ++i)
//                        {
//                            field1.Add(type);
//                            field2.Add(type);
//                        }
//                    }
//                }
//            }
//        }
//    }
//}