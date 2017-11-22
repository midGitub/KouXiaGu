using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace JiongXiaGu.Collections
{

    [TestFixture]
    public class DoublyLinkedListTest
    {
        private const string value0 = "value0";
        private const string value1 = "value1";
        private const string value2 = "value2";
        private const string value3 = "value3";

        private DoublyLinkedList<string> Create()
        {
            DoublyLinkedList<string> list = new DoublyLinkedList<string>()
            {
                value0, value1, value2
            };
            return list;
        }

        private void TestEnumerable(DoublyLinkedList<string> list, params string[] values)
        {
            int index = 0;
            foreach (var item in list)
            {
                if (index >= values.Length)
                {
                    throw new InvalidOperationException("超出元素数目;");
                }
                else
                {
                    string value = values[index];
                    Assert.AreEqual(value, item);
                }
                index++;
            }
        }

        [Test]
        public void TestAddLast()
        {
            DoublyLinkedList<string> list = Create();

            list.AddLast(value3);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value3);
            TestEnumerable(list, value0, value1, value2, value3);
        }

        [Test]
        public void TestAddFirst()
        {
            DoublyLinkedList<string> list = Create();

            list.AddFirst(value3);
            Assert.AreEqual(4, list.Count);
            Assert.AreEqual(list.First.Value, value3);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value3, value0, value1, value2);
        }

        [Test]
        public void TestRemove()
        {
            DoublyLinkedList<string> list = Create();

            list.Remove(list.First);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list.First.Value, value1);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value1, value2);
        }

        [Test]
        public void TestEnumerable0()
        {
            DoublyLinkedList<string> list = Create();
            TestEnumerable(list, value0, value1, value2);
        }

        [Test]
        public void TestEnumerable1()
        {
            DoublyLinkedList<string> list = Create();
            try
            {
                foreach (var item in list)
                {
                    list.Add(value0);
                }
                Assert.Fail("未抛出合集变更异常;");
            }
            catch (InvalidOperationException)
            {
            }
        }

        [Test]
        public void Test1()
        {
            DoublyLinkedList<string> list = Create();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value0, value1, value2);

            list.Remove(list.First);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list.First.Value, value1);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value1, value2);

            list.AddFirst(value0);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value0, value1, value2);

            list.RemoveAfterNodes(list.First);
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(list.First, list.Last);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value0);
            TestEnumerable(list, value0);

            var node0 = list.First;
            var node1 = list.AddAfter(node0, value2);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value0, value2);

            node0 = list.AddBefore(node1, value1);
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual(list.First.Value, value0);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value0, value1, value2);

            list.RemoveBeforNodes(node0);
            Assert.AreEqual(2, list.Count);
            Assert.AreEqual(list.First.Value, value1);
            Assert.AreEqual(list.Last.Value, value2);
            TestEnumerable(list, value1, value2);
        }
    }
}
