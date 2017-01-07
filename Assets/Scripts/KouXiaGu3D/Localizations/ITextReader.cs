using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    public interface ITextReader
    {
        IEnumerable<TextItem> ReadTexts();
    }

}
