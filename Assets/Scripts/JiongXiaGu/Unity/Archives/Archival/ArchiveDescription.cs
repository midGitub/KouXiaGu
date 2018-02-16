using System;
using System.Xml.Serialization;
using UnityEngine;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 存档描述;
    /// </summary>
    [XmlRoot("ArchiveDescription")]
    public struct ArchiveDescription
    {
        /// <summary>
        /// 存档名;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 是否为自动保存的存档?
        /// </summary>
        public bool IsAutoSave { get; set; }

        /// <summary>
        /// 保存时的游戏程序版本;
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 存档最后修改时间;
        /// </summary>
        public DateTime Time { get; set; }

        public override string ToString()
        {
            return base.ToString() + "[Name:" + Name + ",Time:" + Time.ToShortTimeString() + "]";
        }
    }
}
