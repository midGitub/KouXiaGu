using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{
    public struct BakeResult
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
    }
}
