using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{

    [TestFixture]
    public class OperationRecorderTest
    {
        [Test]
        public void MaxRecordTest()
        {
            var recorder = new OperationRecorder<IVoidableOperation>(40);

            for (int i = 0; i < 20; i++)
            {
                recorder.Register(new EmptyOperation(0));
            }
            for (int i = 0; i < 20; i++)
            {
                recorder.Register(new EmptyOperation(1));
            }
            Assert.AreEqual(40, recorder.Count);
            recorder.MaxRecord = 20;
            recorder.Register(new EmptyOperation(1));
            Assert.AreEqual(20, recorder.Count);

            foreach (EmptyOperation item in recorder.Operations)
            {
                Assert.AreEqual(1, item.ID);
            }
        }

        class EmptyOperation : IVoidableOperation
        {
            public EmptyOperation(int id)
            {
                ID = id;
            }

            public int ID { get; private set; }

            public void Redo() { }
            public void Undo() { }
        }


        [Test]
        public void Test1()
        {
            List<int> data = GetData();
            var recorder = new OperationRecorder<IVoidableOperation>();

            var add4 = new ListAdd<int>(data, 4);
            Assert.AreEqual(4, data[4]);
            Assert.AreEqual(5, data.Count);
            recorder.Register(add4);
            Assert.AreEqual(1, recorder.Count);

            var add5 = new ListAdd<int>(data, 5);
            Assert.AreEqual(5, data[5]);
            Assert.AreEqual(6, data.Count);
            recorder.Register(add5);
            Assert.AreEqual(2, recorder.Count);

            var remove1 = new ListRemoveAt<int>(data, 1);
            Assert.AreEqual(0, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(5, data[4]);
            Assert.AreEqual(5, data.Count);
            recorder.Register(remove1);
            Assert.AreEqual(3, recorder.Count);


            recorder.PerformUndo();
            Assert.AreEqual(6, data.Count);

            recorder.PerformRedo();
            recorder.PerformRedo();
            recorder.PerformRedo();
            Assert.AreEqual(0, data[0]);
            Assert.AreEqual(2, data[1]);
            Assert.AreEqual(3, data[2]);
            Assert.AreEqual(4, data[3]);
            Assert.AreEqual(5, data[4]);
            Assert.AreEqual(5, data.Count);

            recorder.PerformUndo();
            recorder.PerformUndo();
            recorder.PerformUndo();
            recorder.PerformUndo();
            Assert.AreEqual(0, data[0]);
            Assert.AreEqual(1, data[1]);
            Assert.AreEqual(2, data[2]);
            Assert.AreEqual(3, data[3]);
            Assert.AreEqual(4, data.Count);

            var add9 = new ListAdd<int>(data, 9);
            Assert.AreEqual(5, data.Count);
            recorder.Register(add9);
            Assert.AreEqual(1, recorder.Count);
        }


        List<int> GetData()
        {
            List<int> data = new List<int>()
            {
               0, 1, 2, 3,
            };
            return data;
        }
    }
}
