using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.xgLocalization
{

    public interface ITextReader
    {
        Language Language { get; }
        IEnumerable<TextItem> ReadTexts();
    }

}
