using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;

namespace KouXiaGu.Resources
{

    public class FileXmlSerializer<T>
    {
        public FileXmlSerializer(SingleFilePath file)
        {
            File = file;
            serializer = new XmlSerializer(typeof(T));
        }

        public SingleFilePath File { get; private set; }
        protected XmlSerializer serializer { get; private set; }

        public T Read()
        {
            string filePath = File.GetFullPath();
            return Read(filePath);
        }

        /// <summary>
        /// 从文件读取到;
        /// </summary>
        public T Read(string filePath)
        {
            T item = (T)serializer.DeserializeXiaGu(filePath);
            return item;
        }

        public void Write(T item)
        {
            string filePath = File.GetFullPath();
            Write(item, filePath);
        }

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public void Write(T item, string filePath)
        {
            serializer.SerializeXiaGu(filePath, item);
        }
    }


    public class FilesXmlSerializer<T> : IReader<List<T>>
    {
        public FilesXmlSerializer(MultipleFilePath file)
        {
            this.file = file;
            serializer = new XmlSerializer(typeof(T));
        }

        protected MultipleFilePath file;
        XmlSerializer serializer;

        /// <summary>
        /// 读取到文件,若遇到异常则输出到日志;
        /// </summary>
        public List<T> Read()
        {
            ICollection<FailureInfo> faulted;
            List<T> result = Read(file, out faulted);
            LogFaulted(faulted);
            return result;
        }

        /// <summary>
        /// 将异常输出到日志;
        /// </summary>
        protected void LogFaulted(ICollection<FailureInfo> faulted)
        {
            if (faulted != null)
            {
                string errorStr = faulted.ToLog("读取资源时出现异常;");
                Debug.LogWarning(errorStr);
            }
        }

        /// <summary>
        /// 读取到所有;
        /// </summary>
        /// <param name="faulted">读取失败的文件,若不存在则为Null</param>
        /// <returns></returns>
        public List<T> Read(out ICollection<FailureInfo> faulted)
        {
            return Read(file, out faulted);
        }

        /// <summary>
        /// 读取到所有;
        /// </summary>
        /// <param name="filePath">文件路径接口</param>
        /// <param name="faulted">读取失败的文件,若不存在则为Null</param>
        /// <returns></returns>
        public List<T> Read(MultipleFilePath filePath, out ICollection<FailureInfo> faulted)
        {
            faulted = null;
            List<T> completed = new List<T>();

            IEnumerable<string> filePaths = filePath.FindFiles();
            foreach (var path in filePaths)
            {
                try
                {
                    T item = Read(path);
                    completed.Add(item);
                }
                catch (Exception ex)
                {
                    if (faulted == null)
                    {
                        faulted = new List<FailureInfo>();
                    }
                    var failureInfo = new FailureInfo(path, ex);
                    faulted.Add(failureInfo);
                }
            }

            return completed;
        }

        /// <summary>
        /// 读取到文件;
        /// </summary>
        public T Read(string filePath)
        {
            return (T)serializer.DeserializeXiaGu(filePath);
        }

        /// <summary>
        /// 输出到文件;
        /// </summary>
        public void Write(T item, string filePath)
        {
            serializer.SerializeXiaGu(filePath, item);
        }

        public struct FailureInfo
        {
            public FailureInfo(string path, Exception ex)
            {
                FilePath = path;
                Exception = ex;
            }

            public string FilePath { get; private set; }
            public Exception Exception { get; private set; }

            public override string ToString()
            {
                return "[FilePath:" + FilePath + ",Exception:" + Exception + "]";
            }
        }
    }
}
