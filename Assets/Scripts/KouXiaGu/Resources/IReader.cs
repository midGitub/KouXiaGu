using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.Resources
{

    public interface IReader<T>
    {
        T Read();
    }

    public interface IReader<TResult, T>
    {
        TResult Read(T item);
    }

    public interface IWriter<T>
    {
        void Write(T item, FileMode fileMode);
    }

    public interface IWriter<TSource, T>
    {
        void Write(TSource source, T item, FileMode fileMode);
    }

    public interface IFileReaderWriter<T> : IReader<T>, IWriter<T>
    {
        ISingleFilePath FilePath { get; set; }
    }

    public interface IFilesReaderWriter<T> : IReader<T>, IWriter<T>
    {
        IMultipleFilePath FilePath { get; set; }
    }
}
