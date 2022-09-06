using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    [System.Serializable]
    public abstract class IHitBox
    {
        #region Field
        [FoldoutGroup("HitBox", true)]
        [LabelText("위치 오프셋"), Tooltip("시작점(origin)이 위치 오프셋의 좌표만큼 좌표이동을 합니다.")]
        public Vector3 offset;
        [FoldoutGroup("HitBox")]
        [LabelText("레이캐스트 방향"), Tooltip("시작점(origin)에서 어느 방향으로 광선(레이)를 쏠지에 대한 방향값입니다.")]
        public Vector3 direction;
        [FoldoutGroup("HitBox"), Min(0.0001f)]
        [LabelText("레이캐스트 최대 길이"), Tooltip("시작점에서 레이캐스트 방향으로 쏠 때의 최대 길이값입니다.")]
        public float maxDistance = 0.0001f;
        [FoldoutGroup("HitBox"), EnumToggleButtons]
        [LabelText("범위 적용 타입"), Tooltip("LENGTH : 레이캐스트 최대 길이에 범위값 적용\nSIZE : 히트박스의 크기에 범위값 적용(ex: Sphere 히트박스 -> 반지름, Box 히트박스 -> 육면체의 크기")]
        public RANGE_TYPE_FLAG rangeTypeFlag;

        protected float length;
        protected float size;
        
        [FoldoutGroup("Gizmos", 5)]
        [LabelText("시점 색상"), Tooltip("시점영역의 히트박스 색상값입니다.")]
        public Color startGizmoColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        [FoldoutGroup("Gizmos")]
        [LabelText("꿑점 색상"), Tooltip("끝점영역의 히트박스 색상값입니다.")]
        public Color endGizmoColor = new Color(1.0f, 0.92f, 0.016f, 0.5f);
        [FoldoutGroup("Gizmos")]
        [LabelText("레이 영역 색상"), Tooltip("시점과 끝점 사이의 레이캐스트 영역에 대한 색상값입니다.")]
        public Color areaColor = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        #endregion

        #region Properties
        public Vector3 Direction
        {
            get
            {
                Vector3 nDir = direction.normalized;
                if (nDir == Vector3.zero)
                    return Vector3.up;
                return nDir;
            }
        }
        public float TotalMaxDistance => maxDistance + length;
        #endregion

        #region Methods
        public IHitBox() { }
        public IHitBox(Vector3 offset, Vector3 direction, float maxDistance)
        {
            this.offset = offset;
            this.direction = direction;
            if (maxDistance < 0.0001f)
                this.maxDistance = 0.0001f;
            else
                this.maxDistance = maxDistance;
        }
        /** <summary>
         *  설정된 HitBox로 layerMask에 해당하는 충돌체들을 검출
         *  </summary>
         *  <param name="origin">
         *  HitBox의 원점 (월드좌표)
         *  </param>
         *  <param name="rot">
         *  모든 좌표를 rot으로 회전
         *  </param>
         *  <param name="bRadial">
         *  광역피해인가? (true면 모든 충돌체 반환, false면 가장 먼저 충돌한 충돌체 반환)
         *  </param>
         *  <param name="layerMask">
         *  어떤 레이어를 검사할지 설정
         *  </param>
         *  <returns>
         *  충돌체들에 대한 정보
         *  </returns>
         */
        public abstract RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask);
        public virtual Vector3 GetOrigin(Vector3 origin, Quaternion rot)
        {
            return origin + (rot * offset);
        }
        public void SetRange(float range)
        {
            if (Utility.HasFlag((int)rangeTypeFlag, (int)RANGE_TYPE_FLAG.LENGTH))
                length = range == 0.0f ? float.MaxValue : range;
            else
                length = 0.0f;

            if (Utility.HasFlag((int)rangeTypeFlag, (int)RANGE_TYPE_FLAG.SIZE))
                size = range == 0.0f ? float.MaxValue : range;
            else
                size = 0.0f;
        }
        public abstract IHitBox Instantiate();
        /// <summary>
        /// data에 있는 데이터를 자신의 데이터에 복사한다
        /// </summary>
        /// <param name="data">복사할 데이터</param>
        protected void CopyData(IHitBox data)
        {
            offset = data.offset;
            direction = data.direction;
            maxDistance  = data.maxDistance;
            rangeTypeFlag = data.rangeTypeFlag;
            length = data.length;
            size = data.size;
            startGizmoColor = data.startGizmoColor;
            endGizmoColor = data.endGizmoColor;
            areaColor = data.areaColor;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public abstract void DrawGizmos(Vector3 origin, Quaternion rot);
#endif
        #endregion

        [System.Flags]
        public enum RANGE_TYPE_FLAG
        {
            LENGTH = 0x01,
            SIZE = 0x02,
        }
    }
    [System.Serializable]
    public class RayHitcast : IHitBox
    {
        #region Methods
        public RayHitcast() { }
        public RayHitcast(Vector3 offset, Vector3 direction, float maxDistance)
            : base(offset, direction, maxDistance)
        {

        }

        public override RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask)
        {
            RaycastHit[] result;

            Vector3 originFinal = origin + (rot * offset);
            Vector3 dirFinal = rot * Direction;

            if (bRadial)
                result = Physics.RaycastAll(originFinal, dirFinal, TotalMaxDistance, layerMask);
            else
            {
                Physics.Raycast(originFinal, dirFinal, out RaycastHit singleResult, TotalMaxDistance, layerMask);
                if (singleResult.transform != null)
                    result = new RaycastHit[1] { singleResult };
                else
                    result = new RaycastHit[0];
            }

            return result;
        }
        public override IHitBox Instantiate()
        {
            RayHitcast ray = new RayHitcast();

            ray.CopyData(this);

            return ray;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public override void DrawGizmos(Vector3 origin, Quaternion rot)
        {
            Vector3 sp = origin + (rot * offset);
            Vector3 ep = sp + rot * Direction * TotalMaxDistance;

            Gizmos.color = startGizmoColor;
            Gizmos.DrawSphere(sp, 0.5f);
            Gizmos.color = endGizmoColor;
            Gizmos.DrawSphere(ep, 0.5f);
            Gizmos.color = areaColor;
            Gizmos.DrawLine(sp, ep);
        }
#endif
        #endregion
    }
    [System.Serializable]
    public class SphereHitcast : IHitBox
    {
        #region Field
        [FoldoutGroup("SphereHitBox", true), LabelText("구의 반지름"), Tooltip("구의 크기를 정합니다."), Min(0.0f)]
        public float radius;
        #endregion

        #region Properties
        public float TotalRadius => radius + size;
        #endregion

        #region Methods
        public SphereHitcast() { }
        public SphereHitcast(Vector3 offset, Vector3 direction, float maxDistance, float radius)
            : base(offset, direction, maxDistance)
        {
            this.radius = radius;
        }
        public override RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask)
        {
            RaycastHit[] result;
            Vector3 originFinal = origin + (rot * offset);
            Vector3 dirFinal = rot * Direction;

            if (bRadial)
                result = Physics.SphereCastAll(originFinal, TotalRadius, dirFinal, TotalMaxDistance, layerMask);
            else
            {
                Physics.SphereCast(originFinal, TotalRadius, dirFinal, out RaycastHit singleResult, TotalMaxDistance, layerMask);
                if (singleResult.transform != null)
                    result = new RaycastHit[1] { singleResult };
                else
                    result = new RaycastHit[0];
            }

            return result;
        }
        public override IHitBox Instantiate()
        {
            SphereHitcast sphere = new SphereHitcast();

            sphere.CopyData(this);
            sphere.radius = radius;

            return sphere;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public override void DrawGizmos(Vector3 origin, Quaternion rot)
        {
            float totalRadius = TotalRadius;

            Vector3 sp = origin + (rot * offset);
            Vector3 ep = sp + rot * Direction * TotalMaxDistance;

            Vector3 orthRight = rot * Direction == Vector3.up ? Vector3.right : Vector3.Cross(Vector3.up, rot * Direction).normalized;
            Vector3 sOffset = orthRight * totalRadius;

            Gizmos.color = areaColor;
            for (int i = 0; i < 60; i++)
            {
                Quaternion q = Quaternion.AngleAxis(i * 6, rot * Direction);
                Gizmos.DrawLine(sp + q * sOffset, ep + q * sOffset);
            }
            Gizmos.color = startGizmoColor;
            Gizmos.DrawSphere(sp, totalRadius);
            Gizmos.color = endGizmoColor;
            Gizmos.DrawSphere(ep, totalRadius);
        }
#endif
        #endregion
    }
    [System.Serializable]
    public class SectorHitcast : IHitBox
    {
        #region Field
        [FoldoutGroup("SectorHitBox", true), LabelText("원뿔의 길이"), Tooltip("원점으로부터 얼마만큼의 크기까지 검사할지에 대한 설정"), Min(0.0f)]
        public float radius;
        [FoldoutGroup("SectorHitBox"), LabelText("원뿔의 각"), Tooltip("레이캐스트 방향으로부터 얼마만큼의 각도까지 충돌혀용할지 설정"), Wrap(0.0f, 360.0f)]
        public float angle;
        #endregion

        #region Properties
        public float TotalRadius => radius + size;
        #endregion

        #region Methods
        public SectorHitcast() { }
        public SectorHitcast(Vector3 offset, Vector3 direction, float maxDistance, float radius, float angle)
            : base(offset, direction, maxDistance)
        {
            this.radius = radius;
            this.angle = angle;
        }
        public override RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask)
        {
            List<RaycastHit> result = new List<RaycastHit>();

            RaycastHit[] hits;

            Vector3 originFinal = origin + (rot * offset);
            Vector3 dirFinal = rot * Direction;

            if (bRadial)
                hits = Physics.SphereCastAll(originFinal, TotalRadius, dirFinal, TotalMaxDistance, layerMask);
            else
            {
                Physics.SphereCast(originFinal, TotalRadius, dirFinal, out RaycastHit hit, TotalMaxDistance, layerMask);
                if (hit.transform != null)
                    hits = new RaycastHit[1] { hit };
                else
                    hits = new RaycastHit[0];
            }

            foreach (var hit in hits)
            {
                Vector3 hitPos = Utility.GetHitPoint(originFinal, hit);
                hitPos.y = 0.0f;
                if (Vector3.Dot((hitPos - originFinal).normalized, dirFinal) >= Mathf.Cos(angle / 2 * Mathf.Deg2Rad))
                    result.Add(hit);
            }

            return result.ToArray();
        }
        public override IHitBox Instantiate()
        {
            SectorHitcast sector = new SectorHitcast();

            sector.CopyData(this);

            sector.radius = radius;
            sector.angle = angle;

            return sector;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public override void DrawGizmos(Vector3 origin, Quaternion rot)
        {
            Vector3 sp = origin + (rot * offset);
            Vector3 ep = sp + rot * Direction * TotalMaxDistance;

            Vector3 dp = rot * Direction * TotalRadius;
            Vector3 orthRight = rot * Direction;

            if (orthRight == Vector3.up)        orthRight = Vector3.right;
            else if (orthRight == Vector3.down) orthRight = Vector3.left;
            else                                orthRight = Vector3.Cross(Vector3.up, rot * Direction).normalized;

            for (int i = 0; i < 30; i++)
            {
                Vector3 rotOffset1 = Quaternion.AngleAxis(angle / 2, Quaternion.AngleAxis(i * 6, rot * Direction) * orthRight) * dp;
                Vector3 rotOffset2 = Quaternion.AngleAxis(angle / 2, Quaternion.AngleAxis((i * 6) + 180, rot * Direction) * orthRight) * dp;
                Vector3 scp1 = sp + rotOffset1;
                Vector3 ecp1 = ep + rotOffset1;
                Vector3 scp2 = sp + rotOffset2;
                Vector3 ecp2 = ep + rotOffset2;

                Gizmos.color = startGizmoColor;
                Gizmos.DrawLine(sp, scp1);
                Gizmos.DrawLine(sp, scp2);
                Gizmos.color = endGizmoColor;
                Gizmos.DrawLine(ep, ecp1);
                Gizmos.DrawLine(ep, ecp2);
                Gizmos.color = areaColor;
                if (Mathf.Sin(angle * Mathf.Deg2Rad) < 0)
                {
                    rotOffset1 = Quaternion.AngleAxis(90, Quaternion.AngleAxis(i * 6, rot * Direction) * orthRight) * dp;
                    rotOffset2 = Quaternion.AngleAxis(90, Quaternion.AngleAxis((i * 6) + 180, rot * Direction) * orthRight) * dp;
                    Gizmos.DrawLine(sp + rotOffset1, ep + rotOffset1);
                    Gizmos.DrawLine(sp + rotOffset2, ep + rotOffset2);
                }
                else
                {
                    Gizmos.DrawLine(scp1, ecp1);
                    Gizmos.DrawLine(scp2, ecp2);
                }

                float angle30 = angle / 60;
                for (int j = 0; j < 30; j++)
                {
                    Gizmos.color = startGizmoColor;
                    rotOffset1 = Quaternion.AngleAxis(angle30 * j, Quaternion.AngleAxis(i * 6, rot * Direction) * orthRight) * dp;
                    rotOffset2 = Quaternion.AngleAxis(angle30 * (j + 1), Quaternion.AngleAxis(i * 6, rot * Direction) * orthRight) * dp;
                    Gizmos.DrawLine(sp + rotOffset1, sp + rotOffset2);
                    Gizmos.color = endGizmoColor;
                    Gizmos.DrawLine(ep + rotOffset1, ep + rotOffset2);
                    Gizmos.color = startGizmoColor;
                    rotOffset1 = Quaternion.AngleAxis(angle30 * j, Quaternion.AngleAxis((i * 6) + 180, rot * Direction) * orthRight) * dp;
                    rotOffset2 = Quaternion.AngleAxis(angle30 * (j + 1), Quaternion.AngleAxis((i * 6) + 180, rot * Direction) * orthRight) * dp;
                    Gizmos.DrawLine(sp + rotOffset1, sp + rotOffset2);
                    Gizmos.color = endGizmoColor;
                    Gizmos.DrawLine(ep + rotOffset1, ep + rotOffset2);
                }
            }
        }
#endif
        #endregion
    }
    [System.Serializable]
    public class CapsuleHitcast : IHitBox
    {
        #region Field
        [FoldoutGroup("CapsuleHitBox", true), LabelText("캡슐의 반지름"), Tooltip("몸통의 너비(캡술의 반지름)의 크기를 정합니다."), Min(0.0f)]
        public float radius;
        [FoldoutGroup("CapsuleHitBox", true), LabelText("캡슐의 끝점 위치"), Tooltip("시작점(원점 + 오프셋)에서 이어지는 끝점을 정합니다.\n(캡슐은 2개의 점과 하나의 반지름으로 구성)")]
        public Vector3 endPoint;
        #endregion

        #region Properties
        public float TotalRadius => radius + size;
        #endregion

        #region Methods
        public CapsuleHitcast() { }
        public CapsuleHitcast(Vector3 offset, Vector3 endPoint, float radius, Vector3 direction, float maxDistance)
            : base(offset, direction, maxDistance)
        {
            this.endPoint = endPoint;
            this.radius = radius;
        }
        public override RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask)
        {
            RaycastHit[] result;

            Vector3 spFinal = origin + (rot * offset);
            Vector3 epFinal = origin + (rot * endPoint);
            Vector3 dirFinal = rot * Direction;

            if (bRadial)
                result = Physics.CapsuleCastAll(spFinal, epFinal, TotalRadius, dirFinal, TotalMaxDistance, layerMask);
            else
            {
                Physics.CapsuleCast(spFinal, epFinal, TotalRadius, dirFinal, out RaycastHit singleResult, TotalMaxDistance, layerMask);
                if (singleResult.transform != null)
                    result = new RaycastHit[1] { singleResult };
                else
                    result = new RaycastHit[0];
            }

            return result;
        }
        public override Vector3 GetOrigin(Vector3 origin, Quaternion rot)
        {
            Vector3 spFinal = origin + (rot * offset);
            Vector3 epFinal = origin + (rot * endPoint);

            return spFinal + (epFinal - spFinal) / 2;
        }
        public override IHitBox Instantiate()
        {
            CapsuleHitcast capsule = new CapsuleHitcast();

            capsule.CopyData(this);

            capsule.radius = radius;
            capsule.endPoint = endPoint;

            return capsule;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public override void DrawGizmos(Vector3 origin, Quaternion rot)
        {
            float totalRadius = TotalRadius;

            Vector3 sp1 = origin + (rot * offset);
            Vector3 sp2 = origin + (rot * endPoint);
            Vector3 ep1 = sp1 + rot * Direction * TotalMaxDistance;
            Vector3 ep2 = sp2 + rot * Direction * TotalMaxDistance;
            float capsuleDistance = (sp2 - sp1).magnitude;

            if (sp1.y > sp2.y) (sp2, sp1) = (sp1, sp2);
            if (ep1.y > ep2.y) (ep2, ep1) = (ep1, ep2);

            Vector3 dirRight = rot * Direction;
            if (dirRight == Vector3.up)         dirRight = Vector3.right;
            else if (dirRight == Vector3.down)  dirRight = Vector3.left;
            else                                dirRight = Vector3.Cross(Vector3.up, rot * Direction).normalized;

            Vector3 capsuleDir = (sp2 - sp1).normalized;
            if (capsuleDir == Vector3.zero) capsuleDir = Vector3.up;

            Vector3 capsuleRight = capsuleDir == Direction ? dirRight : Vector3.Cross(capsuleDir, rot * Direction).normalized;

            Vector3 rrp = capsuleRight * totalRadius;

            // 바디
            for (int i = 0; i <= 30; ++i)
            {
                Vector3 r1 = Quaternion.AngleAxis(i * 6, capsuleDir) * rrp;
                Vector3 r2 = Quaternion.AngleAxis((i + 1) * 6, capsuleDir) * rrp;
                Vector3 s1r1 = sp1 + r1;
                Vector3 s1r2 = sp1 + r2;
                Vector3 s2r1 = sp2 + r1;
                Vector3 s2r2 = sp2 + r2;

                r1 = Quaternion.AngleAxis(i * -6, capsuleDir) * rrp;
                r2 = Quaternion.AngleAxis((i + 1) * -6, capsuleDir) * rrp;
                Vector3 e1r1 = ep1 + r1;
                Vector3 e1r2 = ep1 + r2;
                Vector3 e2r1 = ep2 + r1;
                Vector3 e2r2 = ep2 + r2;

                Gizmos.color = startGizmoColor;
                Gizmos.DrawLine(s1r1, s1r2);
                Gizmos.DrawLine(s2r1, s2r2);
                Gizmos.DrawLine(s1r1, s2r1);

                Gizmos.color = endGizmoColor;
                Gizmos.DrawLine(e1r1, e1r2);
                Gizmos.DrawLine(e2r1, e2r2);
                Gizmos.DrawLine(e1r1, e2r1);

                Gizmos.color = areaColor;
                if (i == 0 || i == 30)
                {
                    int count = Mathf.Max(1, (int)(capsuleDistance / 2.5f));

                    for (int j = 0; j <= count; ++j)
                    {
                        float interval = j / (float)count * capsuleDistance;

                        Gizmos.DrawLine(s1r1 + (capsuleDir * interval), e1r1 + (interval * capsuleDir));
                    }
                }

                r1 = Quaternion.AngleAxis(i * -6, rot * Direction) * rrp;
                r2 = Quaternion.AngleAxis((i + 1) * -6, rot * Direction) * rrp;
                s1r1 = sp1 + r1;
                s1r2 = sp1 + r2;
                e1r1 = ep1 + r1;
                e1r2 = ep1 + r2;

                r1 = Quaternion.AngleAxis(i * 6, rot * Direction) * rrp;
                r2 = Quaternion.AngleAxis((i + 1) * 6, rot * Direction) * rrp;
                s2r1 = sp2 + r1;
                s2r2 = sp2 + r2;
                e2r1 = ep2 + r1;
                e2r2 = ep2 + r2;

                Gizmos.color = startGizmoColor;
                Gizmos.DrawLine(s1r1, s1r2);
                Gizmos.DrawLine(s2r1, s2r2);

                Gizmos.color = endGizmoColor;
                Gizmos.DrawLine(e1r1, e1r2);
                Gizmos.DrawLine(e2r1, e2r2);

                Gizmos.color = areaColor;
                Gizmos.DrawLine(s1r1, e1r1);
                Gizmos.DrawLine(s2r1, e2r1);
            }


            Gizmos.color = startGizmoColor;
            Gizmos.DrawWireSphere(sp1, totalRadius);
            Gizmos.DrawWireSphere(sp2, totalRadius);
            Gizmos.color = endGizmoColor;
            Gizmos.DrawWireSphere(ep1, totalRadius);
            Gizmos.DrawWireSphere(ep2, totalRadius);
            Gizmos.color = areaColor;
            Gizmos.DrawLine(sp1, ep1);
            Gizmos.DrawLine(sp2, ep2);
        }
#endif
        #endregion
    }
    [System.Serializable]
    public class BoxHitcast : IHitBox
    {
        #region Field
        [FoldoutGroup("BoxHitBox", true), LabelText("박스 크기"), Tooltip("시작점(origin)으로부터 각 축만큼의 거리를 설정하여 박스의 크기를 정합니다.")]
        public Vector3 halfExtents;
        [FoldoutGroup("BoxHitBox"), LabelText("박스 회전"), Tooltip("얼마만큼의 회전된 박스로 레이를 쏴서 충돌검사할지 정합니다.")]
        public Quaternion orientation;
        #endregion

        #region Properties
        public Vector3 TotalHalfExtents => halfExtents + Vector3.one * size;
        #endregion

        #region Methods
        public BoxHitcast() { }
        public BoxHitcast(Vector3 offset, Vector3 direction, float maxDistance, Vector3 halfExtents, Quaternion orientation)
            : base(offset, direction, maxDistance)
        {
            this.halfExtents = halfExtents;
            this.orientation = orientation;
        }
        public override RaycastHit[] Sweep(Vector3 origin, Quaternion rot, bool bRadial, int layerMask)
        {
            RaycastHit[] result;

            Vector3 originFinal = origin + (rot * offset);
            Vector3 dirFinal = rot * Direction;

            if (bRadial)
                result = Physics.BoxCastAll(originFinal, TotalHalfExtents, dirFinal, rot * orientation, TotalMaxDistance, layerMask);
            else
            {
                Physics.BoxCast(originFinal, TotalHalfExtents, dirFinal, out RaycastHit singleResult, orientation, TotalMaxDistance, layerMask);
                if (singleResult.transform != null)
                    result = new RaycastHit[1] { singleResult };
                else
                    result = new RaycastHit[0];
            }

            return result;
        }
        public override IHitBox Instantiate()
        {
            BoxHitcast box = new BoxHitcast();

            box.CopyData(this);

            box.halfExtents = halfExtents;
            box.orientation = orientation;

            return box;
        }
        #endregion

        #region Debug
#if UNITY_EDITOR
        public override void DrawGizmos(Vector3 origin, Quaternion rot)
        {
            Vector3 totalExtents = TotalHalfExtents;

            Vector3 sp = origin + (rot * offset);
            Vector3 ep = sp + rot * Direction * TotalMaxDistance;

            Vector3[] bp = new Vector3[8];

            Quaternion boxRot = rot * orientation;

            bp[0] = boxRot * new Vector3(totalExtents.x, totalExtents.y, totalExtents.z);
            bp[1] = boxRot * new Vector3(totalExtents.x, totalExtents.y, -totalExtents.z);
            bp[2] = boxRot * new Vector3(totalExtents.x, -totalExtents.y, totalExtents.z);
            bp[3] = boxRot * new Vector3(totalExtents.x, -totalExtents.y, -totalExtents.z);
            bp[4] = boxRot * new Vector3(-totalExtents.x, totalExtents.y, totalExtents.z);
            bp[5] = boxRot * new Vector3(-totalExtents.x, totalExtents.y, -totalExtents.z);
            bp[6] = boxRot * new Vector3(-totalExtents.x, -totalExtents.y, totalExtents.z);
            bp[7] = boxRot * new Vector3(-totalExtents.x, -totalExtents.y, -totalExtents.z);

            Gizmos.color = startGizmoColor;
            Gizmos.DrawLine(sp + bp[0], sp + bp[1]);
            Gizmos.DrawLine(sp + bp[0], sp + bp[2]);
            Gizmos.DrawLine(sp + bp[3], sp + bp[1]);
            Gizmos.DrawLine(sp + bp[3], sp + bp[2]);
            Gizmos.DrawLine(sp + bp[4], sp + bp[5]);
            Gizmos.DrawLine(sp + bp[4], sp + bp[6]);
            Gizmos.DrawLine(sp + bp[7], sp + bp[5]);
            Gizmos.DrawLine(sp + bp[7], sp + bp[6]);
            Gizmos.DrawLine(sp + bp[7], sp + bp[6]);
            Gizmos.DrawLine(sp + bp[0], sp + bp[4]);
            Gizmos.DrawLine(sp + bp[1], sp + bp[5]);
            Gizmos.DrawLine(sp + bp[2], sp + bp[6]);
            Gizmos.DrawLine(sp + bp[3], sp + bp[7]);

            Gizmos.color = endGizmoColor;
            Gizmos.DrawLine(ep + bp[0], ep + bp[1]);
            Gizmos.DrawLine(ep + bp[0], ep + bp[2]);
            Gizmos.DrawLine(ep + bp[3], ep + bp[1]);
            Gizmos.DrawLine(ep + bp[3], ep + bp[2]);
            Gizmos.DrawLine(ep + bp[4], ep + bp[5]);
            Gizmos.DrawLine(ep + bp[4], ep + bp[6]);
            Gizmos.DrawLine(ep + bp[7], ep + bp[5]);
            Gizmos.DrawLine(ep + bp[7], ep + bp[6]);
            Gizmos.DrawLine(ep + bp[7], ep + bp[6]);
            Gizmos.DrawLine(ep + bp[0], ep + bp[4]);
            Gizmos.DrawLine(ep + bp[1], ep + bp[5]);
            Gizmos.DrawLine(ep + bp[2], ep + bp[6]);
            Gizmos.DrawLine(ep + bp[3], ep + bp[7]);

            Gizmos.color = areaColor;
            for (int i = 0; i < 8; ++i)
                Gizmos.DrawLine(sp + bp[i], ep + bp[i]);
        }
#endif
        #endregion
    }
}