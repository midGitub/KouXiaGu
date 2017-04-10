using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KouXiaGu
{

    public static class AsyncReaderExtensions
    {

        /// <summary>
        /// 在其它线程读取到;
        /// </summary>
        public static AsyncOperation<TResult> ReadAsync<TResult>(this IReader<TResult> reader)
        {
            var item = new AsyncReader<TResult>(reader);
            item.Start();
            return item;
        }

        /// <summary>
        /// 在其它线程读取到;
        /// </summary>
        public static AsyncOperation<TResult> ReadAsync<TResult, T1>(this IReader<TResult, T1> reader, T1 t1)
        {
            var item = new AsyncReader<TResult, T1>(reader, t1);
            item.Start();
            return item;
        }

        class AsyncReader<TResult> : AsyncOperation<TResult>
        {
            IReader<TResult> reader;

            public AsyncReader(IReader<TResult> reader)
            {
                this.reader = reader;
            }

            protected override TResult Operate()
            {
                return reader.Read();
            }
        }

        class AsyncReader<TResult, T1> : AsyncOperation<TResult>
        {
            IReader<TResult, T1> reader;
            T1 t1;

            public AsyncReader(IReader<TResult, T1> reader, T1 t1)
            {
                this.reader = reader;
                this.t1 = t1;
            }

            protected override TResult Operate()
            {
                return reader.Read(t1);
            }
        }

    }


}
