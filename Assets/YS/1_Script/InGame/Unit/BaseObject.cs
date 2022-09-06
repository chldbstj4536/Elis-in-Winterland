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
        [FoldoutGroup("베이스 오브젝트")]
        [SerializeField, LabelText("히트박스"), Tooltip("오브젝트의 히트박스")]
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
            [FoldoutGroup("베이스 오브젝트", false, 0)]
            [LabelText("프리팹"), Tooltip("생성할 유닛의 프리팹")]
            public BaseObject prefab;
            [FoldoutGroup("베이스 오브젝트")]
            [LabelText("이름")]
            public string name;
            [FoldoutGroup("베이스 오브젝트"), TextArea]
            [LabelText("설명")]
            public string desc;
            [FoldoutGroup("베이스 오브젝트")]
            [LabelText("아이콘")]
            public Sprite icon;
        }
    }
}