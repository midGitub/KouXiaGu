using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    class TerrainParameter : UnitySington<TerrainParameter>
    {

        TerrainParameter()
        {
        }

        /// <summary>
        /// 地形Shader;
        /// </summary>
        public Shader TerrainShader;

    }

}
