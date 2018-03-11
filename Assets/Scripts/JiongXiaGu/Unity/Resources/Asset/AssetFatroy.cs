using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{


    public class AssetFatroy
    {
        public static AssetFatroy Default { get; private set; }

        /// <summary>
        /// 从流数据获取到资产;
        /// </summary>
        public T Load<T>(Stream stream)
            where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从 AssetBundle 获取到资产;
        /// </summary>
        public T Load<T>(AssetBundle assetBundle)
            where T : UnityEngine.Object
        {
            throw new NotImplementedException();
        }
    }
}
