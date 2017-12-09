using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    public struct LandformBakeResult
    {
        private Texture2D diffuseMap;
        private Texture2D heightMap;

        public Texture2D DiffuseMap
        {
            get { return diffuseMap; }
            set { diffuseMap = value; }
        }

        public Texture2D HeightMap
        {
            get { return heightMap; }
            set { heightMap = value; }
        }

        public void Destroy()
        {
            UnityHelper.DestroyAndSetNull(ref diffuseMap);
            UnityHelper.DestroyAndSetNull(ref heightMap);
        }
    }
}
