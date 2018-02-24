using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain.Landform
{

    /// <summary>
    /// 地形管理;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LandformController : MonoBehaviour
    {
        private LandformController()
        {
        }

        [SerializeField, Range(0, 64)]
        private float tessellation = 48f;
        [SerializeField, Range(0, 5)]
        private float displacement = 1.5f;

        /// <summary>
        /// 地形细分程度;
        /// </summary>
        public float Tessellation
        {
            get { return tessellation; }
        }

        /// <summary>
        /// 地形高度缩放;
        /// </summary>
        public float Displacement
        {
            get { return displacement; }
        }

        private void OnValidate()
        {
            LandformChunkRenderer.SetTessellation(tessellation);
            LandformChunkRenderer.SetDisplacement(displacement);
        }

        /// <summary>
        /// 获取到该坐标地形的高度;若未创建地形,则返回-1;
        /// </summary>
        public float GetHeight(Vector3 position)
        {
            throw new NotImplementedException();
        }
    }
}
