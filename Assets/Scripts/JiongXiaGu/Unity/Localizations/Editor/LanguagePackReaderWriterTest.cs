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
    class LanguagePackReaderWriterTest
    {
        [Test]
        public void Test()
        {
            var reader = new LanguagePackReader();
            LanguagePack languagePack = Create();
            Stream languagePackStream = WriteTest(reader, languagePack);
            var otherLanguagePack = ReadTest(reader, languagePackStream);
            CheckIsSame(languagePack, otherLanguagePack);
        }

        LanguagePack Create()
        {
            string name = SystemLanguage.ChineseSimplified.ToString();
            LanguageDictionary languageDictionary = new LanguageDictionary()
            {
                { "Key1", "Value1"},
                { "Key2", "Value2"},
                { "Key3", "Value3"},
            };
            var languagePack = new LanguagePack(name, name, languageDictionary);
            return languagePack;
        }

        Stream WriteTest(LanguagePackReader reader, LanguagePack languagePack)
        {
            Stream languagePackStream = new MemoryStream();
            reader.Write(languagePackStream, languagePack);
            return languagePackStream;
        }

        LanguagePack ReadTest(LanguagePackReader reader, Stream languagePackStream)
        {
            languagePackStream.Seek(0, SeekOrigin.Begin);
            LanguagePack languagePack = reader.Read(languagePackStream);
            languagePackStream.Dispose();
            return languagePack;
        }

        void CheckIsSame(LanguagePack languagePack, LanguagePack other)
        {
            Assert.AreEqual(languagePack.Name, other.Name);
            Assert.AreEqual(languagePack.Language, other.Language);
            Assert.IsTrue(languagePack.LanguageDictionary.IsSameContent(other.LanguageDictionary));
        }
    }
}
