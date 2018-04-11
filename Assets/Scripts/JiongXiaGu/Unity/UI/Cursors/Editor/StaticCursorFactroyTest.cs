using JiongXiaGu.Unity.Resources;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    [TestFixture]
    public class StaticCursorFactroyTest
    {
        private StaticCursorConfig defaultConfig = new StaticCursorConfig()
        {
            Hotspot = new Vector2(0, 0),
            CursorMode = CursorMode.Auto,
        };

        private Texture2D defaultTexture = new Texture2D(1, 1);

        [Test]
        public void Test()
        {
            var factroy = new StaticCursorFactroy();
            var memoryContent = new MemoryContent();
            var defaultCursor = new StaticCursor(defaultTexture, defaultConfig);

            using (memoryContent.BeginUpdateAuto())
            {
                factroy.Write(memoryContent, defaultCursor);
            }

            var cursor2 = factroy.Read(memoryContent);

            Assert.AreEqual(defaultConfig, cursor2.Config);
            Assert.AreEqual(defaultTexture.width, cursor2.Texture.width);
            Assert.AreEqual(defaultTexture.height, cursor2.Texture.height);
        }
    }
}
