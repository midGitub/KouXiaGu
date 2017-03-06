using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace KouXiaGu.World
{

    [TestFixture]
    class DynamicResourceTest
    {

        [Test]
        public void TestRedistribute1()
        {
            DynamicResource res = new DynamicResource(30);

            var v1 = res.GetResource(0, 2);
            var v2 = res.GetResource(0, 7);
            var v3 = res.GetResource(1, 20);
            var v4 = res.GetResource(1, 20);

            Assert.AreEqual(res.Demand, 49);

            Assert.AreEqual(v1.Practice, 0);
            Assert.AreEqual(v2.Practice, 0);
            Assert.AreEqual(v3.Practice, 15);
            Assert.AreEqual(v4.Practice, 15);

            v1.Dispose();
            v3.SetDemand(10);

            Assert.AreEqual(v1.Practice, 0);
            Assert.AreEqual(v2.Practice, 0);
            Assert.AreEqual(v3.Practice, 10);
            Assert.AreEqual(v4.Practice, 20);
        }


        [Test]
        public void TestRedistribute2()
        {
            DynamicResource res = new DynamicResource(20);

            var v1 = res.GetResource(1, 5);
            var v2 = res.GetResource(0, 10);

            Assert.AreEqual(res.Demand, 15);

            Assert.AreEqual(v1.Practice, 5);
            Assert.AreEqual(v2.Practice, 10);

            v1.SetDemand(20);
            Assert.AreEqual(v1.Practice, 20);
            Assert.AreEqual(v2.Practice, 0);

            v1.Dispose();
            Assert.AreEqual(v1.Practice, 0);
            Assert.AreEqual(v2.Practice, 10);
        }


        [Test]
        public void TestRedistribute3()
        {
            DynamicResource res = new DynamicResource(20);

            var v1 = res.GetResource(1, 5);
            var v2 = res.GetResource(0, 10);

            res.SetTotal(10);

            Assert.AreEqual(v1.Practice, 5);
            Assert.AreEqual(v2.Practice, 5);
        }


        [Test]
        public void TestRedistribute4()
        {
            DynamicResource res = new DynamicResource(20);

            var v1 = res.GetResource(1, 5);
            var v2 = res.GetResource(0, 10);

            res.SetTotal(14);

            Assert.AreEqual(v1.Practice, 5);
            Assert.AreEqual(v2.Practice, 9);
        }

    }
}
