using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RectTerrain
{
    public class LandformResPool : ResPool<LandformRes>
    {
        private IReadOnlyDictionary<string, Description<LandformDescription>> descriptions;

        protected override Task<LandformRes> Create(string key)
        {
            Description<LandformDescription> description;
            if (descriptions.TryGetValue(key, out description))
            {
                var info = LandformRes.CreateAsync(description.Content, description.Value);
                return info;
            }
            else
            {
                return null;
            }
        }

        protected override void Release(string key, Task<LandformRes> info)
        {
            throw new NotImplementedException();
        }
    }

}
