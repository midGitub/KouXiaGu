//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 帮助组件读取XML文件;
//    /// </summary>
//    [Serializable]
//    public sealed class XmlCreateInfoModule : CreateInfo, IXmlModule
//    {
//        private XmlCreateInfoModule() { }

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        private const string m_XElementName = "ObjectCreate";

//        private const bool m_IsDisallowMultiple = true;

//        /// <summary>
//        /// XElement名;
//        /// </summary>
//        public string XElementName
//        {
//            get { return m_XElementName; }
//        }

//        /// <summary>
//        /// 是否不允许多重放置?
//        /// </summary>
//        public bool IsDisallowMultiple
//        {
//            get { return m_IsDisallowMultiple; }
//        }

//        /// <summary>
//        /// 加入这个组件到物体上;
//        /// </summary>
//        /// <param name="insObject"></param>
//        /// <param name="module"></param>
//        /// <param name="resources"></param>
//        public void Add(XGameObject insObject, XElement module, PrefabRes resources)
//        {
//            CreateInfo createInfo = new CreateInfo();
//            LoadInfo(module, ref createInfo, this);
//            resources.Add(resources.XGameObjectName, createInfo);
//        }

//        /// <summary>
//        /// 根据Xml设置到;
//        /// </summary>
//        /// <param name="createElement"></param>
//        /// <param name="createInfo"></param>
//        /// <param name="defaultCreateInfo"></param>
//        public static void LoadInfo(XElement createElement, ref CreateInfo createInfo, CreateInfo defaultCreateInfo)
//        {
//            XAttribute CreatStyle = createElement.Attribute("Style");
//            createInfo.CreateStyle = AssembleXml.LoadEnum<CreatStyle>(CreatStyle, defaultCreateInfo.CreateStyle);

//            XAttribute CanRotate = createElement.Attribute("CanRotate");
//            createInfo.CanRotate = AssembleXml.Load(CanRotate, defaultCreateInfo.CanRotate);


//            XElement Limit = createElement.Element("Limit");

//            XAttribute LongestRow = Limit.Attribute("LongestRow");
//            createInfo.LimitLongestRow = AssembleXml.Load(LongestRow, defaultCreateInfo.LimitLongestRow);

//            XAttribute ShortestRow = Limit.Attribute("ShortestRow");
//            createInfo.LimitShortestRow = AssembleXml.Load(ShortestRow, defaultCreateInfo.LimitShortestRow);

//            XAttribute SpacingX = Limit.Attribute("SpacingX");
//            XAttribute SpacingY = Limit.Attribute("SpacingY");
//            createInfo.spacing = AssembleXml.Load(SpacingX, SpacingY, defaultCreateInfo.spacing);
//        }


//    }

//}
