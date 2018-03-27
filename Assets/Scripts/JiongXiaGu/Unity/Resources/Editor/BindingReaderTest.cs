using NUnit.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    [TestFixture]
    public class BindingReaderTest
    {
        [Test]
        public void ReadWriteTest()
        {
            BindingSerializer<File> serializer = new BindingSerializer<File>();
            MemoryContent content = new MemoryContent();
            var file = DefalutFile;

            using (content.BeginUpdate())
            {
                serializer.Serialize(content, ref file);
            }

            Assert.True(content.Contains(nameof(File.XmlTest)));
            Assert.True(content.Contains(nameof(File.ProtoTest)));
            Assert.IsFalse(content.Contains(nameof(File.IgnoreField)));

            var value = serializer.Deserialize(content) as File;
            Assert.AreEqual(file, value);
        }

        private File DefalutFile => new File();

        public class File : IEquatable<File>
        {
            [XmlAsset(nameof(XmlTest))]
            public Description XmlTest { get; set; }

            [ProtoAsset(nameof(ProtoTest))]
            public Description ProtoTest { get; set; }

            [ProtoAsset(nameof(IgnoreField))]
            internal Description IgnoreField;

            public static File Defalut => new File()
            {
                XmlTest = new Description()
                {
                    ID = "1",
                    Name = "v1",
                },

                ProtoTest = new Description()
                {
                    ID = "2",
                    Name = "v2",
                },

                IgnoreField = new Description()
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
                       XmlTest.Equals(other.XmlTest) &&
                       ProtoTest.Equals(other.ProtoTest) &&
                       IgnoreField.Equals(other.IgnoreField);
            }

            public override int GetHashCode()
            {
                var hashCode = -318127454;
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(XmlTest);
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(ProtoTest);
                hashCode = hashCode * -1521134295 + EqualityComparer<Description>.Default.GetHashCode(IgnoreField);
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

        [ProtoContract]
        public struct Description : IEquatable<Description>
        {
            [ProtoMember(1)]
            public string ID { get; set; }
            [ProtoMember(2)]
            public string Name { get; set; }
            [ProtoMember(3)]
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
