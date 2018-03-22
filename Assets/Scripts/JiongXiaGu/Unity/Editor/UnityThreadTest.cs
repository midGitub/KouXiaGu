//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity
//{

//    [TestFixture]
//    public class UnityThreadTest
//    {
//        [Test]
//        public void Test()
//        {
//            Assert.IsTrue(UnityThread.IsUnityThread);


//            bool isRunInUnityThread = false;

//            UnityThread.Run(delegate ()
//            {
//                isRunInUnityThread = true;
//            }).Wait();

//            Assert.IsTrue(isRunInUnityThread);

//            Assert.IsFalse(UnityThread.IsPlaying);
//        }
//    }
//}
