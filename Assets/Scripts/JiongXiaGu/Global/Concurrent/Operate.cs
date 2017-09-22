using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Concurrent
{

    /// <summary>
    /// 表示一个异步操作;
    /// </summary>
    public class Operate : AsyncOperation, IAsyncRequest
    {
        public Operate(Action operate)
        {
            this.operate = operate;
        }

        readonly Action operate;
        public bool InQueue { get; private set; }

        void IAsyncRequest.OnAddQueue()
        {
            if (InQueue)
                throw new ArgumentException("重复加入!");

            InQueue = true;
        }

        bool IAsyncRequest.Prepare()
        {
            return true;
        }

        bool IAsyncRequest.Operate()
        {
            try
            {
                operate();
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
            return false;
        }

        void IAsyncRequest.OnQuitQueue()
        {
            InQueue = false;
        }
    }
}
