using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    public class RectTerrainInfo
    {
        public IReadOnlyDictionary<string, LandformInfo> Landform { get; private set; }

    }

    public class LandformInfoPool
    {
        private Dictionary<string, Task<LandformInfo>> infos = new Dictionary<string, Task<LandformInfo>>();
        private IReadOnlyDictionary<string, Description<LandformDescription>> descriptions;

        /// <summary>
        /// 准备对应资源;
        /// </summary>
        public bool Prepare(string key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 准备所有资源;
        /// </summary>
        public void PrepareAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 尝试获取到对应资源,若还未加载完成则返回false;
        /// </summary>
        public bool TryGet(string key, out LandformInfo info)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到对应资源,若还未创建则创建到,若不存在此 key,则返回异常;
        /// </summary>
        public Task<LandformInfo> GetOrLoad(string key)
        {
            throw new NotImplementedException();
        }
    }

}
