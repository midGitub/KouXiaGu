using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Helper
{

    [TestFixture]
    public class PathHelperTest
    {

        [Test]
        public void RelativePathTest1()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\2.txt";
            string relativePath = @"2.txt";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest2()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu/Assets/Scenes\";
            string relativeTo = @"F:\My_Code\Unity5/KouXiaGu\Assets\Scenes\2.txt";
            string relativePath = @"2.txt";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest3()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\2.txt";
            string relativePath = @"Assets\Scenes\2.txt";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest4()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativePath = @"Assets\Scenes\";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest4_1()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativePath = @"Assets\Scenes\";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest5()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativePath = @"";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest5_1()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes";
            string relativePath = @"";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest5_2()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes";
            string relativePath = @"";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest5_3()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativePath = @"";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest6()
        {
            string absolutePath = @"123\1233/1233";
            string relativeTo = @"123\1233/1233\11.text";
            string relativePath = @"11.text";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }

        [Test]
        public void RelativePathTest7()
        {
            string absolutePath = @"F:\My_Code\Unity5\口虾蛄\Assets\Scenes";
            string relativeTo = @"F:\My_Code\Unity5\口虾蛄\Assets\Scenes\口虾蛄.txt";
            string relativePath = @"口虾蛄.txt";

            string actualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
            Assert.AreEqual(relativePath, actualRelativePath);
        }


        [Test]
        public void RelativePath_Error1()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\";
            string relativeTo = @"C:\My_Code\Unity5\";

            try
            {
                string ActualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
                Assert.Fail(ActualRelativePath);
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void RelativePath_Error2()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\";

            try
            {
                string ActualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
                Assert.Fail(ActualRelativePath);
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void RelativePath_Error3()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets2\Scenes\";

            try
            {
                string ActualRelativePath = PathHelper.GetRelativePath(absolutePath, relativeTo);
                Assert.Fail(ActualRelativePath);
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void IsCommonRoot_Test1()
        {
            string parent = @"F:\abcd\qwe";
            string child = @"F:\abcd\qwe\123.txt";
            bool isCommonRoot = true;

            bool _isCommonRoot = PathHelper.IsCommonRoot(parent, child);
            Assert.AreEqual(isCommonRoot, _isCommonRoot);
        }

        [Test]
        public void IsCommonRoot_Test2()
        {
            string parent = @"F:\abcd\qwe";
            string child = @"F:\abcd\qwe\";
            bool isCommonRoot = true;

            bool _isCommonRoot = PathHelper.IsCommonRoot(parent, child);
            Assert.AreEqual(isCommonRoot, _isCommonRoot);
        }

        [Test]
        public void IsCommonRoot_Test3()
        {
            string parent = @"F:\abcd\qwe\1123";
            string child = @"F:\abcd\qwe\";
            bool isCommonRoot = false;

            bool _isCommonRoot = PathHelper.IsCommonRoot(parent, child);
            Assert.AreEqual(isCommonRoot, _isCommonRoot);
        }

        [Test]
        public void IsCommonRoot_Test4()
        {
            string parent = @"abcd\qwe\1123";
            string child = @"abcd\qwe\";
            bool isCommonRoot = false;

            bool _isCommonRoot = PathHelper.IsCommonRoot(parent, child);
            Assert.AreEqual(isCommonRoot, _isCommonRoot);
        }

        [Test]
        public void IsCommonRoot_Test5()
        {
            string parent = @"目录\qwe\1123";
            string child = @"目录\qwe\";
            bool isCommonRoot = false;

            bool _isCommonRoot = PathHelper.IsCommonRoot(parent, child);
            Assert.AreEqual(isCommonRoot, _isCommonRoot);
        }

        [Test]
        public void IsMatch_Test1()
        {
            string input = "";
            string pattern = "";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test2()
        {
            string input = "123大点456";
            string pattern = "123大点?56";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test3()
        {
            string input = "123大点456";
            string pattern = "1*6";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test4()
        {
            string input = "123大点456";
            string pattern = "1*?6";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test5()
        {
            string input = "123大点456";
            string pattern = "*";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test6()
        {
            string input = "123大点456";
            string pattern = "123??456";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test7()
        {
            string input = "123大点4567";
            string pattern = "123大点456?";
            bool isMatch = true;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test8()
        {
            string input = "123大点4567";
            string pattern = "123大点456";
            bool isMatch = false;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }

        [Test]
        public void IsMatch_Test9()
        {
            string input = "123大点4567";
            string pattern = "1*6";
            bool isMatch = false;

            bool _isMatch = PathHelper.IsMatch(input, pattern);
            Assert.AreEqual(isMatch, _isMatch);
        }
    }
}
