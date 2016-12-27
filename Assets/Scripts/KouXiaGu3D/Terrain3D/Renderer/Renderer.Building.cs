using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public sealed partial class Renderer : UnitySingleton<Renderer>
    {

        /// <summary>
        /// 获取到对地形进行改变的参数;
        /// </summary>
        class BuildingDecorate
        {

            [SerializeField]
            MeshDisplay displayMeshPool;

            /// <summary>
            /// 高度微调;
            /// </summary>
            [SerializeField]
            Shader heightAdjust;


            /// <summary>
            /// 获取到需要显示到场景的内容和网格;
            /// </summary>
            public List<KeyValuePair<BakingNode, MeshRenderer>> GetDisplayMeshs(IBakeRequest request, IEnumerable<BakingNode> bakingNodes)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// 高度调整图烘焙;
            /// </summary>
            public RenderTexture HeightAdjustMapRender(Texture heightMap)
            {
                throw new NotImplementedException();
            }

        }

    }

}
