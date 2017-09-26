using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化组件处置器;
    /// </summary>
    public interface ILanguageHandle
    {
        /// <summary>
        /// 当语言发生变化后调用;
        /// </summary>
        void OnLanguageChanged();
    }
}
