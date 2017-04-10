using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public interface IWriter<TContent>
    {
        void Write(TContent item);
    }

    public interface IWriter<TContent, T1>
    {
        void Write(TContent item, T1 t1);
    }

}
