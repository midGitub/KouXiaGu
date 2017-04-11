using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 资源读取基类;
    /// </summary>
    public abstract class DataReader<TR, TW>
    {
        public abstract CustomFilePath File { get; }
        public abstract string FileExtension { get; }
        public abstract TR Read(IEnumerable<string> filePaths);
        public abstract void Write(TW item, string filePath);

        public TR Read()
        {
            var filePaths = GetFilePaths();
            return Read(filePaths);
        }

        protected virtual IEnumerable<string> GetFilePaths()
        {
            foreach (var path in File.GetFilePaths())
            {
                string filePath = Path.ChangeExtension(path, FileExtension);

                if (System.IO.File.Exists(filePath))
                {
                    yield return filePath;
                }
            }
        }

        public TR ReadFromDirectory(string dirPath)
        {
            string filePath = File.Combine(dirPath);
            string[] filePaths = new string[] { filePath };
            return Read(filePaths);
        }

        public void WriteToDirectory(TW item, string dirPath)
        {
            string filePath = File.Combine(dirPath);
            filePath = Path.ChangeExtension(filePath, FileExtension);

            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);

            Write(item, filePath);
        }
    }

    /// <summary>
    /// 读取为 字典格式;
    /// </summary>
    public abstract class DataDictionaryReader<T, TW> : DataReader<Dictionary<int, T>, TW>
        where T : ElementInfo
    {
        protected abstract IEnumerable<T> Read(string filePath);

        public override Dictionary<int, T> Read(IEnumerable<string> filePaths)
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddToDictionary(dictionary, infos);
            }

            return dictionary;
        }

        /// <summary>
        /// 添加到字典内,默认为替换已经存在的元素;
        /// </summary>
        protected virtual void AddToDictionary(Dictionary<int, T> dictionary, IEnumerable<T> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }
    }

}
