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
        [Header("����")]
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
         * �� ���� (������ �ɹ� ������ zoomLevel, zoomLerpTime�� ���� �������ε� �� ���� ����)
         * </summary>
         * <param name="level">�� Level ex)2.0f = 2����, 0.5f = 0.5����</param>
         * <param name="lerpTime">�󸶳� �ε巴�� �������� ���� �����ð�����</param>
        */
        public void Zoom(float level, float lerpTime)
        {
            zoomLevel = level;
            zoomLerpTime = lerpTime;
        }


        /**
         * <summary>
         * ī�޶� ����
         * </summary>
         * <param name="intensity">ī�޶��� ������ ����</param>
         * <param name="time">�� ���� �ð�</param>
         * <param name="intervalTime">
         * ��鸲 ����
         * ex)time = 1, intervalTime = 0.1f �� �� 10�� ���
         * </param>
         * <param name="type">
         * RANDOM(default), VERTICAL, HORIZONTAL : ��� �������� ������� ���� Flag
         * REDUCE(default), CONSTANT : �ð��� �����鼭 ��鸲�� ���������� ���� Flag
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