using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{


    public interface ITextObserver
    {
        string Key { get; }
        void SetText(string text);

        /// <summary>
        /// 无法获取到对应的文本;
        /// </summary>
        void OnTextNotFound();
    }

}
