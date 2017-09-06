using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{


    public static class AwaiterExtensions
    {

        public static RequestAwaiter GetAwaiter(this IRequest request)
        {
            return new RequestAwaiter(request);
        }

        public struct RequestAwaiter : INotifyCompletion
        {
            public RequestAwaiter(IRequest request)
            {
                this.request = request;
            }

            IRequest request;

            public bool IsCompleted
            {
                get { return request.IsCompleted; }
            }

            public void OnCompleted(Action continuation)
            {
                continuation();
            }

            public void GetResult()
            {
                return;
            }
        }

    }
}
