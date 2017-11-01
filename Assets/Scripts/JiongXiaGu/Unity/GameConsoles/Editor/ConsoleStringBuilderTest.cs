using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.GameConsoles
{
    /// <summary>
    /// 控制台记录类测试;
    /// </summary>
    [TestFixture]
    public class ConsoleStringBuilderTest
    {

        [Test]
        public void TestAddMessage_1()
        {
            StringBuilder stringBuilder = new StringBuilder(15, 15);
            stringBuilder.Append("Test1");
            ConsoleStringBuilder consoleStringBuilder = new ConsoleStringBuilder(stringBuilder);

            Assert.AreEqual("Test1", consoleStringBuilder.GetText());

            consoleStringBuilder.Write("Test2");
            Assert.AreEqual("Test1" + Environment.NewLine + "Test2", consoleStringBuilder.GetText());

            consoleStringBuilder.Write("Test3");
            Assert.AreEqual("Test2" + Environment.NewLine + "Test3", consoleStringBuilder.GetText());

            consoleStringBuilder.Write("Test4");
            Assert.AreEqual("Test3" + Environment.NewLine + "Test4", consoleStringBuilder.GetText());
        }

        [Test]
        public void TestAddMessage_2()
        {
            StringBuilder stringBuilder = new StringBuilder(14, 14);
            ConsoleStringBuilder consoleStringBuilder = new ConsoleStringBuilder(stringBuilder);

            consoleStringBuilder.Write("Test1 Test2 Test3 Test4 Test5");
            Assert.AreEqual(ConsoleStringBuilder.MessageTooLongErrorString, consoleStringBuilder.GetText());
        }
    }
}
