using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using ProtoBuf;

namespace KouXiaGu.Resources
{

    public interface IFileSerializer<T>
    {
        T Read(string filePath);
        void Write(T item, string filePath, FileMode fileMode);
    }

    public sealed class ProtoFileSerializer<T> : IFileSerializer<T>
    {
        public T Read(string filePath)
        {
            using (Stream fStream = new FileStream(filePath, FileMode.Open))
            {
                return Serializer.Deserialize<T>(fStream);
            }
        }

        public void Write(T item, string filePath, FileMode fileMode)
        {
            using (Stream fStream = new FileStream(filePath, fileMode))
            {
                Serializer.Serialize(fStream, item);
            }
        }
    }

    public sealed class XmlFileSerializer<T> : IFileSerializer<T>
    {
        public XmlFileSerializer()
        {
            Serializer = new XmlSerializer(typeof(T));
        }

        public XmlSerializer Serializer { get; private set; }

        public T Read(string filePath)
        {
            T item = (T)Serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public void Write(T item, string filePath, FileMode fileMode)
        {
            Serializer.SerializeXiaGu(filePath, item);
        }
    }



    public class FileSerializer<T> : ISerializer<T>
    {
        public FileSerializer(IFilePath file, IFileSerializer<T> serializer)
        {
            File = file;
            Serializer = serializer;
        }

        public IFilePath File { get; set; }
        public IFileSerializer<T> Serializer { get; set; }

        public virtual T Read()
        {
            string filePath = File.GetMainPath();
            return Serializer.Read(filePath);
        }

        public virtual void Write(T item, FileMode fileMode)
        {
            string filePath = File.GetMainPath();
            Serializer.Write(item, filePath, fileMode);
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
    public abstract class FilesSerializer<T> : FileSerializer<T>, ISerializer<T>
    {
        public FilesSerializer(IFilePath multipleFile, IFileSerializer<T> serializer, ICombiner<T> combiner) : base(multipleFile, serializer)
        {
            Combiner = combiner;
        }

        public ICombiner<T> Combiner;

        public override T Read()
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
    public class FilesSerializer<TSource, TResult> : ISerializer<TResult>
    {
        public FilesSerializer(IFilePath multipleFile, IFileSerializer<TSource> serializer, ICombiner<TSource, TResult> combiner)
        {
            File = multipleFile;
            Serializer = serializer;
            Combiner = combiner;
        }

        public IFilePath File { get; set; }
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
            TSource source = Combiner.Separate(item);
            string filePath = File.GetMainPath();
            Serializer.Write(source, filePath, fileMode);
        }
    }
}
