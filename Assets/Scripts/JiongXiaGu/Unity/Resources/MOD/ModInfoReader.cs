using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组信息读写器;
    /// </summary>
    public class ModInfoReader
    {
        private const string DescriptionFileName = "Desc";
        private readonly XmlSerializer<ModDescription> xmlSerializer;

        public ModInfoReader()
        {
            xmlSerializer = new XmlSerializer<ModDescription>();
        }

        /// <summary>
        /// 创建模组;
        /// </summary>
        public ModInfo Create(string modDirectory, ModDescription description)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取模组;
        /// </summary>
        public ModInfo Read(string modDirectory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将模组内容输出磁盘或更新磁盘的内容;
        /// </summary>
        public void Write(ModInfo modInfo)
        {
            string filePath = Path.Combine(modInfo.DirectoryInfo.FullName, DescriptionFileName);
            filePath = Path.ChangeExtension(filePath, xmlSerializer.FileExtension);
            xmlSerializer.Write(filePath, modInfo.Description, FileMode.Create);
        }
    }
}
