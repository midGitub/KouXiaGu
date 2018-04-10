﻿using JiongXiaGu.Unity.Resources;
using System;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{


    public class StaticCursorFactroy
    {
        internal const string ConfigFileName = "Cursor.xml";
        internal const string TextureFileName = "Cursor.png";
        private readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(StaticCursorConfig));

        public StaticCursor Read(IReadOnlyContent content)
        {
            if (content == null)
                throw new NotImplementedException();

            var config = ReadConfig(content);
            var texture = ReadTexture(content);
            return new StaticCursor(texture, config.Hotspot, config.CursorMode);
        }

        private StaticCursorConfig ReadConfig(IReadOnlyContent content)
        {
            using (var stream = content.GetInputStream(ConfigFileName))
            {
                var config = (StaticCursorConfig)xmlSerializer.Deserialize(stream);
                return config;
            }
        }

        private Texture2D ReadTexture(IReadOnlyContent content)
        {
            byte[] bytes;

            using (var stream = content.GetInputStream(TextureFileName))
            {
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
            }

            var texture = TextureFactroy.ReadAsCursor(bytes);
            return texture;
        }

        public void Write(IContent writable, StaticCursor staticCursor)
        {
            if (writable == null)
                throw new NotImplementedException();

            StaticCursorConfig config = new StaticCursorConfig()
            {
                Hotspot = staticCursor.Hotspot,
                CursorMode = staticCursor.CursorMode,
            };
            WriteConfig(writable, config);

            WriteTexture(writable, staticCursor.Texture);
        }

        private void WriteConfig(IContent writable, StaticCursorConfig config)
        {
            using (var stream = writable.GetOutputStream(ConfigFileName))
            {
                xmlSerializer.Serialize(stream, config);
            }
        }

        private void WriteTexture(IContent writable, Texture2D texture)
        {
            using (var stream = writable.GetOutputStream(TextureFileName))
            {
                TextureFactroy.WriteAsPNG(texture, stream);
            }
        }
    }
}
