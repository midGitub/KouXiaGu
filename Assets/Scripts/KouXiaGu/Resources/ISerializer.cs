using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    public interface ISerializer<T> : IReader<T>, IWriter<T>
    {
    }

    public interface IReader<T>
    {
        /// <summary>
        /// 读取到文件,若遇到异常则输出到日志;
        /// </summary>
        T Read();
    }

    public interface IWriter<T>
    {
        /// <summary>
        /// 输出/保存到;
        /// </summary>
        /// <param name="overlay">是否覆盖原文件?</param>
        void Write(T item, bool overlay);
    }
}
