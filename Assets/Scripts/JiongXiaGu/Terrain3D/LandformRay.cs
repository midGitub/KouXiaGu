using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.Terrain3D
{

    /// <summary>
    /// 提供对地形的射线功能;
    /// </summary>
    public sealed class LandformRay : SceneSington<LandformRay>
    {
        LandformRay()
        {
        }

        [CustomUnityLayer("地形层")]
        public const string LayerName = "Landform";
        const float DefaultRayMaxDistance = 8000f;

        [SerializeField]
        LayerMask rayLayerMask;

        public LayerMask RayLayerMask
        {
            get { return rayLayerMask; }
        }

        public int RayLayer
        {
            get { return LayerMask.NameToLayer(LayerName); }
        }

        void Awake()
        {
            SetInstance(this);
        }

        public bool Raycast(Ray ray, out RaycastHit raycastHit)
        {
            return Physics.Raycast(ray, out raycastHit, DefaultRayMaxDistance, RayLayerMask);
        }

        public bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit raycastHit)
        {
            return Physics.Raycast(origin, direction,out raycastHit, DefaultRayMaxDistance, RayLayerMask);
        }

        /// <summary>
        /// 获取到主摄像机的鼠标所指向的地形坐标,若无法获取到则返回默认值;
        /// </summary>
        public Vector3 MouseRayPointOrDefault()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Raycast(ray, out raycastHit))
            {
                return raycastHit.point;
            }
            return default(Vector3);
        }

        /// <summary>
        /// 获取到主摄像机的鼠标所指向的地形坐标,若无法获取到则返回 false;
        /// </summary>
        public bool TryGetMouseRayPoint(out Vector3 mousePoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Raycast(ray, out raycastHit))
            {
                mousePoint = raycastHit.point;
                return true;
            }
            else
            {
                mousePoint = default(Vector3);
                return false;
            }
        }
    }
}
