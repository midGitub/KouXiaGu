using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace JiongXiaGu.Unity.Resources
{


    public class ModificationFactory
    {
        private readonly List<Modification> modifications = new List<Modification>();

        internal void OnDispose(Modification modification)
        {
            modifications.Remove(modification);
        }

        /// <summary>
        /// 读取内容,若目录不存在,或者不是定义的可读内容则返回异常;
        /// </summary>
        public Modification Read(string directory)
        {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            Modification modification;
            if (!TryGet(directory, out modification))
            {
                ModificationDescription description = ReadDescription(directory);
                modification = new Modification(this, directory, description);
                modifications.Add(modification);
            }
            return modification;
        }

        /// <summary>
        /// 创建可读内容,若目录已经存在则返回异常;
        /// </summary>
        public Modification Create(string directory, ModificationDescription description)
        {
            Modification modification;
            if (!TryGet(directory, out modification))
            {
                Directory.CreateDirectory(directory);
                WriteDescription(directory, description);
                modification = new Modification(this, directory, description);
                modifications.Add(modification);
            }
            return modification;
        }

        /// <summary>
        /// 尝试获取到该目录的模组实例;
        /// </summary>
        public bool TryGet(string directory, out Modification modification)
        {
            modification = modifications.Find(item => item.Directory == directory);
            return modification != null;
        }

        public ModificationInfo ReadInfo(string directory)
        {
            ModificationDescription description = ReadDescription(directory);
            return new ModificationInfo(directory, description);
        }


        /// <summary>
        /// 从内容读取到描述,并且更新实例;
        /// </summary>
        public void UpdateDescription(Modification content)
        {
            ModificationDescription description = ReadDescription(content.BaseContent);
            content.Description = description;
        }

        /// <summary>
        /// 写入资源描述;
        /// </summary>
        public void UpdateDescription(Modification content, ModificationDescription description)
        {
            WriteDescription(content.BaseContent, description);
            content.Description = description;
        }



        private const string DescriptionFileName = "ModDescription.xml";
        private readonly XmlSerializer<ModificationDescription> descriptionSerializer = new XmlSerializer<ModificationDescription>();

        /// <summary>
        /// 输出新的描述到;
        /// </summary>
        private void WriteDescription(Content content, ModificationDescription description)
        {
            using (content.BeginUpdate())
            {
                using (var stream = content.GetOutputStream(DescriptionFileName))
                {
                    descriptionSerializer.Serialize(stream, description);
                }
            }
        }

        private ModificationDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(DescriptionFileName))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }

        private void WriteDescription(string directory, ModificationDescription description)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        private ModificationDescription ReadDescription(string directory)
        {
            string filePath = Path.Combine(directory, DescriptionFileName);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var descr = descriptionSerializer.Deserialize(stream);
                return descr;
            }
        }
    }
}
