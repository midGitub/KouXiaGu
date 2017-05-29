using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources
{


    public class FileXmlSerializer<T> : ISerializer<T>
    {
        public FileXmlSerializer(IFilePath file)
        {
            File = file;
            serializer = new XmlSerializer(typeof(T));
        }

        public IFilePath File { get; set; }
        protected XmlSerializer serializer { get; private set; }

        public virtual T Read()
        {
            string filePath = File.GetMainPath();
            return Read(filePath);
        }

        public T Read(string filePath)
        {
            T item = (T)serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public void Write(T item)
        {
            string filePath = File.GetMainPath();
            Write(item, filePath);
        }

        public void Write(T item, FileMode fileMode)
        {
            string filePath = File.GetMainPath();
            serializer.SerializeXiaGu(filePath, item, fileMode);
        }

        public void Write(T item, string filePath, FileMode fileMode = FileMode.Create)
        {
            serializer.SerializeXiaGu(filePath, item);
        }
    }


    public abstract class BasicFilesXmlSerializer<T>
    {
        public BasicFilesXmlSerializer(IFilePath file)
        {
            File = file;
            serializer = new XmlSerializer(typeof(T));
        }

        public IFilePath File { get; set; }
        protected XmlSerializer serializer { get; private set; }

        public List<T> ReadAll()
        {
            List<T> completed = new List<T>();
            var filePaths = File.GetExistentPaths();
            foreach (var path in filePaths)
            {
                try
                {
                    T item = Read(path);
                    completed.Add(item);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                }
            }
            return completed;
        }

        public T Read(string filePath)
        {
            T item = (T)serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public void Write(T item)
        {
            string filePath = File.GetMainPath();
            Write(item, filePath);
        }

        public void Write(T item, FileMode fileMode)
        {
            string filePath = File.GetMainPath();
            serializer.SerializeXiaGu(filePath, item, fileMode);
        }

        public void Write(T item, string filePath, FileMode fileMode = FileMode.Create)
        {
            serializer.SerializeXiaGu(filePath, item);
        }
    }

    /// <summary>
    /// 使多个相同类型结合方法;
    /// </summary>
    public interface ICombiner<T>
    {
        T Combine(IEnumerable<T> items);
    }

    /// <summary>
    /// 多个文件读取;
    /// </summary>
    public class FilesXmlSerializer<T> : BasicFilesXmlSerializer<T>, ISerializer<T>
    {
        public FilesXmlSerializer(IFilePath multipleFile, ICombiner<T> combiner) : base(multipleFile)
        {
            Combiner = combiner;
        }

        public ICombiner<T> Combiner { get; private set; }

        public T Read()
        {
            List<T> items = ReadAll();
            T item = Combiner.Combine(items);
            return item;
        }
    }


    public interface ICombiner<TSource, TResult>
    {
        TResult Combine(IEnumerable<TSource> items);
        TSource Separate(TResult item);
    }

    /// <summary>
    /// 读取多个文件;
    /// </summary>
    /// <typeparam name="TSource">从文件读取到的内容;</typeparam>
    /// <typeparam name="TResult">转换后的内容;</typeparam>
    public class FilesXmlSerializer<TSource, TResult> : BasicFilesXmlSerializer<TSource>, ISerializer<TResult>
    {
        public FilesXmlSerializer(IFilePath multipleFile, ICombiner<TSource, TResult> combiner) : base(multipleFile)
        {
            Combiner = combiner;
        }

        public ICombiner<TSource, TResult> Combiner { get; set; }

        public TResult Read()
        {
            List<TSource> items = ReadAll();
            TResult item = Combiner.Combine(items);
            return item;
        }

        public void Write(TResult item, FileMode fileMode)
        {
            TSource source = Combiner.Separate(item);
            Write(source, fileMode);
        }
    }
}
