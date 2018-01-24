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
        private readonly string rootDirectory;

        public LanguagePackSerializerTest()
        {
            rootDirectory = Path.Combine(NUnit.TempDirectory, nameof(LanguagePackSerializerTest));
            if (Directory.Exists(rootDirectory))
            {
                Directory.Delete(rootDirectory, true);
            }
            Directory.CreateDirectory(rootDirectory);
        }

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

        private LanguageSplitDictionary languageSplitDictionary = new LanguageSplitDictionary()
        {
            { "V1", "V1_Key1", "Value1"},
            { "V1", "V1_Key2", "Value2"},
            { "V1", "V1_Key3", "Value3"},

            { "V2", "V2_Key1", "Value1"},
            { "V2", "V2_Key2", "Value2"},
            { "V2", "V2_Key3", "Value3"},
        };

        [Test]
        public void DictionaryTest()
        {
            LanguagePack pack = new LanguagePack(description, languageDictionary);

            TestReadWriteInMemory(pack);
            TestReadWriteInFile(pack, "Dictionary.zip");

        }

        [Test]
        public void SplitDictionaryTest()
        {
            LanguagePack pack = new LanguagePack(description, languageSplitDictionary);

            TestReadWriteInMemory(pack);
            TestReadWriteInFile(pack, "SplitDictionary.zip");
        }

        private void TestReadWriteInMemory(LanguagePack pack1)
        {
            using (Stream stream = new MemoryStream())
            {
                ReadWriteTest(stream, pack1);
            }
        }

        private void TestReadInFile(LanguagePack pack1, string fileName)
        {
            string filePath = Path.Combine(rootDirectory, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Read))
            {
                ReadTest(stream, pack1);
            }
        }

        private void TestReadWriteInFile(LanguagePack pack1, string fileName)
        {
            string filePath = Path.Combine(rootDirectory, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                ReadWriteTest(stream, pack1);
            }
        }

        private void ReadWriteTest(Stream stream, LanguagePack pack1)
        {
            var serializer = new LanguagePackSerializer();

            serializer.Serialize(stream, pack1);

            stream.Seek(0, SeekOrigin.Begin);
            ReadTest(stream, pack1);
        }

        private void ReadTest(Stream stream, LanguagePack pack1)
        {
            var serializer = new LanguagePackSerializer();

            var pack2 = serializer.DeserializePack(stream);

            CheckIsSame(pack1.Description, pack2.Description);
            CheckIsSame(pack1.LanguageDictionary, pack2.LanguageDictionary);
        }

        private void CheckIsSame(LanguagePack pack1, LanguagePack pack2)
        {
            CheckIsSame(pack1.Description, pack1.Description);
            CheckIsSame(pack1.LanguageDictionary, pack1.LanguageDictionary);
        }

        private void CheckIsSame(LanguagePackDescription desc1, LanguagePackDescription desc2)
        {
            Assert.AreEqual(desc1.Name, desc2.Name);
            Assert.AreEqual(desc1.Language, desc2.Language);
        }

        private void CheckIsSame(ILanguageDictionary dictionary1, ILanguageDictionary dictionary2)
        {
            Assert.IsTrue(dictionary1.IsSame(dictionary2));
        }


        [Test]
        public void TestReadInMemory()
        {
            var serializer = new LanguagePackSerializer();
            LanguagePack dictionary = new LanguagePack(description, languageDictionary);
            LanguagePack splitDictionary = new LanguagePack(description, languageSplitDictionary);

            using (Stream stream = new MemoryStream())
            {
                serializer.Serialize(stream, dictionary);

                stream.Seek(0, SeekOrigin.Begin);
                var pack = serializer.DeserializeSplit(stream);

                CheckIsSame(dictionary, pack);
            }

            using (Stream stream = new MemoryStream())
            {
                serializer.Serialize(stream, splitDictionary);

                stream.Seek(0, SeekOrigin.Begin);
                var pack = serializer.DeserializePack(stream);

                CheckIsSame(splitDictionary, pack);
            }
        }



        //public void SerializeInMemory()
        //{
        //    var reader = new LanguagePackSerializer();
        //    LanguagePack pack1 = CreatePack();
        //    Stream stream = new MemoryStream();
        //    reader.Serialize(stream, pack1);
        //    stream.Seek(0, SeekOrigin.Begin);
        //    var pack2 = reader.DeserializePack(stream);

        //    CheckIsSame(pack1.Description, pack2.Description);
        //    CheckIsSame(pack1.LanguageDictionary, pack2.LanguageDictionary);
        //}

        //public void SerializeInFile()
        //{
        //    var reader = new LanguagePackSerializer();
        //    LanguagePack pack1 = CreatePack();
        //    LanguagePack pack2;
        //    string filePath = Path.Combine(GetTempDirectory(), "ReadWritTest.zip");

        //    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //    {
        //        reader.Serialize(stream, pack1);
        //    }

        //    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        pack2 = reader.DeserializePack(stream);
        //    }

        //    CheckIsSame(pack1.Description, pack2.Description);
        //    CheckIsSame(pack1.LanguageDictionary, pack2.LanguageDictionary);
        //}

        //public void SerializeDescInFile()
        //{
        //    LanguagePackDescription desc;
        //    var reader = new LanguagePackSerializer();
        //    var pack1 = CreatePack();
        //    string filePath = Path.Combine(GetTempDirectory(), "ReadDescriptionTest.zip");

        //    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        //    {
        //        reader.Serialize(stream, pack1);
        //    }

        //    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {
        //        desc = reader.Deserialize(stream);
        //    }

        //    CheckIsSame(pack1.Description, desc);
        //}

        //private string GetTempDirectory()
        //{
        //    string directory = Path.Combine(NUnit.TempDirectory, "Localizations");
        //    Directory.CreateDirectory(directory);
        //    return directory;
        //}

        //private LanguagePack CreatePack()
        //{
        //    var languagePack = new LanguagePack(description, languageDictionary);
        //    return languagePack;
        //}
    }
}
