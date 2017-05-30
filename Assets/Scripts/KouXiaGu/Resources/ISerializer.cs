//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.Resources
//{

//    /// <summary>
//    /// 资源读取方式;
//    /// </summary>
//    public interface ISerializer<T> : IReader<T>, IWriter<T>
//    {
//        //ISingleFilePath File { get; set; }
//    }

//    public interface IReader<T>
//    {
//        /// <summary>
//        /// 读取到文件,若遇到异常则输出到日志;
//        /// </summary>
//        T Read();
//    }

//    public interface IWriter<T>
//    {
//        /// <summary>
//        /// 输出/保存到;
//        /// </summary>
//        /// <param name="fileMode">文件读取方式;</param>
//        void Write(T item, FileMode fileMode);
//    }
//}
