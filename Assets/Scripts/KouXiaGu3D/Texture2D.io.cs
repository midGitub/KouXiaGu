using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu3D
{


    /// <summary>
    /// 对 Texture2D 保存的拓展方法;
    /// </summary>
    public static class Texture2DExpand
    {

        const string ExtensionPNG = ".png";
        const string ExtensionJPG = ".jpg";

        /// <summary>
        /// 保存为PNG格式,并指定 filePath 后缀为 .png
        /// </summary>
        public static void SavePNG(this Texture2D texture, string filePath, FileMode mode)
        {
            if (texture == null)
                throw new NullReferenceException();

            filePath = Path.ChangeExtension(filePath, ExtensionPNG);
            byte[] data = texture.EncodeToPNG();
            SaveBinary(data, filePath, mode);
        }

        /// <summary>
        /// 保存为JPG格式,并指定 filePath 后缀为 .jpg
        /// </summary>
        public static void SaveJPG(this Texture2D texture, string filePath, FileMode mode)
        {
            if (texture == null)
                throw new NullReferenceException();

            filePath = Path.ChangeExtension(filePath, ExtensionJPG);
            byte[] data = texture.EncodeToJPG();
            SaveBinary(data, filePath, mode);
        }

        /// <summary>
        /// 以二进制形式保存到;
        /// </summary>
        static void SaveBinary(byte[] data, string filePath, FileMode mode)
        {
            using (FileStream file = File.Open(filePath, mode))
            {
                using (BinaryWriter binary = new BinaryWriter(file))
                {
                    binary.Write(data);
                }
            }
        }

        /// <summary>
        /// 保存到这个目录并且按 现在时间 的 记号(DateTime.Ticks) 命名;
        /// </summary>
        public static void SavePNG(this Texture2D texture, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, DateTime.Now.Ticks.ToString());
            SavePNG(texture, filePath, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// 保存到这个目录并且按 现在时间 的 记号(DateTime.Ticks) 命名;
        /// </summary>
        public static void SaveJPG(this Texture2D texture, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, DateTime.Now.Ticks.ToString());
            SaveJPG(texture, filePath, FileMode.OpenOrCreate);
        }

    }

}
