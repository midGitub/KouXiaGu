using System;
using System.IO;
using UnityEngine;

namespace JiongXiaGu
{
    /// <summary>
    /// Texture 拓展方法;
    /// </summary>
    public static class TextureFactroy
    {

        #region Read

        /// <summary>
        /// 读取贴图;
        /// </summary>
        public static Texture2D Read(byte[] data, TextureFormat format, bool mipmap)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Texture2D texture = new Texture2D(0, 0, format, mipmap);
            if (ImageConversion.LoadImage(texture, data))
            {
                return texture;
            }
            else
            {
                GameObject.Destroy(texture);
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 读取贴图为光标类型;
        /// </summary>
        public static Texture2D ReadAsCursor(byte[] data)
        {
            var texture = Read(data, TextureFormat.ARGB32, false);
#if UNITY_EDITOR
            texture.alphaIsTransparency = true;
#endif
            return texture;
        }

        #endregion

        #region Write

        public static void WriteAsPNG(this Texture2D texture, Stream stream)
        {
            byte[] data = texture.EncodeToPNG();
            stream.Write(data, 0, data.Length);
        }

        public static void WriteAsJPG(this Texture2D texture, Stream stream)
        {
            byte[] data = texture.EncodeToJPG();
            stream.Write(data, 0, data.Length);
        }





        public const string PNGExtension = ".png";
        public const string JPGExtension = ".jpg";

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
            filePath = Path.ChangeExtension(filePath, PNGExtension);
            byte[] data = texture.EncodeToPNG();
            WriteBinary(data, filePath, mode);
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
            filePath = Path.ChangeExtension(filePath, JPGExtension);
            byte[] data = texture.EncodeToJPG();
            WriteBinary(data, filePath, mode);
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


        private static byte[] ReadBinary(string filePath, FileMode mode)
        {
            using (FileStream stream = File.Open(filePath, mode))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return reader.ReadBytes((int)stream.Length);
                }
            }
        }

        /// <summary>
        /// 以二进制形式输出到;
        /// </summary>
        private static void WriteBinary(byte[] data, string filePath, FileMode mode)
        {
            using (FileStream stream = File.Open(filePath, mode))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(data);
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

        #endregion
    }
}
