using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    public interface IMonthObservable
    {
        IDisposable Subscribe(IMonthObserver observer);
    }

    /// <summary>
    /// 监视月份变化;
    /// </summary>
    public interface IMonthObserver
    {
        void OnNext(Months month);
    }

}
