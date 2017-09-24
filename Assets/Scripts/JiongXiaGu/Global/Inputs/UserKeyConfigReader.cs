using JiongXiaGu.Resources;
using System.IO;

namespace JiongXiaGu.Inputs
{
    /// <summary>
    /// 用户定义的按键输入文件序列化;
    /// </summary>
    public class UserKeyConfigReader : FileReader<CustomKey2[]>
    {
        public const string UserKeyConfigFileName = "Configs/KeyboardInput";

        public UserKeyConfigReader() : base(new XmlFileSerializer<CustomKey2[]>())
        {
        }

        public override string GetFilePath()
        {
            string path = Path.Combine(Resource.UserConfigDirectoryPath, UserKeyConfigFileName + FileExtension);
            return path;
        }
    }
}
