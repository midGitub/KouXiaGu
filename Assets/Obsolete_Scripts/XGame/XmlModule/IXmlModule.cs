using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using XGame.Running;

namespace XGame.XmlModule
{

    /// <summary>
    /// 组合模块接口;
    /// </summary>
    public interface IXmlModule
    {

        /// <summary>
        /// 节点名;
        /// </summary>
        string XElementName { get; }

        /// <summary>
        /// 是否不允许多重放置?
        /// </summary>
        bool IsDisallowMultiple { get; }

        /// <summary>
        /// 向物体添加对应信息;
        /// </summary>
        /// <param name="insObject"></param>
        /// <param name="module"></param>
        void Add(XElement module, XGameObject insObject, ModInfo modInfo);

    }

}
