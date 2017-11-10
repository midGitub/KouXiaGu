using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 资源定义;
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// 核心资源;
        /// </summary>
        public ILoadableResource Core
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 拓展资源;
        /// </summary>
        public IReadOnlyCollection<DlcInfo> Dlc
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 模组资源;
        /// </summary>
        public IReadOnlyCollection<ModInfo> Mod
        {
            get { throw new NotImplementedException(); }
        }
    }
}
