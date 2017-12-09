using System;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形资源;
    /// </summary>
    [Serializable]
    public class LandformRes
    {
        [SerializeField]
        private Texture2D heightTex;
        [SerializeField]
        private Texture2D heightBlendTex;
        [SerializeField]
        private Texture2D diffuseTex;
        [SerializeField]
        private Texture2D diffuseBlendTex;

        public Texture2D HeightTex
        {
            get { return heightTex; }
            internal set { heightTex = value; }
        }

        public Texture2D HeightBlendTex
        {
            get { return heightBlendTex; }
            internal set { heightBlendTex = value; }
        }

        public Texture2D DiffuseTex
        {
            get { return diffuseTex; }
            internal set { diffuseTex = value; }
        }

        public Texture2D DiffuseBlendTex
        {
            get { return diffuseBlendTex; }
            internal set { diffuseBlendTex = value; }
        }

        public void Destroy()
        {
            if (heightTex != null)
            {
                UnityEngine.Object.Destroy(heightTex);
                heightTex = null;
            }

            if (heightBlendTex != null)
            {
                UnityEngine.Object.Destroy(heightBlendTex);
                heightBlendTex = null;
            }

            if (diffuseTex != null)
            {
                UnityEngine.Object.Destroy(diffuseTex);
                diffuseTex = null;
            }

            if (diffuseBlendTex != null)
            {
                UnityEngine.Object.Destroy(diffuseBlendTex);
                diffuseBlendTex = null;
            }
        }
    }
}
