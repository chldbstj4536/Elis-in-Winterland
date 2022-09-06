using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract class PrefabGenerator : PoolingComponent
    {
        #region Field
        [LabelText("��� �ð�")]
        public float duration;
        [LabelText("���� ��ġ ������")]
        public Vector3 offset;
        [LabelText("���� ���� ȸ����")]
        public Vector3 rot;

        [LabelText("��� ���������ð�")]
        public float emitStartDelay;
        [LabelText("��� �ð�")]
        public float emitDuration;
        [LabelText("�� ���� ������ ���� �ֱ� ����"), Tooltip("true : �� ���� ������ ���� �ֱ� ����, false : ���� �ֱ�� �� ���� ���� ����")]
        public bool bUseTotalCount;
        [ShowIf("bUseTotalCount"), LabelText("    �� ���� ����")]
        public uint totalCount;
        [HideIf("bUseTotalCount"), LabelText("    �ʴ� ���� ��")]
        public uint countPerSec;

        public PoolingComponent prefab;

        public delegate void EmitGameObjectEvent(GameObject obj);
        public event EmitGameObjectEvent OnEmitGameObjectEvent;
        private event Delegate_NoRetVal_NoParam EmissionEndEvent;
        public event Delegate_NoRetVal_NoParam OnLifeTimeEndEvent;
        #endregion

        #region Properties
        public event Delegate_NoRetVal_NoParam OnEmissionEndEvent
        {
            add { EmissionEndEvent = value; }
            remove { EmissionEndEvent -= value; }
        }
        #endregion

        #region Methods
        public void Play()
        {
            Invoke(nameof(OnLifeTimeEnd), duration);
            StartCoroutine(CoUpdate());
        }
        private IEnumerator CoUpdate()
        {
            if ((bUseTotalCount && totalCount == 0) || (!bUseTotalCount && countPerSec == 0))
                yield return CachedWaitForSeconds.Get(emitDuration);

            else
            {
                float curEmissionTime = emitDuration;
                float updateTime = bUseTotalCount ? emitDuration / totalCount : 1.0f / countPerSec;

                if (emitStartDelay > 0.0f)
                    yield return CachedWaitForSeconds.Get(emitStartDelay);

                WaitForSeconds wfUpdateTime = CachedWaitForSeconds.Get(updateTime);

                while (curEmissionTime > 0.00001f)
                {
                    // ��ü ����
                    var obj = PrefabPool.GetObject(prefab);
                    obj.transform.position = GetEmitPosition();
                    OnEmitGameObject(obj);

                    yield return wfUpdateTime;

                    curEmissionTime -= updateTime;
                }
            }

            OnEmissionEnd();
        }
        /// <summary>
        /// ���ӿ�����Ʈ ���� �� ȣ��
        /// </summary>
        /// <param name="obj">������ ���ӿ�����Ʈ</param>
        protected virtual void OnEmitGameObject(GameObject obj)
        {
            OnEmitGameObjectEvent?.Invoke(obj);
        }
        /// <summary>
        /// ��簡 ������ ȣ��
        /// </summary>
        protected virtual void OnEmissionEnd()
        {
            EmissionEndEvent?.Invoke();
        }
        /// <summary>
        /// LifeTime �ð��� ������ ȣ��
        /// </summary>
        protected virtual void OnLifeTimeEnd()
        {
            OnLifeTimeEndEvent?.Invoke();
            Release();
        }
        /// <summary>
        /// ������Ʈ�� ������ ��ġ�� �޾ƿ��� �Լ�
        /// </summary>
        /// <returns>������ ������Ʈ�� ��ġ</returns>
        protected abstract Vector3 GetEmitPosition();
        #endregion
    }
}