using JiongXiaGu.Unity.Scenarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏世界,在游戏场景创建;负责对时间进行更新;
    /// </summary>
    public class Wrold
    {
        /// <summary>
        /// 世界资源;
        /// </summary>
        public WroldResource Resource { get; private set; }

        public Wrold(WroldResource resource)
        {
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
