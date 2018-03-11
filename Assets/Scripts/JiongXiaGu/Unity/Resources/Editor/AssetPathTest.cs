//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace JiongXiaGu.Unity.Resources
//{

//    [TestFixture]
//    public class AssetPathTest
//    {

//        private const string ContentID = "ABC";
//        private const string RelativePath = "123\\123123";

//        private static char PathReferenceSymbol
//        {
//            get { return AssetPath.PathReferenceSymbol; }
//        }

//        private static char PathStructuralSeparator
//        {
//            get { return AssetPath.PathStructuralSeparator; }
//        }

//        [Test]
//        public void ConstructorTest0()
//        {
//            AssetPath assetPath = new AssetPath(PathReferenceSymbol + string.Empty);
//            string contentID;
//            string relativePath;
//            bool isRelativePath = assetPath.GetRelativePath(out contentID, out relativePath);

//            Assert.AreEqual(true, isRelativePath);
//            Assert.AreEqual(string.Empty, relativePath);
//        }

//        [Test]
//        public void ConstructorTest1()
//        {
//            AssetPath assetPath = new AssetPath(PathReferenceSymbol + RelativePath);
//            string contentID;
//            string relativePath;
//            bool isRelativePath = assetPath.GetRelativePath(out contentID, out relativePath);

//            Assert.AreEqual(true, isRelativePath);
//            Assert.AreEqual(RelativePath, relativePath);
//        }

//        [Test]
//        public void ConstructorTest2()
//        {
//            AssetPath assetPath = new AssetPath(PathReferenceSymbol + ContentID + PathStructuralSeparator + RelativePath);
//            string contentID;
//            string relativePath;
//            bool isRelativePath = assetPath.GetRelativePath(out contentID, out relativePath);

//            Assert.AreEqual(false, isRelativePath);
//            Assert.AreEqual(ContentID, contentID);
//            Assert.AreEqual(RelativePath, relativePath);
//        }

//        [Test]
//        public void ConstructorTest3()
//        {
//            AssetPath assetPath = new AssetPath(PathReferenceSymbol + "  " + ContentID + "  " + PathStructuralSeparator + "  " + RelativePath + "  ");
//            string contentID;
//            string relativePath;
//            bool isRelativePath = assetPath.GetRelativePath(out contentID, out relativePath);

//            Assert.AreEqual(false, isRelativePath);
//            Assert.AreEqual(ContentID, contentID);
//            Assert.AreEqual(RelativePath, relativePath);
//        }

//        [Test]
//        public void ConstructorTest4()
//        {
//            AssetPath assetPath = new AssetPath(ContentID, RelativePath);
//            string contentID;
//            string relativePath;
//            bool isRelativePath = assetPath.GetRelativePath(out contentID, out relativePath);

//            Assert.AreEqual(false, isRelativePath);
//            Assert.AreEqual(ContentID, contentID);
//            Assert.AreEqual(RelativePath, relativePath);
//        }

//        [Test]
//        public void ToStringTest()
//        {
//            AssetPath assetPath = new AssetPath(PathReferenceSymbol + ContentID + PathStructuralSeparator + RelativePath);

//            Assert.AreEqual(assetPath.Name, assetPath.ToString());
//        }
//    }
//}
