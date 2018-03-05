using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace JiongXiaGu.Collections
{

    [TestFixture]
    public class ContrastTest
    {

        private int[] Collection => new int[]
        {
            0,
            1,
            2,
            3,
        };

        private int[] Collection_NotSame => new int[]
        {
            0,
            1,
            2,
            4,
        };

        private int[] Collection_Same => Collection;


        [Test]
        public void TestEnumerable0()
        {
            Contrast.AreSame(Collection as IEnumerable<int>, Collection_Same as IEnumerable<int>);
        }

        [Test]
        public void TestEnumerable1()
        {
            try
            {
                Contrast.AreSame(Collection as IEnumerable<int>, Collection_NotSame as IEnumerable<int>);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }



        [Test]
        public void TestCollection0()
        {
            Contrast.AreSame(Collection as ICollection<int>, Collection_Same as ICollection<int>);
        }

        [Test]
        public void TestCollection1()
        {
            try
            {
                Contrast.AreSame(Collection as ICollection<int>, Collection_NotSame as ICollection<int>);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }



        [Test]
        public void TestList0()
        {
            Contrast.AreSame(Collection as IList<int>, Collection_Same as IList<int>);
        }

        [Test]
        public void TestList1()
        {
            try
            {
                Contrast.AreSame(Collection as IList<int>, Collection_NotSame as IList<int>);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }



        private Dictionary<int, int> Dictionary => new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 1 },
            { 2, 2 },
            { 3, 3 },
        };

        private Dictionary<int, int> Dictionary_NotSame => new Dictionary<int, int>()
        {
            { 0, 0 },
            { 1, 1 },
            { 2, 3 },
            { 3, 3 },
        };

        private Dictionary<int, int> Dictionary_Same => Dictionary;


        [Test]
        public void TestDictionary0()
        {
            Contrast.AreSame(Dictionary as IDictionary<int, int>, Dictionary_Same as IDictionary<int, int>);
        }

        [Test]
        public void TestDictionary1()
        {
            try
            {
                Contrast.AreSame(Dictionary as IDictionary<int, int>, Dictionary_NotSame as IDictionary<int, int>);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }
    }
}
