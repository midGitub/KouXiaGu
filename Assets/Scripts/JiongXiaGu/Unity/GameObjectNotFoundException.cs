using System;
using System.Runtime.Serialization;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 当无法找到指定 GameObject 所引发的异常;
    /// </summary>
    public class GameObjectNotFoundException : Exception
    {
        public GameObjectNotFoundException()
        {
        }

        public GameObjectNotFoundException(string message) : base(message)
        {
        }

        public GameObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GameObjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
