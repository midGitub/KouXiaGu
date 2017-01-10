using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.XmlLocalization
{


    public interface ITextWriter
    {
        Language Language { get; }
        void WriteTexts(IEnumerable<TextItem> texts);
    }

}
