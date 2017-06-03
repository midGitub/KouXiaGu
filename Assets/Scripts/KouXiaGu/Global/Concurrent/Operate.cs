using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Concurrent
{

    /// <summary>
    /// 表示一个异步操作;
    /// </summary>
    public class Operate :AsyncOperation, IAsyncRequest
    {
        public Operate(Action operate)
        {
            this.operate = operate;
        }

        readonly Action operate;
        public bool IsInQueue { get; private set; }

        void IAsyncRequest.AddQueue()
        {
            if (IsInQueue)
                throw new ArgumentException("重复加入!");

            IsInQueue = true;
        }

        void IAsyncRequest.Operate()
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
            finally
            {
                IsInQueue = true;
            }
        }
    }
}
