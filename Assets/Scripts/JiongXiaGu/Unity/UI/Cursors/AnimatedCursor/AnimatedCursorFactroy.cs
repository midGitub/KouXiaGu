using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{


    public class AnimatedCursorFactroy
    {
        internal const string ConfigFileName = "AnimatedCursor.xml";
        internal const string TextureExtensionName = ".png";
        private readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(AnimatedCursorConfig));
        private readonly SortedList<int, Texture2D> sortedList = new SortedList<int, Texture2D>();

        /// <summary>
        /// 从资源合集读取到动画光标;
        /// </summary>
        public AnimatedCursor Read(IReadOnlyContent content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            AnimatedCursorConfig config = ReadConfig(content);
            IEnumerable<Texture2D> textures = ReadTextures(content);
            return new AnimatedCursor(config, textures);
        }

        /// <summary>
        /// 读取到配置信息;
        /// </summary>
        private AnimatedCursorConfig ReadConfig(IReadOnlyContent content)
        {
            using (var stream = content.GetInputStream(ConfigFileName))
            {
                var config = (AnimatedCursorConfig)xmlSerializer.Deserialize(stream);
                return config;
            }
        }

        private IEnumerable<Texture2D> ReadTextures(IReadOnlyContent content)
        {
            sortedList.Clear();
            ReadTextures(content, sortedList);
            return sortedList.Values;
        }

        private void ReadTextures(IReadOnlyContent content, SortedList<int, Texture2D> sortedList)
        {
            byte[] bytes;

            foreach (var textureEntry in content.EnumerateEntries("*" + TextureExtensionName, SearchOption.TopDirectoryOnly))
            {
                int number = GetNumberByName(textureEntry.Name);
                using (var stream = content.GetInputStream(textureEntry))
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                }

                var texture = TextureFactroy.ReadAsCursor(bytes);
                sortedList.Add(number, texture);
            }
        }

        private int GetNumberByName(string name)
        {
            string numberStr;
            int startIndex = name.LastIndexOf('_');

            if (startIndex < 0)
            {
                numberStr = name.Substring(0, name.Length - TextureExtensionName.Length);
            }
            else
            {
                numberStr = name.Substring(startIndex, name.Length - TextureExtensionName.Length);
            }

            int number = Convert.ToInt32(numberStr);
            return number;
        }


        public void Write(IContent writable, AnimatedCursor animatedCursor)
        {
            if (writable == null)
                throw new ArgumentNullException(nameof(writable));
            if (animatedCursor == null)
                throw new ArgumentNullException(nameof(animatedCursor));

            WriteConfig(writable, animatedCursor.Config);
            WriteTextures(writable, animatedCursor.Textures);
        }

        private void WriteConfig(IContent writable, AnimatedCursorConfig config)
        {
            using (var stream = writable.GetOutputStream(ConfigFileName))
            {
                xmlSerializer.Serialize(stream, config);
            }
        }

        private void WriteTextures(IContent writable, IEnumerable<Texture2D> textures)
        {
            int index = 0;
            foreach (var texture in textures)
            {
                using (var stream = writable.GetOutputStream(index.ToString() + TextureExtensionName))
                {
                    TextureFactroy.WriteAsPNG(texture, stream);
                }
                index++;
            }
        }
    }
}
