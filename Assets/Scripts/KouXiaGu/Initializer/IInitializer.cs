using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu
{

    public interface IInitializer
    {
        /// <summary>
        /// 开始初始化;
        /// </summary>
        Task StartInitialize();
    }
}
