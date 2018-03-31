using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Rx
{

    [TestFixture]
    public class ObserverCollectionTest
    {

        [Test]
        public void TestList()
        {
            ObserverList<int> observerCollection = new ObserverList<int>();
            Test(observerCollection);
        }

        [Test]
        public void TestLinkedList()
        {
            ObserverLinkedList<int> observerCollection = new ObserverLinkedList<int>();
            Test(observerCollection);
        }

        private void Test(ObserverCollection<int> observerCollection)
        {
            var observer = new Observer();
            var unSubscriber = observerCollection.Subscribe(observer);

            try
            {
                observerCollection.Subscribe(observer);
                Assert.Fail("重复订阅;");
            }
            catch (ArgumentException)
            {
            }

            observerCollection.NotifyNext(1);
            observerCollection.NotifyNext(2);
            observerCollection.NotifyNext(3);
            observerCollection.NotifyError(null);
            observerCollection.NotifyCompleted();

            Assert.AreEqual(1, observer.Queue.Dequeue());
            Assert.AreEqual(2, observer.Queue.Dequeue());
            Assert.AreEqual(3, observer.Queue.Dequeue());
            Assert.AreEqual(int.MinValue, observer.Queue.Dequeue());
            Assert.AreEqual(int.MaxValue, observer.Queue.Dequeue());

            unSubscriber.Dispose();
            observerCollection.NotifyNext(4);

            Assert.AreEqual(0, observer.Queue.Count);
        }

        private class Observer : IObserver<int>
        {
            public Queue<int> Queue { get; private set; } = new Queue<int>();

            public void OnNext(int value)
            {
                Queue.Enqueue(value);
            }

            public void OnError(Exception error)
            {
                Queue.Enqueue(int.MinValue);
            }

            public void OnCompleted()
            {
                Queue.Enqueue(int.MaxValue);
            }
        }
    }
}
