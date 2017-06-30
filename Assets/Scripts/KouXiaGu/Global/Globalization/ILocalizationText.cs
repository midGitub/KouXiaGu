using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    /// <summary>
    /// 本地化文本更新接口;
    /// </summary>
    public interface ILocalizationText
    {
        /// <summary>
        /// 当文本更新时调用;
        /// </summary>
        void OnLanguageUpdate(LanguagePack pack);
    }
}
