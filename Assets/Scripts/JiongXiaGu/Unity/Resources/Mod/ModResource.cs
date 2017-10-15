using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组资源;
    /// </summary>
    public class ModResource
    {
        public ModInfo ModInfo { get; private set; }

        private Dictionary<Type, object> modResource;

        public ModResource(ModInfo modInfo)
        {
            if (modInfo == null)
                throw new ArgumentNullException(nameof(modInfo));

            ModInfo = modInfo;
            modResource = new Dictionary<Type, object>();
        }
    }
}
