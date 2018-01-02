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
            Assert.AreEqual(DefalutFile, value);
        }

        private File DefalutFile => new File();

        public class File : IEquatable<File>
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

            public override bool Equals(object obj)
            {
                return Equals(obj as File);
            }

            public bool Equals(File other)
            {
                return other != null &&
                       v0.Equals(other.v0) &&
                       v1.Equals(other.v1) &&
                       v2.Equals(other.v2) &&
                       v3.Equals(other.v3);
            }

            public override int GetHashCode()
            {
                var hashCode = -318127454;
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(v0);
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(v1);
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(v2);
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(v3);
                return hashCode;
            }

            public static bool operator ==(File file1, File file2)
            {
                return EqualityComparer<File>.Default.Equals(file1, file2);
            }

            public static bool operator !=(File file1, File file2)
            {
                return !(file1 == file2);
            }
        }

        public struct Description : IEquatable<Description>
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Tags { get; set; }

            public override bool Equals(object obj)
            {
                return obj is Description && Equals((Description)obj);
            }

            public bool Equals(Description other)
            {
                return ID == other.ID &&
                       Name == other.Name &&
                       Tags == other.Tags;
            }

            public override int GetHashCode()
            {
                var hashCode = -1159439338;
                hashCode = hashCode * -1521134295 + base.GetHashCode();
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ID);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Tags);
                return hashCode;
            }

            public static bool operator ==(Description description1, Description description2)
            {
                return description1.Equals(description2);
            }

            public static bool operator !=(Description description1, Description description2)
            {
                return !(description1 == description2);
            }
        }
    }
}
