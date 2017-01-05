using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{


    public interface ITextWriter
    {

        string FilePath { get; }
        void WriteTexts(string language, IEnumerable<TextPack> texts);

    }

}
