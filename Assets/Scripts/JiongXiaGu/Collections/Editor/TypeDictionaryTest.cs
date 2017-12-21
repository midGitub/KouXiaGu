using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Collections
{

    [TestFixture]
    public class TypeDictionaryTest
    {

        [Test]
        public void TypeDictionary()
        {
            var dictionary = new TypeDictionary();
            Test(dictionary);
        }

        [Test]
        public void ConcurrentTypeDictionary()
        {
            var dictionary = new ConcurrentTypeDictionary();
            Test(dictionary);
        }

        private void Test(ITypeDictionary dictionary)
        {
            dictionary.Add(new Value(0));
            dictionary.Add(new Value1(1));
            dictionary.Add(new Value2(2));
            dictionary.Add(new Value3(3));
            Assert.AreEqual(4, dictionary.Count);

            try
            {
                dictionary.Add(new Value(1));
                Assert.Fail("应该为重复加入!");
            }
            catch (ArgumentException)
            {
            }

            try
            {
                dictionary.Add<Value>(new Value1(0));
                Assert.Fail("应该为重复加入!");
            }
            catch (ArgumentException)
            {
            }

            Assert.IsTrue(dictionary.Remove<Value>());
            Assert.IsTrue(IsFind(dictionary.Values, 1, 2, 3));

            Assert.IsTrue(dictionary.Contains<Value1>());
            Assert.IsFalse(dictionary.Contains<Value>());

            try
            {
                dictionary.Get<Value>();
                Assert.Fail("应该提示未找到!");
            }
            catch (KeyNotFoundException)
            {
            }
        }

        /// <summary>
        /// 确认 values 内,存在所有ID的内容;
        /// </summary>
        private bool IsFind(IEnumerable<object> values, params int[] IDs)
        {
            for (int i = 0; i < IDs.Length; i++)
            {
                int id = IDs[i];
                bool isFound = false;

                foreach (var value in values)
                {
                    Value item = (Value)value;
                    if (id == item.ID)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    return false;
                }
            }
            return true;
        }


        public class Value
        {
            public int ID { get; set; }

            public Value()
            {
            }

            public Value(int id)
            {
                ID = id;
            }
        }

        public class Value1 : Value
        {
            public Value1()
            {
            }

            public Value1(int id) : base(id)
            {
            }
        }

        public class Value2 : Value
        {
            public Value2()
            {
            }

            public Value2(int id) : base(id)
            {
            }
        }

        public class Value3 : Value
        {
            public Value3()
            {
            }

            public Value3(int id) : base(id)
            {
            }
        }
    }
}
