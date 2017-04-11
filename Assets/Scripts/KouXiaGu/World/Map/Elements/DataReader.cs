using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map
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

}
