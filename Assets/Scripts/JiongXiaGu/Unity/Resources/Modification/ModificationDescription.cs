using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组描述信息;
    /// </summary>
    [XmlRoot("Modification")]
    public struct ModificationDescription
    {
        /// <summary>
        /// 唯一标识,只允许数字,字母,下划线组成;
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 模组名称;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 作者;
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 版本;
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 标签;
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 所有 AssetBundle 描述;
        /// </summary>
        public Set<AssetBundleDescription> AssetBundles { get; set; }
    }
}
