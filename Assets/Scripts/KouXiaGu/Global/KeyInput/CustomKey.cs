using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 按键读取;
    /// </summary>
    public abstract class CustomKeyReader
    {
        const string CustomKeyFileName = "Input\\Keyboard.xml";

        static string CustomKeyFilePath
        {
            get { return ResourcePath.CombineConfiguration(CustomKeyFileName); }
        }

        public abstract IEnumerable<CustomKey> ReadKeys(string filePath);
        public abstract void WriteKeys(IEnumerable<CustomKey> keys, string filePath);

        public virtual IEnumerable<CustomKey> ReadKeys()
        {
            return ReadKeys(CustomKeyFilePath);
        }

        public virtual void WriteKeys(IEnumerable<CustomKey> keys)
        {
            WriteKeys(keys, CustomKeyFilePath);
        }
    }

    /// <summary>
    /// 读取默认按键;
    /// </summary>
    public class DefaultCustomKeyReader : CustomKeyReader
    {
        const string DefaultCustomKeyFileName = "Input\\Default_Keyboard.xml";

        static string DefaultCustomKeyFilePath
        {
            get { return ResourcePath.CombineConfiguration(DefaultCustomKeyFileName); }
        }

        public DefaultCustomKeyReader(CustomKeyReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            this.reader = reader;
        }

        CustomKeyReader reader;

        public override IEnumerable<CustomKey> ReadKeys()
        {
            return ReadKeys(DefaultCustomKeyFilePath);
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys)
        {
            WriteKeys(keys, DefaultCustomKeyFilePath);
        }

        public override IEnumerable<CustomKey> ReadKeys(string filePath)
        {
            return reader.ReadKeys(filePath);
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys, string filePath)
        {
            reader.WriteKeys(keys, filePath);
        }
    }

    /// <summary>
    /// 读取Xml文件按键信息;
    /// </summary>
    public class XmlCustomKeyReader : CustomKeyReader
    {
        static readonly XmlSerializer keyArraySerializer = new XmlSerializer(typeof(CustomKey[]));

        public XmlCustomKeyReader()
        {
        }

        public override IEnumerable<CustomKey> ReadKeys(string filePath)
        {
            var customKeys = (CustomKey[])keyArraySerializer.DeserializeXiaGu(filePath);
            return customKeys;
        }

        public override void WriteKeys(IEnumerable<CustomKey> keys, string filePath)
        {
            var keyArray = keys as CustomKey[] ?? keys.ToArray();
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            keyArraySerializer.SerializeXiaGu(filePath, keyArray, FileMode.Create);
        }

    }

    /// <summary>
    /// 自定义按键 和 UnityEngine.KeyCode 的映射;
    /// </summary>
    [XmlType("Key")]
    public struct CustomKey : IEquatable<CustomKey>
    {
        public CustomKey(KeyFunction function, KeyCode keyCode)
        {
            this.function = function;
            this.keyCode = keyCode;
        }

        [XmlAttribute("function")]
        public KeyFunction function { get; private set; }

        [XmlAttribute("key")]
        public KeyCode keyCode { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is CustomKey))
                return false;
            return Equals((CustomKey)obj);
        }

        public bool Equals(CustomKey other)
        {
            return this.function == other.function &&
                this.keyCode == other.keyCode;
        }

        public override int GetHashCode()
        {
            return ((int)function) ^ ((int)keyCode);
        }

        public override string ToString()
        {
            return "Function:" + function.ToString() + ";Key:" + keyCode.ToString();
        }

    }

}
