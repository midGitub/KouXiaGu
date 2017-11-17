﻿using NUnit.Framework;
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
        public void RelativePathTest4_1()
        {
            string absolutePath = @"F:\My_Code\Unity5\KouXiaGu";
            string relativeTo = @"F:\My_Code\Unity5\KouXiaGu\Assets\Scenes\";
            string relativePath = @"Assets\Scenes\";

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
    }
}
