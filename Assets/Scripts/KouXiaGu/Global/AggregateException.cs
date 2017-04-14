using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 自建的 AggregateException 异常;
    /// </summary>
    public class AggregateException : Exception
    {

        const string DefaultMessage = "AggregateException";

        public AggregateException()
            : this(DefaultMessage)
        {
        }

        public AggregateException(string message)
            : base(message)
        {
            InnerExceptions = new ReadOnlyCollection<Exception>(new Exception[0]);
        }

        public AggregateException(string message, Exception innerException)
           : base(message, innerException)
        {
            if (innerException == null)
            {
                throw new ArgumentNullException("innerException");
            }
            InnerExceptions = new ReadOnlyCollection<Exception>(new Exception[] { innerException });
        }

        public AggregateException(IEnumerable<Exception> innerExceptions) :
          this(DefaultMessage, innerExceptions)
        {
        }

        public AggregateException(string message, IEnumerable<Exception> innerExceptions)
           : this(message, innerExceptions as IList<Exception> ?? (innerExceptions == null ? null : new List<Exception>(innerExceptions)))
        {
        }

        public AggregateException(params Exception[] innerExceptions) :
          this(DefaultMessage, (IList<Exception>)innerExceptions)
        {
        }

        public AggregateException(string message, params Exception[] innerExceptions) :
           this(message, (IList<Exception>)innerExceptions)
        {
        }

        AggregateException(string message, IList<Exception> innerExceptions)
           : base(message, innerExceptions != null && innerExceptions.Count > 0 ? innerExceptions[0] : null)
        {
            if (innerExceptions == null)
            {
                throw new ArgumentNullException("innerExceptions");
            }

            Exception[] exceptionsCopy = new Exception[innerExceptions.Count];

            for (int i = 0; i < exceptionsCopy.Length; i++)
            {
                exceptionsCopy[i] = innerExceptions[i];

                if (exceptionsCopy[i] == null)
                {
                    throw new ArgumentException();
                }
            }

            InnerExceptions = new ReadOnlyCollection<Exception>(exceptionsCopy);
        }

        public ReadOnlyCollection<Exception> InnerExceptions { get; private set; }

    }

}
