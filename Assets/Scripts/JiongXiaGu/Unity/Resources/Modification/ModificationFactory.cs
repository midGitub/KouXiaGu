using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组创建工厂;
    /// </summary>
    public class ModificationFactory
    {
        /// <summary>
        /// 获取到模组实例(仅Unity线程调用);
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public Modification Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            ModificationDescription description = ReadDescription(directory);

            Modification modification = new Modification(directory, description);
            modification.LoadAllAssetBundles();

            return modification;
        }

        /// <summary>
        /// 异步获取到模组实例;
        /// </summary>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public async Task<Modification> ReadAsync(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            ModificationDescription description = ReadDescription(directory);

            Modification modification = new Modification(directory, description);
            var assetBundleLoadWorkTask = await UnityThread.Run(modification.LoadAllAssetBundlesAsync);
            await assetBundleLoadWorkTask;

            return modification;
        }

        /// <summary>
        /// 读取模组信息;
        /// </summary>
        public ModificationInfo ReadInfo(string directory)
        {
            ModificationDescription description = ReadDescription(directory);
            return new ModificationInfo(directory, description);
        }

        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public ModificationInfo Create(string directory, ModificationDescription description)
        {
            if (ExistDescription(directory))
                throw new IOException("该目录为模组目录;");

            Directory.CreateDirectory(directory);
            WriteDescription(directory, description);
            var modificationInfo = new ModificationInfo(directory, description);
            return modificationInfo;
        }


        public const string DescriptionFileName = "ModDescription.xml";
        private readonly XmlSerializer<ModificationDescription> descriptionSerializer = new XmlSerializer<ModificationDescription>();

        public bool ExistDescription(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            return Directory.Exists(filePath);
        }

        public void WriteDescription(Content content, ModificationDescription description)
        {
            using (content.BeginUpdateAuto())
            {
                using (var stream = content.GetOutputStream(DescriptionFileName))
                {
                    descriptionSerializer.Serialize(stream, description);
                }
            }
        }

        public ModificationDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(DescriptionFileName))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }

        public void WriteDescription(string directory, ModificationDescription description)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        public ModificationDescription ReadDescription(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }


        /// <summary>
        /// 枚举目录下所有 目录 类型的资源;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateModifications(string modsDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory, "*", searchOption))
            {
                string directoryName = Path.GetFileName(directory);
                if (!SearcheHelper.IsIgnore(directoryName))
                {
                    ModificationInfo info;

                    try
                    {
                        info = ReadInfo(directory);
                    }
                    catch
                    {
                        continue;
                    }

                    yield return info;
                }
            }
        }
    }
}
