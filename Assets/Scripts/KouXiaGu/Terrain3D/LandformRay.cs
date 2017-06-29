using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 提供对地形的射线功能;
    /// </summary>
    public class LandformRay
    {
        static readonly LandformRay instance = new LandformRay();

        public static LandformRay Instance
        {
            get { return instance; }
        }

        [CustomUnityLayer("地形层")]
        public const string LayerName = "Landform";
        const float DefaultRayMaxDistance = 8000f;

        public LandformRay()
        {
            RayLayerMask = LayerMask.GetMask(LayerName);
            RayLayer = LayerMask.NameToLayer(LayerName);
        }

        public int RayLayerMask { get; private set; }
        public int RayLayer { get; private set; }

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

        bool Raycast(Ray ray, out RaycastHit raycastHit)
        {
            return Physics.Raycast(ray, out raycastHit, DefaultRayMaxDistance, RayLayerMask, QueryTriggerInteraction.Collide);
        }

    }

}
