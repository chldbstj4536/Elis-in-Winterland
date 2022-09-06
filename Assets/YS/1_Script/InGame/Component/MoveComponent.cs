using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class MoveComponent : MonoBehaviour
    {
        enum MOVE_STATE
        {
            STOP,
            DESTINATION,
            DIRECTION,
            TARGET,
            FIXED,
            KNOCKBACK,
        }

        #region Field
        [BoxGroup("디버그", true, true), DisableIf("@true"), LabelText("이동속도"), SerializeField]
        private float moveSpeed = 1.0f;
        [BoxGroup("디버그", true, true), DisableIf("@true"), LabelText("이동속도 계수"), SerializeField]
        private float moveSpdCoef = 1.0f;     // 이동 속도 계수
        [BoxGroup("디버그", true, true), DisableIf("@true"), ShowInInspector, LabelText("현재 이동 상태")]

        public event OnChangedValue OnChangedMoveSpeed;
        public event OnChangedValue OnChangedMoveSpeedCoef;
        public event OnChangedValue OnChangedTotalMoveSpeed;

        private MOVE_STATE state;
        private Vector3 dest;
        private Vector3 direction;
        private Vector3 lastPos;
        private Transform target;

        private float knockbackSpeed;
        [BoxGroup("디버그", true, true), DisableIf("@true"), ShowInInspector, LabelText("넉백 시 도달 위치")]
        private Vector3 knockbackDest;

        public event Delegate_NoRetVal_NoParam OnArrive;
        public event Delegate_NoRetVal_NoParam OnStop;
        public event Delegate_NoRetVal_NoParam OnMoveStart;
        public event OnMoving OnMovingEvent;
        public event Delegate_NoRetVal_NoParam OnKnockbackStart;
        public event Delegate_NoRetVal_NoParam OnKnockbackEnd;

        public delegate void OnMoving(Vector3 lastPos, Vector3 curPos);
        #endregion

        #region Properties
        public float MoveSpeed
        {
            get { return moveSpeed; }
            set
            {
                moveSpeed = value;
                OnChangedMoveSpeed?.Invoke();
                OnChangedTotalMoveSpeed?.Invoke();
            }
        }
        public float MoveSpeedCoef
        {
            get { return moveSpdCoef; }
            set
            {
                moveSpdCoef = value;
                OnChangedMoveSpeedCoef?.Invoke();
                OnChangedTotalMoveSpeed?.Invoke();
            }
        }
        public float TotalMoveSpeed => moveSpeed * moveSpdCoef;
        public Vector3 Direction => IsMoving ? direction : Vector3.zero;

        /**
         * <summary>
         * 움직이는 중인가?
         * </summary>
         */
        public bool IsMoving => state != MOVE_STATE.STOP && state != MOVE_STATE.FIXED;
        public bool IsInKnockBack => state == MOVE_STATE.KNOCKBACK;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (!IsMoving)
                return;

            lastPos = transform.position;

            switch (state)
            {
                case MOVE_STATE.DESTINATION:
                    transform.position += Time.deltaTime * TotalMoveSpeed * direction;
                    Arrived(direction, dest);
                    break;

                case MOVE_STATE.DIRECTION:
                    transform.position += Time.deltaTime * TotalMoveSpeed * direction;
                    break;

                case MOVE_STATE.TARGET:
                    direction = (target.position - transform.position).normalized;
                    transform.position += Time.deltaTime * TotalMoveSpeed * direction;
                    Arrived(direction, target.position);
                    break;
                case MOVE_STATE.KNOCKBACK:
                    Vector3 knockbackDir = (knockbackDest - transform.position).normalized;
                    transform.position += Time.deltaTime * knockbackSpeed * knockbackDir;
                    ArrivedKnockBack(knockbackDir, knockbackDest);
                    break;
            }

            OnMovingEvent?.Invoke(lastPos, transform.position);
        }
        #endregion

        #region Methods
        /**
         * <summary>
         * 특정 목적지로 이동시키는 함수
         * </summary>
         * <param name="dest">
         * 이동하고자 하는 목적지
         * </param>
         */
        public void MoveToDestination(Vector3 dest)
        {
            if (state == MOVE_STATE.FIXED || state == MOVE_STATE.KNOCKBACK)
                return;

            if (!IsMoving) OnMoveStart?.Invoke();

            this.dest = dest;
            direction = (dest - transform.position).normalized;
            state = MOVE_STATE.DESTINATION;
        }

        /**
         * <summary>
         * 특정 방향으로 이동시키는 함수
         * </summary>
         * <param name="dir">
         * 이동하고자 하는 방향 (정규화된 값을 넣어야함)
         * </param>
         */
        public void MoveToDirection(Vector3 dir)
        {
            if (state == MOVE_STATE.FIXED || state == MOVE_STATE.KNOCKBACK)
                return;

            if (!IsMoving) OnMoveStart?.Invoke();

            direction = dir;
            state = MOVE_STATE.DIRECTION;
        }

        /**
         * <summary>
         * 특정 대상을 향해 이동시키는 함수
         * </summary>
         * <param name="target">
         * 추적할 대상
         * </param>
         */
        public void MoveToTarget(Transform target)
        {
            if (state == MOVE_STATE.FIXED || state == MOVE_STATE.KNOCKBACK)
                return;

            if (!IsMoving) OnMoveStart?.Invoke();

            this.target = target;
            state = MOVE_STATE.TARGET;
        }
        public void KnockBack(Vector3 dest, float speed)
        {
            knockbackSpeed = speed;
            knockbackDest = dest;

            state = MOVE_STATE.KNOCKBACK;

            OnKnockbackStart?.Invoke();
        }

        /**
         * <summary>
         * 정지
         * </summary>
         */
        public virtual void Stop()
        {
            if (state == MOVE_STATE.STOP || state == MOVE_STATE.KNOCKBACK)
                return;

            state = MOVE_STATE.STOP;
            OnStop?.Invoke();
        }

        public void FixMove()
        {
            Stop();

            state = MOVE_STATE.FIXED;
        }


        /*
         * 이동 후 도착했는지 계산
         * @param dir   이동 전의 방향
         * @param dest  목적지
         */
        private void Arrived(Vector3 dir, Vector3 dest)
        {
            if (dir == Vector3.zero || Vector3.Dot((dest - transform.position).normalized, dir) < 0.0f)
            {
                transform.position = dest;
                OnArrive?.Invoke();
                Stop();
            }
        }
        /*
         * 넉백 후 도착했는지 계산
         * @param dir   이동 전의 방향
         * @param dest  목적지
         */
        private void ArrivedKnockBack(Vector3 dir, Vector3 dest)
        {
            if (dir == Vector3.zero || Vector3.Dot((dest - transform.position).normalized, dir) < 0.0f)
            {
                transform.position = dest;
                OnKnockbackEnd?.Invoke();
                state = MOVE_STATE.STOP;
            }
        }
        #endregion
    }
}