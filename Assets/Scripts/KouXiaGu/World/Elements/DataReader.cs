using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

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
                string newPath = Path.ChangeExtension(path, FileExtension);

                if (System.IO.File.Exists(newPath))
                {
                    yield return newPath;
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
            Write(item, filePath);
        }
    }


    public abstract class DataXmlSerializer<T> : DataReader<Dictionary<int, T>, T[]>
        where T : IMarked
    {
        protected static readonly XmlSerializer serializer = new XmlSerializer(typeof(T[]));

        public override string FileExtension
        {
            get { return ".xml"; }
        }

        public override Dictionary<int, T> Read(IEnumerable<string> filePaths)
        {
            Dictionary<int, T> dictionary = new Dictionary<int, T>();

            foreach (var filePath in filePaths)
            {
                var infos = Read(filePath);
                AddOrUpdate(dictionary, infos);
            }

            return dictionary;
        }

        T[] Read(string filePath)
        {
            var item = (T[])serializer.DeserializeXiaGu(filePath);
            return item;
        }

        void AddOrUpdate(Dictionary<int, T> dictionary, IEnumerable<T> infos)
        {
            foreach (var info in infos)
            {
                dictionary.AddOrUpdate(info.ID, info);
            }
        }

        public override void Write(T[] item, string filePath)
        {
            serializer.SerializeXiaGu(filePath, item);
        }
    }

}
