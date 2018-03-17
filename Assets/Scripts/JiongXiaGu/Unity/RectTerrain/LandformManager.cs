using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{


    public class LandformManager
    {
        private Transform SceneObjectsRoot;
        private LandformBake baker;

        public LandformManager()
        {
            SceneObjectsRoot = new GameObject("LandformSceneObjects").transform;
            baker = new LandformBake(SceneObjectsRoot);
        }

        /// <summary>
        /// 获取到指定坐标的高度;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            throw new NotImplementedException();
        }
    }
}
