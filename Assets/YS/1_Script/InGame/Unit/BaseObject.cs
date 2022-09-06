using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TypeReferences;

namespace YS
{
    public abstract class BaseObject : PoolingComponent
    {
        #region Field
        [FoldoutGroup("���̽� ������Ʈ")]
        [SerializeField, LabelText("��Ʈ�ڽ�"), Tooltip("������Ʈ�� ��Ʈ�ڽ�")]
        protected Collider[] hitboxCols;
        private Transform rendererTr;
        #endregion

        #region Properties
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract Sprite Icon { get; }
        public abstract bool IsLookingLeft { get; }
        public Transform RendererTransform => rendererTr;
        public GameObject Renderer => rendererTr.gameObject;
        #endregion

        #region Methods
        public override void OnInstantiate()
        {
            base.OnInstantiate();

            rendererTr = transform.GetChild(0);
        }
        public abstract void Flip(bool isLeft);
        #endregion

        [System.Serializable]
        public class BaseObjectData
        {
            [FoldoutGroup("���̽� ������Ʈ", false, 0)]
            [LabelText("������"), Tooltip("������ ������ ������")]
            public BaseObject prefab;
            [FoldoutGroup("���̽� ������Ʈ")]
            [LabelText("�̸�")]
            public string name;
            [FoldoutGroup("���̽� ������Ʈ"), TextArea]
            [LabelText("����")]
            public string desc;
            [FoldoutGroup("���̽� ������Ʈ")]
            [LabelText("������")]
            public Sprite icon;
        }
    }
}