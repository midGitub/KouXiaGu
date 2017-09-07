using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KouXiaGu.Concurrent
{

    public static class RequestDispatcherExtensions
    {


        public static Request Add(this IRequestDispatcher dispatcher, Action action)
        {
            var request = new ActionRequest(action);
            dispatcher.Add(request);
            return request;
        }

        public static Request Add(this IRequestDispatcher dispatcher, Action action, CancellationToken cancellationToken)
        {
            var request = new ActionRequest(action, cancellationToken);
            dispatcher.Add(request);
            return request;
        }

        class ActionRequest : Request
        {
            public ActionRequest(Action action)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                this.action = action;
            }

            public ActionRequest(Action action, CancellationToken cancellationToken) : base(cancellationToken)
            {
                if (action == null)
                    throw new ArgumentNullException("action");

                this.action = action;
            }

            readonly Action action;

            protected override void Operate()
            {
                action();
            }
        }



        public static Request<T> Add<T>(this IRequestDispatcher dispatcher, Func<T> func)
        {
            var request = new FuncRequest<T>(func);
            dispatcher.Add(request);
            return request;
        }

        public static Request<T> Add<T>(this IRequestDispatcher dispatcher, Func<T> func, CancellationToken cancellationToken)
        {
            var request = new FuncRequest<T>(func, cancellationToken);
            dispatcher.Add(request);
            return request;
        }


        class FuncRequest<T> : Request<T>
        {
            public FuncRequest(Func<T> func)
            {
                if (func == null)
                    throw new ArgumentNullException("func");

                this.func = func;
            }

            public FuncRequest(Func<T> func, CancellationToken cancellationToken) : base(cancellationToken)
            {
                if (func == null)
                    throw new ArgumentNullException("action");

                this.func = func;
            }

            readonly Func<T> func;

            protected override T Operate()
            {
                return func();
            }
        }
    }
}
