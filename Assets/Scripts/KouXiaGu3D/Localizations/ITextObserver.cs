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
    }

}
