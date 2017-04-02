using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Globalization
{

    public interface ITextObserver
    {
        void UpdateTexts(IDictionary<string, string> textDictionary);
    }

}
