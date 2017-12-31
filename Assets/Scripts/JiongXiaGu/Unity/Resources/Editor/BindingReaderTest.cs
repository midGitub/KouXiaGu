using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.Binding
{

    [TestFixture]
    public class BindingReaderTest
    {
        [Test]
        public void ReadWriteTest()
        {
            BindingSerializer reader = new BindingSerializer(typeof(File));
            MemoryContent content = new MemoryContent();

            reader.Serialize(content, DefalutFile);
            Assert.True(content.Contains("v0"));
            Assert.True(content.Contains("v1"));
            Assert.True(content.Contains("v2"));

            var value = reader.Deserialize(content) as File;
            AreEqual(DefalutFile, value);
        }

        private void AreEqual(File v1, File v2)
        {
            AreEqual(v1.v0, v2.v0);
            AreEqual(v1.v1, v2.v1);
            AreEqual(v1.v2, v2.v2);
        }

        private void AreEqual(Description v1, Description v2)
        {
            Assert.AreEqual(v1.ID, v2.ID);
            Assert.AreEqual(v1.Name, v2.Name);
            Assert.AreEqual(v1.Tags, v2.Tags);
        }

        private File DefalutFile => new File();

        public class File
        {
            [XmlAsset("v0", false)]
            public Description v0;

            [XmlAsset("v1", false)]
            public Description v1 { get; set; }

            [XmlAsset("v2", false)]
            public Description v2 { get; private set; }

            [XmlAsset("v3", false)]
            private Description v3;

            public static File Defalut => new File()
            {
                v0 = new Description()
                {
                    ID = "0",
                    Name = "v0",
                },

                v1 = new Description()
                {
                    ID = "1",
                    Name = "v1",
                },

                v2 = new Description()
                {
                    ID = "2",
                    Name = "v2",
                },

                v3 = new Description()
                {
                    ID = "3",
                    Name = "v3",
                },
            };
        }

        public struct Description
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Tags { get; set; }
        }
    }
}
