using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Inputs
{


    public class FunctionInfo
    {
        /// <summary>
        /// 功能名称;
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 功能名称,提供本地化组件;
        /// </summary>
        public string l10nName { get; private set; }

        /// <summary>
        /// 按键信息;
        /// </summary>
        public CustomKey2 Key { get; private set; }
    }
}
