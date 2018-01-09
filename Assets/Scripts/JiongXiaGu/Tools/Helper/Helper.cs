using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    internal static class Helper
    {

        /// <summary>
        /// 不进行任何操作;
        /// </summary>
        public static T Initialization<T>(this Lazy<T> obj)
        {
            return obj.Value;
        }

        /// <summary>
        /// 创建通过using语法使用的处置类;
        /// </summary>
        public static IDisposable CreateDisposer(Action action)
        {
            return new Disposer(action);
        }

        private struct Disposer : IDisposable
        {
            private Action action;

            public Disposer(Action action)
            {
                if (action == null)
                    throw new ArgumentNullException(nameof(action));

                this.action = action;
            }

            public void Dispose()
            {
                if (action != null)
                {
                    action.Invoke();
                    action = null;
                }
            }
        }
    }
}
