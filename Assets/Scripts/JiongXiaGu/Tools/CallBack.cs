using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{


    public struct CallBack : IDisposable
    {
        private Action CallBackAction { get; set; }

        public CallBack(Action callBack)
        {
            CallBackAction = callBack;
        }

        public void Dispose()
        {
            CallBackAction?.Invoke();
        }
    }
}
