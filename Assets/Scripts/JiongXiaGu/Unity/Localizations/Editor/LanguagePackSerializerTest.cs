using JiongXiaGu.Collections;
using NUnit.Framework;
using System.IO;
using UnityEngine;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 语言包读写测试;
    /// </summary>
    [TestFixture]
    public class LanguagePackSerializerTest
    {

        private LanguagePackDescription description = new LanguagePackDescription()
        {
            Name = "Unknow",
            Language = "Chinese",
        };

        private LanguageDictionary languageDictionary = new LanguageDictionary()
        {
            { "Key1", "Value1"},
            { "Key2", "Value2"},
            { "Key3", "Value3"},
        };

        [Test]
        public void SerializeInMemory()
        {
            var reader = new LanguagePackSerializer();
            LanguagePack pack1 = CreatePack();
            Stream stream = new MemoryStream();
            reader.Serialize(stream, pack1);
            stream.Seek(0, SeekOrigin.Begin);
            var pack2 = reader.Deserialize(stream);

            CheckIsSame(pack1.Description, pack2.Description);
            CheckIsSame(pack1.LanguageDictionary, pack2.LanguageDictionary);
        }

        [Test]
        public void SerializeInFile()
        {
            var reader = new LanguagePackSerializer();
            LanguagePack pack1 = CreatePack();
            LanguagePack pack2;
            string filePath = Path.Combine(GetTempDirectory(), "ReadWritTest.zip");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                reader.Serialize(stream, pack1);
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                pack2 = reader.Deserialize(stream);
            }

            CheckIsSame(pack1.Description, pack2.Description);
            CheckIsSame(pack1.LanguageDictionary, pack2.LanguageDictionary);
        }

        [Test]
        public void SerializeDescInFile()
        {
            LanguagePackDescription desc;
            var reader = new LanguagePackSerializer();
            var pack1 = CreatePack();
            string filePath = Path.Combine(GetTempDirectory(), "ReadDescriptionTest.zip");

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                reader.Serialize(stream, pack1);
            }

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                desc = reader.DeserializeDesc(stream);
            }

            CheckIsSame(pack1.Description, desc);
        }

        private string GetTempDirectory()
        {
            string directory = Path.Combine(NUnit.TempDirectory, "Localizations");
            Directory.CreateDirectory(directory);
            return directory;
        }

        private LanguagePack CreatePack()
        {
            var languagePack = new LanguagePack(description, languageDictionary);
            return languagePack;
        }

        private void CheckIsSame(LanguagePackDescription desc1, LanguagePackDescription desc2)
        {
            Assert.AreEqual(desc1.Name, desc2.Name);
            Assert.AreEqual(desc1.Language, desc2.Language);
        }

        private void CheckIsSame(LanguageDictionary dictionary1, LanguageDictionary dictionary2)
        {
            Assert.IsTrue(dictionary1.IsSame(dictionary2));
        }
    }
}
