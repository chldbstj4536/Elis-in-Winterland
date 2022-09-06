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
        [LabelText("��ġ ������"), Tooltip("������(origin)�� ��ġ �������� ��ǥ��ŭ ��ǥ�̵��� �մϴ�.")]
        public Vector3 offset;
        [FoldoutGroup("HitBox")]
        [LabelText("����ĳ��Ʈ ����"), Tooltip("������(origin)���� ��� �������� ����(����)�� ������ ���� ���Ⱚ�Դϴ�.")]
        public Vector3 direction;
        [FoldoutGroup("HitBox"), Min(0.0001f)]
        [LabelText("����ĳ��Ʈ �ִ� ����"), Tooltip("���������� ����ĳ��Ʈ �������� �� ���� �ִ� ���̰��Դϴ�.")]
        public float maxDistance = 0.0001f;
        [FoldoutGroup("HitBox"), EnumToggleButtons]
        [LabelText("���� ���� Ÿ��"), Tooltip("LENGTH : ����ĳ��Ʈ �ִ� ���̿� ������ ����\nSIZE : ��Ʈ�ڽ��� ũ�⿡ ������ ����(ex: Sphere ��Ʈ�ڽ� -> ������, Box ��Ʈ�ڽ� -> ����ü�� ũ��")]
        public RANGE_TYPE_FLAG rangeTypeFlag;

        protected float length;
        protected float size;
        
        [FoldoutGroup("Gizmos", 5)]
        [LabelText("���� ����"), Tooltip("���������� ��Ʈ�ڽ� �����Դϴ�.")]
        public Color startGizmoColor = new Color(0.0f, 1.0f, 0.0f, 0.5f);
        [FoldoutGroup("Gizmos")]
        [LabelText("�L�� ����"), Tooltip("���������� ��Ʈ�ڽ� �����Դϴ�.")]
        public Color endGizmoColor = new Color(1.0f, 0.92f, 0.016f, 0.5f);
        [FoldoutGroup("Gizmos")]
        [LabelText("���� ���� ����"), Tooltip("������ ���� ������ ����ĳ��Ʈ ������ ���� �����Դϴ�.")]
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
         *  ������ HitBox�� layerMask�� �ش��ϴ� �浹ü���� ����
         *  </summary>
         *  <param name="origin">
         *  HitBox�� ���� (������ǥ)
         *  </param>
         *  <param name="rot">
         *  ��� ��ǥ�� rot���� ȸ��
         *  </param>
         *  <param name="bRadial">
         *  ���������ΰ�? (true�� ��� �浹ü ��ȯ, false�� ���� ���� �浹�� �浹ü ��ȯ)
         *  </param>
         *  <param name="layerMask">
         *  � ���̾ �˻����� ����
         *  </param>
         *  <returns>
         *  �浹ü�鿡 ���� ����
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
        /// data�� �ִ� �����͸� �ڽ��� �����Ϳ� �����Ѵ�
        /// </summary>
        /// <param name="data">������ ������</param>
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
        [FoldoutGroup("SphereHitBox", true), LabelText("���� ������"), Tooltip("���� ũ�⸦ ���մϴ�."), Min(0.0f)]
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
        [FoldoutGroup("SectorHitBox", true), LabelText("������ ����"), Tooltip("�������κ��� �󸶸�ŭ�� ũ����� �˻������� ���� ����"), Min(0.0f)]
        public float radius;
        [FoldoutGroup("SectorHitBox"), LabelText("������ ��"), Tooltip("����ĳ��Ʈ �������κ��� �󸶸�ŭ�� �������� �浹�������� ����"), Wrap(0.0f, 360.0f)]
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
        [FoldoutGroup("CapsuleHitBox", true), LabelText("ĸ���� ������"), Tooltip("������ �ʺ�(ĸ���� ������)�� ũ�⸦ ���մϴ�."), Min(0.0f)]
        public float radius;
        [FoldoutGroup("CapsuleHitBox", true), LabelText("ĸ���� ���� ��ġ"), Tooltip("������(���� + ������)���� �̾����� ������ ���մϴ�.\n(ĸ���� 2���� ���� �ϳ��� ���������� ����)")]
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

            // �ٵ�
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
        [FoldoutGroup("BoxHitBox", true), LabelText("�ڽ� ũ��"), Tooltip("������(origin)���κ��� �� �ุŭ�� �Ÿ��� �����Ͽ� �ڽ��� ũ�⸦ ���մϴ�.")]
        public Vector3 halfExtents;
        [FoldoutGroup("BoxHitBox"), LabelText("�ڽ� ȸ��"), Tooltip("�󸶸�ŭ�� ȸ���� �ڽ��� ���̸� ���� �浹�˻����� ���մϴ�.")]
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