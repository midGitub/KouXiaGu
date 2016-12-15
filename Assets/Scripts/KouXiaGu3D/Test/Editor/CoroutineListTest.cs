using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using NUnit.Framework;

namespace KouXiaGu.Test
{

    [TestFixture]
    public class CoroutineListTest
    {
        readonly Unit[] unitArray = new Unit[]
        {
            new Unit(0),
            new Unit(1),
            new Unit(2),
            new Unit(3),
            new Unit(4),
        };

        [Test]
        public void GetCompletes()
        {
            CoroutineList<Unit> list = GetCoroutineList();

            list.MoveNext();
            list.MoveNext();
            var completes = list.GetCompletes();
            Assert.AreEqual(2, completes.Length);

            int index = 0;

            foreach (var item in completes)
            {
                Assert.AreEqual(item.Key.ID, index++);
            }
        }

        [Test]
        public void GetWaits()
        {
            CoroutineList<Unit> list = GetCoroutineList();

            list.MoveNext();
            var waits = list.GetWaitsAndCurrent();
            Assert.AreEqual(unitArray.Length - 1, waits.Length);

            int index = 1;

            foreach (var item in waits)
            {
                Assert.AreEqual(item.Key.ID, index++);
            }
        }

        [Test]
        public void RemoveAll()
        {
            CoroutineList<Unit> list = GetCoroutineList();

            int moveCount = unitArray.Length;

            for (int i = 0; i < moveCount; i++)
            {
                list.MoveNext();
            }

            Assert.AreEqual(unitArray.Length - moveCount, list.WaitCount);
            Assert.AreEqual(moveCount, list.CompleteCount);
        }

        CoroutineList<Unit> GetCoroutineList()
        {
            CoroutineList<Unit> list = new CoroutineList<Unit>();
            list.SetCoroutines(unitArray, item => item.MoveNext());
            return list;
        }

        class Unit 
        {
            public Unit(int id)
            {
                this.ID = id;
            }

            public int ID { get; private set; }

            public IEnumerator MoveNext()
            {
                while (true)
                {
                    yield return this;
                }
            }
        }

    }

}
