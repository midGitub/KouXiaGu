using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Translates
{

    /// <summary>
    /// 本地化组件观察者;
    /// </summary>
    public interface ILanguageObserver
    {
        /// <summary>
        /// 当语言发生变化后调用(在Unity线程调用);
        /// </summary>
        void OnLanguageChanged();
    }
}
