using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain.Resources
{

    //public class LandformInfoPool
    //{
    //    private Dictionary<string, LandformInfo> infos = new Dictionary<string, LandformInfo>();
    //    private IReadOnlyDictionary<string, Description<LandformDescription>> descriptions;

    //    public LandformInfo GetOrLoad(string key)
    //    {
    //        LandformInfo info;
    //        if (!infos.TryGetValue(key, out info))
    //        {
    //            info = Load(key);
    //        }
    //        return info;
    //    }

    //    private LandformInfo Load(string key)
    //    {
    //        Description<LandformDescription> description;
    //        if (descriptions.TryGetValue(key, out description))
    //        {
    //            LandformInfo info = new LandformInfo(description.Content, description.Value);
    //            return info;
    //        }
    //        return default(LandformInfo);
    //    }
    //}

    public class RectTerrainInfo
    {
        public IReadOnlyDictionary<string, LandformInfo> Landform { get; private set; }

    }
}
