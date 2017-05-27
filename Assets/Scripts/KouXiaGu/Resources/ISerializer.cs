using System;
using System.Collections.Generic;
using System.IO;
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
        /// <param name="fileMode">文件读取方式;</param>
        void Write(T item, FileMode fileMode);
    }
}
