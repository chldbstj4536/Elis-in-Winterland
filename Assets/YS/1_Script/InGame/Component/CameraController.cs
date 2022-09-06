using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class CameraController : MonoBehaviour
    {
        private enum CAM_STATE
        {
            FIX_CENTER,
            END_OF_FIELD,
            FOLLOW_TARGET
        }
        public enum SHAKE_FLAG
        {
            RANDOM = 0x00,
            VERTICAL = 0x01,
            HORIZONTAL = 0x02,
            REDUCE = 0x00,
            CONSTANT = 0x10,
        }

        #region Field
        [Header("상태")]
        public Transform target;

        public float xCamMoveValue = 4.0f;
        public float moveLerpTime = 1.5f;
        public float endOfMap_XPos;

        public float zoomLevel = 1.0f;
        public float zoomLerpTime = 1.5f;

        private Camera mainCam;
        private CAM_STATE state = CAM_STATE.FIX_CENTER;
        private Vector3 curShakeVector = Vector3.zero;
        private float zoomDefaultLevel;
        #endregion

        #region Unity Methods
        // Start is called before the first frame update
        void Start()
        {
            mainCam = Camera.main;
            zoomDefaultLevel = mainCam.fieldOfView;
        }

        // Update is called once per frame
        void Update()
        {
            StateUpdate();

            Vector3 pos = transform.position;

            switch (state)
            {
                case CAM_STATE.FIX_CENTER:
                    pos.x = Mathf.Lerp(pos.x, 0, moveLerpTime * Time.deltaTime);
                    break;
                case CAM_STATE.END_OF_FIELD:
                    pos.x = Mathf.Lerp(pos.x, pos.x < 0.0f ? -endOfMap_XPos : endOfMap_XPos, moveLerpTime * Time.deltaTime);
                    break;
                case CAM_STATE.FOLLOW_TARGET:
                    pos.x = Mathf.Lerp(pos.x, target.position.x, moveLerpTime * Time.deltaTime);
                    break;
            }

            transform.position = pos;
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, zoomDefaultLevel / zoomLevel, zoomLerpTime * Time.deltaTime);
        }
        #endregion

        #region Methods
        void StateUpdate()
        {
            Vector3 targetPos = target.position;

            if (Mathf.Abs(targetPos.x) <= xCamMoveValue)
                state = CAM_STATE.FIX_CENTER;
            else if (endOfMap_XPos <= Mathf.Abs(targetPos.x))
                state = CAM_STATE.END_OF_FIELD;
            else
                state = CAM_STATE.FOLLOW_TARGET;
        }

        /**
         * <summary>
         * 줌 설정 (각각의 맴버 변수인 zoomLevel, zoomLerpTime을 직접 변경으로도 줌 설정 가능)
         * </summary>
         * <param name="level">줌 Level ex)2.0f = 2배줌, 0.5f = 0.5배줌</param>
         * <param name="lerpTime">얼마나 부드럽게 줌할지에 대한 보간시간변수</param>
        */
        public void Zoom(float level, float lerpTime)
        {
            zoomLevel = level;
            zoomLerpTime = lerpTime;
        }


        /**
         * <summary>
         * 카메라 흔들기
         * </summary>
         * <param name="intensity">카메라의 움직임 강도</param>
         * <param name="time">총 흔드는 시간</param>
         * <param name="intervalTime">
         * 흔들림 간격
         * ex)time = 1, intervalTime = 0.1f 면 총 10번 흔듦
         * </param>
         * <param name="type">
         * RANDOM(default), VERTICAL, HORIZONTAL : 어느 방향으로 흔들지에 대한 Flag
         * REDUCE(default), CONSTANT : 시간이 지나면서 흔들림이 감소할지에 대한 Flag
         * </param>
         */
        public void ShakeCamera(float intensity, float time, float intervalTime, SHAKE_FLAG type)
        {
            StartCoroutine(ShakeCoroutine(intensity, time, intervalTime, type));
        }

        private IEnumerator ShakeCoroutine(float intensity, float time, float intervalTime, SHAKE_FLAG type)
        {
            WaitForSeconds interval = CachedWaitForSeconds.Get(intervalTime);

            SHAKE_FLAG dirState = (SHAKE_FLAG)((byte)type & 0x0f);
            SHAKE_FLAG intensityState = (SHAKE_FLAG)((byte)type & 0xf0);

            Vector3 dir = new Vector3(0.0f, 0.0f, 0.0f);
            float remainingTime = time;
            float curIntensity = intensity;

            switch (dirState)
            {
                case SHAKE_FLAG.VERTICAL:
                    dir.x = 1.0f;
                    break;
                case SHAKE_FLAG.HORIZONTAL:
                    dir.y = 1.0f;
                    break;
            }

            while (remainingTime > 0.0f)
            {
                if (dirState == SHAKE_FLAG.RANDOM)
                    dir = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), Vector3.forward) * Vector3.right;
                else
                    dir = -dir;

                if (intensityState == SHAKE_FLAG.REDUCE)
                    curIntensity = intensity * (remainingTime / time);

                transform.position -= curShakeVector;
                curShakeVector = dir * curIntensity;
                transform.position += curShakeVector;

                remainingTime -= intervalTime;
                yield return interval;
            }

            transform.position -= curShakeVector;
            curShakeVector = Vector3.zero;
        }
        #endregion
    }
}