using System.IO;

namespace KouXiaGu.Resources.Archives
{
    public class ArchiveInfoSerializer
    {
        public ArchiveInfoSerializer() : this(new XmlFileSerializer<ArchiveInfo>(), "Info")
        {
        }

        public ArchiveInfoSerializer(ISerializer<ArchiveInfo> serializer, string name)
        {
            Serializer = serializer;
            Name = name;
        }

        public ISerializer<ArchiveInfo> Serializer { get; private set; }
        public string Name { get; private set; }

        public void Serialize(string archiveDirectory, ArchiveInfo result)
        {
            string path = GetArchiveInfoPath(archiveDirectory);
            using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Serializer.Serialize(result, stream);
            }
        }

        public ArchiveInfo Deserialize(string archiveDirectory)
        {
            string path = GetArchiveInfoPath(archiveDirectory);
            using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Serializer.Deserialize(stream);
            }
        }

        public string GetArchiveInfoPath(string archiveDirectory)
        {
            return Path.Combine(archiveDirectory, Name + Serializer.Extension);
        }
    }
}
