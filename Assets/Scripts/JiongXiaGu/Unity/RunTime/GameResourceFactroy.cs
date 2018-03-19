using JiongXiaGu.Unity.RectTerrain;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏资源读取方法;
    /// </summary>
    public class GameResourceFactroy
    {
        private readonly LandformResourceFactroy landformResourceFactroy = new LandformResourceFactroy();

        /// <summary>
        /// 读取游戏资源;
        /// </summary>
        public GameResource Read(IEnumerable<Modification> modifications, IProgress<string> progress)
        {
            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            GameResource resource = new GameResource();

            var modificationList = resource.Modifications = new List<Modification>(modifications);

            progress?.Report("读取地形资源");
            resource.LandformResource = landformResourceFactroy.Read(modificationList, AddMode.AddOrUpdate);



            return resource;
        }
    }
}
