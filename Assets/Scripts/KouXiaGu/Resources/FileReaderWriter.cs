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



    public interface IFileReaderWriter<T> : IReader<T>, IWriter<T>
    {
        ISingleFilePath File { get; set; }
    }

    public class FileReaderWriter<T> : IFileReaderWriter<T>
    {
        public FileReaderWriter(ISingleFilePath file, IFileSerializer<T> serializer)
        {
            File = file;
            Serializer = serializer;
        }

        public ISingleFilePath File { get; set; }
        public IFileSerializer<T> Serializer { get; set; }

        public virtual T Read()
        {
            string filePath = File.GetFullPath();
            return Serializer.Read(filePath);
        }

        public virtual void Write(T item, FileMode fileMode)
        {
            string filePath = File.GetFullPath();
            Serializer.Write(item, filePath, fileMode);
        }
    }



    public interface IFilesReaderWriter<T> : IReader<T>, IWriter<T>
    {
        IMultipleFilePath File { get; set; }
    }

    /// <summary>
    /// 使多个相同类型结合方法;
    /// </summary>
    public interface ICombiner<T>
    {
        T Combine(IEnumerable<T> items);
        IEnumerable<KeyValuePair<string, T>> Separate(T item);
    }

    /// <summary>
    /// 多个文件读取;
    /// </summary>
    public abstract class FilesReaderWriter<T> : IFilesReaderWriter<T>
    {
        public FilesReaderWriter(IMultipleFilePath multipleFile, IFileSerializer<T> serializer, ICombiner<T> combiner)
        {
            File = File;
            Serializer = serializer;
            Combiner = combiner;
        }

        public IMultipleFilePath File { get; set; }
        public IFileSerializer<T> Serializer { get; set; }
        public ICombiner<T> Combiner { get; set; }

        public T Read()
        {
            List<T> items = ReadAll();
            T item = Combiner.Combine(items);
            return item;
        }

        public List<T> ReadAll()
        {
            List<T> completed = new List<T>();
            var filePaths = File.GetExistentPaths();
            foreach (var path in filePaths)
            {
                try
                {
                    T item = Serializer.Read(path);
                    completed.Add(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return completed;
        }

        public void Write(T item, FileMode fileMode)
        {
            IEnumerable<KeyValuePair<string, T>> pairs = Combiner.Separate(item);
            foreach (var pair in pairs)
            {
                string path = File.CreateFilePath(pair.Key);
                Serializer.Write(pair.Value, path, fileMode);
            }
        }
    }



    public interface ICombiner<TSource, TResult>
    {
        TResult Combine(IEnumerable<TSource> items);
        IEnumerable<KeyValuePair<string, TSource>> Separate(TResult item);
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
            File = multipleFile;
            Serializer = serializer;
            Combiner = combiner;
        }

        public IMultipleFilePath File { get; set; }
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
            var filePaths = File.GetExistentPaths();
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

        public void Write(TResult item, FileMode fileMode)
        {
            IEnumerable<KeyValuePair<string, TSource>> pairs = Combiner.Separate(item);
            foreach (var pair in pairs)
            {
                string path = File.CreateFilePath(pair.Key);
                Serializer.Write(pair.Value, path, fileMode);
            }
        }
    }
}
