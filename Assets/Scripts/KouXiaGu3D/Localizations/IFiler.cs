using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Localizations
{

    public interface IFiler
    {

        IEnumerable<TextPack> ReadTexts();
        void WriteTexts(IEnumerable<TextPack> texts);

    }

}
