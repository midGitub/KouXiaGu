using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;

namespace JiongXiaGu.Unity.Archives
{


    public class ArchiveFatroy
    {
        private const string DescriptionFileName = "ArchiveDescription.xml";
        private readonly XmlSerializer<ArchiveDescription> descriptionSerializer = new XmlSerializer<ArchiveDescription>();

        /// <summary>
        /// 枚举目录下所有存档;
        /// </summary>
        public IEnumerable<Archive> EnumerateArchives(string archivesDirectory)
        {
            foreach (var directory in Directory.EnumerateDirectories(archivesDirectory, "*", SearchOption.TopDirectoryOnly))
            {
                Archive archive;
                try
                {
                    archive = Create(directory);
                }
                catch
                {
                    archive = null;
                    continue;
                }
                yield return archive;
            }
        }

        public Archive Create(string archiveDirectory)
        {
            DirectoryContent directoryContent = new DirectoryContent(archiveDirectory);
            ArchiveDescription description = ReadDescription(directoryContent);
            return new Archive(directoryContent, description);
        }

        /// <summary>
        /// 输出新的描述到;
        /// </summary>
        private void WriteDescription(Content content, ArchiveDescription description)
        {
            using (content.BeginUpdate())
            {
                using (var stream = content.GetOutputStream(DescriptionFileName))
                {
                    descriptionSerializer.Serialize(stream, description);
                }
            }
        }

        /// <summary>
        /// 读取到描述;
        /// </summary>
        private ArchiveDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(DescriptionFileName))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
