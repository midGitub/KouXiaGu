using System;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Resources.Archives
{

    /// <summary>
    /// 存档信息,记录存档信息,输出输入结构;
    /// </summary>
    [XmlRoot("ArchiveInfo")]
    public struct ArchiveInfo
    {

        public ArchiveInfo(string name, bool isAutoSave) : this()
        {
            Name = name;
            IsAutoSave = isAutoSave;
        }

        public ArchiveInfo(string name) : this(name, false)
        {
        }

        /// <summary>
        /// 创建一个当前适用的存档信息(仅在Unity线程);
        /// </summary>
        public ArchiveInfo(ArchiveInfo info) : this(info.Name, info.IsAutoSave)
        {
            TimeTicks = DateTime.Now.Ticks;
            ProgramVersion = Application.version;
        }

        /// <summary>
        /// 存档名;
        /// </summary>
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// 是否为自动保存存档?
        /// </summary>
        [XmlElement]
        public bool IsAutoSave { get; set; }

        /// <summary>
        /// 存档最后修改时间 Ticks;
        /// </summary>
        [XmlElement]
        public long TimeTicks { get; set; }

        /// <summary>
        /// 保存时的游戏程序版本;
        /// </summary>
        [XmlElement]
        public string ProgramVersion{ get; set; }

        /// <summary>
        /// 存档最后修改时间;
        /// </summary>
        public DateTime Time
        {
            get { return new DateTime(TimeTicks); }
        }

        public override string ToString()
        {
            return base.ToString() + "[Name:" + Name + ",Time:" + Time.ToShortTimeString() + "]";
        }
    }
}
