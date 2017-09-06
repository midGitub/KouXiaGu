using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{

    public static class RequestDispatcherExtensions
    {

        public static IRequest Add(this IRequestDispatcher dispatcher, Action action)
        {
            var request = new Request(action);
            dispatcher.Add(request);
            return request;
        }

        class Request : IRequest
        {
            public Request(Action action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                this.action = action;
                IsCompleted = false;
            }

            readonly Action action;
            public bool IsCompleted { get; private set; }

            public void MoveNext()
            {
                action();
                IsCompleted = true;
            }
        }


        public static IRequest<T> Add<T>(this IRequestDispatcher dispatcher, Func<T> func)
        {
            var request = new Request<T>(func);
            dispatcher.Add(request);
            return request;
        }

        class Request<T> : IRequest<T>
        {
            public Request(Func<T> func)
            {
                if (func == null)
                    throw new ArgumentNullException("func");

                this.func = func;
                IsCompleted = false;
            }

            readonly Func<T> func;
            public bool IsCompleted { get; private set; }
            public T Result { get; private set; }

            public void MoveNext()
            {
                Result = func();
                IsCompleted = true;
            }
        }
    }
}
