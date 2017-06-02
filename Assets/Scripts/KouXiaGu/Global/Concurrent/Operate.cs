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
        public bool IsOperating { get; private set; }

        void IAsyncRequest.AddQueue()
        {
            if (IsInQueue)
                throw new ArgumentException("重复加入!");

            IsInQueue = true;
        }

        IEnumerator IAsyncRequest.Operate()
        {
            try
            {
                IsOperating = true;
                operate();
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
            finally
            {
                IsOperating = false;
            }
            yield break;
        }

        void IAsyncRequest.OutQueue()
        {
            if (IsInQueue)
                throw new ArgumentException("时序错误;");

            IsInQueue = false;
        }
    }
}
