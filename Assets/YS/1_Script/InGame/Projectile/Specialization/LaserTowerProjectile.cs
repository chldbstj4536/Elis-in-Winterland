using UnityEngine;
using Sirenix.OdinInspector;

namespace YS
{
    public class LaserTowerProjectile : LinearProjectile
    {
        private Vector3 laserPos;
        public Transform line;

        public override void OnMovingEvent(Vector3 lastPos, Vector3 curPos)
        {
            base.OnMovingEvent(lastPos, curPos);
            line.transform.localScale = new Vector3(1.0f, Vector3.Distance(laserPos, curPos), 1.0f);
            line.transform.rotation = Quaternion.FromToRotation(Vector3.up, (laserPos - curPos).normalized);
        }

        public void SetLaserTowerProjectile(Vector3 laserPos)
        {
            // 이펙트 생성 코드 필요
            this.laserPos = laserPos;
            line.transform.localScale = new Vector3(1.0f, Vector3.Distance(laserPos, transform.position), 1.0f);
            line.transform.rotation = Quaternion.FromToRotation(Vector3.up, (laserPos - transform.position).normalized);
        }
    }
}