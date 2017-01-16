using System;
using System.IO;
using UnityEngine;

namespace KouXiaGu
{


    /// <summary>
    /// 对 Texture2D 保存的拓展方法;
    /// </summary>
    public static class Texture2D_IO_Extensions
    {

        const string ExtensionPNG = ".png";
        const string ExtensionJPG = ".jpg";


        /// <summary>
        /// 保存为PNG格式,并指定 filePath 后缀为 .png
        /// </summary>
        public static void SavePNG(this Texture2D texture, string directoryPath, string fileName, FileMode mode)
        {
            string filePath = Path.Combine(directoryPath, fileName);
            SavePNG(texture, filePath, mode);
        }

        /// <summary>
        /// 保存为PNG格式,并指定 filePath 后缀为 .png
        /// </summary>
        public static void SavePNG(this Texture2D texture, string filePath, FileMode mode)
        {
            filePath = Path.ChangeExtension(filePath, ExtensionPNG);
            byte[] data = texture.EncodeToPNG();
            SaveBinary(data, filePath, mode);
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
            SavePNG(texture, filePath, FileMode.Create);
        }


        /// <summary>
        /// 保存为PNG格式,并指定 filePath 后缀为 .png
        /// </summary>
        public static void SavePNG(this RenderTexture renderTexture, string directoryPath, string fileName, FileMode mode)
        {
            string filePath = Path.Combine(directoryPath, fileName);
            SavePNG(renderTexture, filePath, mode);
        }

        /// <summary>
        /// 保存为PNG格式,并指定 filePath 后缀为 .png
        /// </summary>
        public static void SavePNG(this RenderTexture renderTexture, string filePath, FileMode mode)
        {
            Texture2D texture = renderTexture.GetTexture2D();
            SavePNG(texture, filePath, mode);
        }

        /// <summary>
        /// 保存到这个目录并且按 现在时间 的 记号(DateTime.Ticks) 命名;
        /// </summary>
        public static void SavePNG(this RenderTexture renderTexture, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, DateTime.Now.Ticks.ToString());
            SavePNG(renderTexture, filePath, FileMode.Create);
        }



        /// <summary>
        /// 保存为JPG格式,并指定 filePath 后缀为 .jpg
        /// </summary>
        public static void SaveJPG(this Texture2D texture, string directoryPath, string fileName, FileMode mode)
        {
            string filePath = Path.Combine(directoryPath, fileName);
            SaveJPG(texture, filePath, mode);
        }

        /// <summary>
        /// 保存为JPG格式,并指定 filePath 后缀为 .jpg
        /// </summary>
        public static void SaveJPG(this Texture2D texture, string filePath, FileMode mode)
        {
            filePath = Path.ChangeExtension(filePath, ExtensionJPG);
            byte[] data = texture.EncodeToJPG();
            SaveBinary(data, filePath, mode);
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
            SaveJPG(texture, filePath, FileMode.Create);
        }


        /// <summary>
        /// 保存为JPG格式,并指定 filePath 后缀为 .jpg
        /// </summary>
        public static void SaveJPG(this RenderTexture renderTexture, string filePath, FileMode mode)
        {
            Texture2D texture = renderTexture.GetTexture2D();
            SaveJPG(texture, filePath, mode);
        }

        /// <summary>
        /// 保存到这个目录并且按 现在时间 的 记号(DateTime.Ticks) 命名;
        /// </summary>
        public static void SaveJPG(this RenderTexture renderTexture, string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filePath = Path.Combine(directoryPath, DateTime.Now.Ticks.ToString());
            SaveJPG(renderTexture, filePath, FileMode.Create);
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

        public static Texture2D GetTexture2D(this RenderTexture renderTexture)
        {
            RenderTexture currentActiveRT = RenderTexture.active;

            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
            texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = currentActiveRT;
            return texture;
        }


    }

}
