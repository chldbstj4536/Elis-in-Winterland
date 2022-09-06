using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YS
{
    public class BezierTest : MonoBehaviour
    {
        public enum DRAW_GIZMOS_TYPE
        {
            ANIMATION,
            MOVING_LINE,
        }

        public Bezier bezier;
        // 목적지까지 걸리는 도착 시간
        public float speed;
        private float t;
        public DRAW_GIZMOS_TYPE type;
        

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            t = (t + Time.deltaTime * speed) % 1.0f;

            transform.position = bezier.GetBezierPosition(t);
        }

        private void OnDrawGizmos()
        {
            switch (type)
            {
                case DRAW_GIZMOS_TYPE.ANIMATION:
                    bezier.DrawGizmo(t);
                    break;
                case DRAW_GIZMOS_TYPE.MOVING_LINE:
                    bezier.DrawMovingLineGizmo();
                    break;
            }    
        }
    }
}