using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Diagnostics
{

    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
