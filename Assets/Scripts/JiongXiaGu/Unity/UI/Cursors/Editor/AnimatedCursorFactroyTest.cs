using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Resources;
using UnityEngine;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.UI.Cursors.Editor
{

    [TestFixture]
    public class AnimatedCursorFactroyTest
    {
        private AnimatedCursorConfig defaultConfig = new AnimatedCursorConfig()
        {
            IsLoop = true,
            hotspot = new Vector2(0, 0),
            Speed = 2,
            cursorMode = CursorMode.Auto,
        };

        private Texture2D[] defaultTextures = new Texture2D[]
        {
            new Texture2D(1, 1),
            new Texture2D(2, 2),
            new Texture2D(3, 3),
        };

        [Test]
        public void Test()
        {
            var factroy = new AnimatedCursorFactroy();
            var memoryContent = new MemoryContent();
            var defaultCursor = new AnimatedCursor(defaultConfig, defaultTextures);

            using (memoryContent.BeginUpdateAuto())
            {
                factroy.Write(memoryContent, defaultCursor);
            }

            var cursor2 = factroy.Read(memoryContent);

            Assert.AreEqual(defaultConfig, cursor2.Config);
            AreEqual(defaultTextures, cursor2.Textures);
        }

        private void AreEqual(IReadOnlyList<Texture2D> t1, IReadOnlyList<Texture2D> t2)
        {
            Contrast.AreSame(t1, t2, (v1, v2) => v1.width == v2.width && v1.height == v2.height);
        }
    }
}
