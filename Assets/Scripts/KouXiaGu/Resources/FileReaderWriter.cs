using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources
{
    
    public interface IReader<T>
    {
        /// <summary>
        /// 读取到文件;
        /// </summary>
        T Read();
    }

    public interface IReader<TResult, T>
    {
        /// <summary>
        /// 读取到文件;
        /// </summary>
        T Read(T t);
    }

    public interface IWriter<T>
    {
        /// <summary>
        /// 输出/保存到;
        /// </summary>
        void Write(T item, FileMode fileMode);
    }



    public interface IFileReaderWriter<T> : IReader<T>, IWriter<T>
    {
        ISingleFilePath FilePath { get; set; }
    }

    public class FileReaderWriter<T> : IFileReaderWriter<T>
    {
        public FileReaderWriter(ISingleFilePath file, IFileSerializer<T> serializer)
        {
            FilePath = file;
            Serializer = serializer;
        }

        public ISingleFilePath FilePath { get; set; }
        public IFileSerializer<T> Serializer { get; set; }

        public virtual T Read()
        {
            string filePath = FilePath.GetFullPath();
            return Serializer.Read(filePath);
        }

        public virtual void Write(T item, FileMode fileMode = FileMode.Create)
        {
            string filePath = FilePath.GetFullPath();
            Serializer.Write(item, filePath, fileMode);
        }
    }



    public interface IFilesReaderWriter<T> : IReader<T>, IWriter<T>
    {
        IMultipleFilePath FilePath { get; set; }
    }

    public interface ICombiner<TSource, TResult>
    {
        TResult Combine(IEnumerable<TSource> items);

        /// <summary>
        /// 拆分数据;
        /// </summary>
        IEnumerable<WriteInfo<TSource>> Separate(TResult item);
    }

    public struct WriteInfo<T>
    {
        public WriteInfo(string name, T data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; private set; }
        public T Data { get; private set; }
    }


    /// <summary>
    /// 读取多个文件;
    /// </summary>
    /// <typeparam name="TSource">从文件读取到的内容;</typeparam>
    /// <typeparam name="TResult">转换后的内容;</typeparam>
    public class FilesReaderWriter<TSource, TResult> : IFilesReaderWriter<TResult>
    {
        public FilesReaderWriter(IMultipleFilePath multipleFile, IFileSerializer<TSource> serializer, ICombiner<TSource, TResult> combiner)
        {
            FilePath = multipleFile;
            Serializer = serializer;
            Combiner = combiner;
        }

        public IMultipleFilePath FilePath { get; set; }
        public IFileSerializer<TSource> Serializer { get; set; }
        public ICombiner<TSource, TResult> Combiner { get; set; }

        public TResult Read()
        {
            List<TSource> items = ReadAll();
            TResult item = Combiner.Combine(items);
            return item;
        }

        public List<TSource> ReadAll()
        {
            List<TSource> completed = new List<TSource>();
            var filePaths = FilePath.GetExistentPaths();
            foreach (var path in filePaths)
            {
                try
                {
                    TSource item = Serializer.Read(path);
                    completed.Add(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return completed;
        }

        public void Write(TResult item, FileMode fileMode = FileMode.Create)
        {
            IEnumerable<WriteInfo<TSource>> infos = Combiner.Separate(item);
            foreach (var info in infos)
            {
                string path = FilePath.CreateFilePath(info.Name);
                Serializer.Write(info.Data, path, fileMode);
            }
        }


        public TSource Read(string name)
        {
            string path = FilePath.CreateFilePath(name);
            return Serializer.Read(path);
        }

        public void Write(TSource item, string name, FileMode fileMode = FileMode.Create)
        {
            string path = FilePath.CreateFilePath(name);
            Serializer.Write(item, path, fileMode);
        }

        public bool Exists(string name)
        {
            string path = FilePath.CreateFilePath(name);
            return System.IO.File.Exists(path);
        }

        public bool Delete(string name)
        {
            string path = FilePath.CreateFilePath(name);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }
    }


    /// <summary>
    /// 使多个相同类型结合方法;
    /// </summary>
    [Obsolete]
    public interface ICombiner<T> : ICombiner<T, T>
    {
    }

    /// <summary>
    /// 多个文件读取;
    /// </summary>
    [Obsolete]
    public abstract class FilesReaderWriter<T> : FilesReaderWriter<T, T>, IFilesReaderWriter<T>
    {
        public FilesReaderWriter(IMultipleFilePath multipleFile, IFileSerializer<T> serializer, ICombiner<T> combiner) : base(multipleFile, serializer, combiner)
        {
        }
    }
}
