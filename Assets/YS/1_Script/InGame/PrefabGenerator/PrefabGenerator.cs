using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public abstract class PrefabGenerator : PoolingComponent
    {
        #region Field
        [LabelText("재생 시간")]
        public float duration;
        [LabelText("생성 위치 오프셋")]
        public Vector3 offset;
        [LabelText("생성 영역 회전량")]
        public Vector3 rot;

        [LabelText("방사 시작지연시간")]
        public float emitStartDelay;
        [LabelText("방사 시간")]
        public float emitDuration;
        [LabelText("총 생성 개수로 생성 주기 결정"), Tooltip("true : 총 생성 개수로 생성 주기 결정, false : 생성 주기로 총 생성 개수 결정")]
        public bool bUseTotalCount;
        [ShowIf("bUseTotalCount"), LabelText("    총 생성 개수")]
        public uint totalCount;
        [HideIf("bUseTotalCount"), LabelText("    초당 생성 수")]
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
                    // 객체 생성
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
        /// 게임오브젝트 생성 시 호출
        /// </summary>
        /// <param name="obj">생성된 게임오브젝트</param>
        protected virtual void OnEmitGameObject(GameObject obj)
        {
            OnEmitGameObjectEvent?.Invoke(obj);
        }
        /// <summary>
        /// 방사가 끝나면 호출
        /// </summary>
        protected virtual void OnEmissionEnd()
        {
            EmissionEndEvent?.Invoke();
        }
        /// <summary>
        /// LifeTime 시간이 지나면 호출
        /// </summary>
        protected virtual void OnLifeTimeEnd()
        {
            OnLifeTimeEndEvent?.Invoke();
            Release();
        }
        /// <summary>
        /// 오브젝트를 생성할 위치를 받아오는 함수
        /// </summary>
        /// <returns>생성할 오브젝트의 위치</returns>
        protected abstract Vector3 GetEmitPosition();
        #endregion
    }
}