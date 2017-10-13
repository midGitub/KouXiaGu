using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.Archives
{

    public class ArchiveDescriptionReader
    {
        public ArchiveDescriptionReader() : this(new XmlSerializer<ArchiveDescription>(), "Info")
        {
        }

        public ArchiveDescriptionReader(ISerializer<ArchiveDescription> serializer, string name)
        {
            Serializer = serializer;
            Name = name;
        }

        public ISerializer<ArchiveDescription> Serializer { get; private set; }
        public string Name { get; private set; }

        public void Write(string archiveDirectory, ArchiveDescription result)
        {
            string path = GetArchiveInfoPath(archiveDirectory);
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(stream, result);
            }
        }

        public ArchiveDescription Read(string archiveDirectory)
        {
            string path = GetArchiveInfoPath(archiveDirectory);
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Serializer.Deserialize(stream);
            }
        }

        public string GetArchiveInfoPath(string archiveDirectory)
        {
            return Path.Combine(archiveDirectory, Name + Serializer.FileExtension);
        }
    }
}
