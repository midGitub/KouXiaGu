using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.XmlLocalization
{


    public interface ITextObserver
    {
        void UpdateTexts(IReadOnlyDictionary textDictionary);
    }

}
