using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace KouXiaGu.Terrain
{

    /// <summary>
    /// 获取到地貌信息和
    /// 负责提供地形贴图数据;
    /// </summary>
    public static class LandformManager
    {

        /// <summary>
        /// 地貌信息描述文件文件名;
        /// </summary>
        const string definitionFileName = "LandformDefinition.xml";

        /// <summary>
        /// 地貌信息描述文件路径;
        /// </summary>
        public static string DefinitionFilePath
        {
            get { return ResourcePath.CombineConfiguration(definitionFileName); }
        }


        /// <summary>
        /// 用于保存已经准备完毕的地貌信息;
        /// </summary>
        static readonly Dictionary<int, Landform> landformDefinitions = new Dictionary<int, Landform>();


    }

}
