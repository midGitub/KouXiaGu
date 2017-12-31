using UnityEngine;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JiongXiaGu.Unity.Resources.BindingRead
{

    [TestFixture]
    public class BindingReaderTest
    {

        [Test]
        public void ReadWriteTest()
        {
           var value = typeof(File).GetMethods(BindingFlags.Instance | BindingFlags.Public).Select(item => item.Name).ToText();
            Debug.Log(value);
        }

        public class File
        {
            [XmlAsset("v0")]
            public Description v0;

            [XmlAsset("v1")]
            public Description v1 { get; set; }

            [XmlAsset("v2")]
            public Description v2 { get; private set; }

            [XmlAsset("v3")]
            private Description v3;
        }

        public struct Description
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Tags { get; set; }
        }
    }
}
