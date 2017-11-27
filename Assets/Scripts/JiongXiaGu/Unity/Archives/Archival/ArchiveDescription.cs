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
        [XmlElement]
        public string Name { get; set; }

        /// <summary>
        /// 是否为自动保存的存档?
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
        [XmlIgnore]
        public DateTime Time
        {
            get { return new DateTime(TimeTicks); }
        }

        public ArchiveDescription(string name) : this(name, false)
        {
        }

        public ArchiveDescription(string name, bool isAutoSave) : this()
        {
            Name = name;
            IsAutoSave = isAutoSave;
        }

        /// <summary>
        /// 更新存档信息到当前配置变量(仅在Unity线程);
        /// </summary>
        public ArchiveDescription Update()
        {
            TimeTicks = DateTime.Now.Ticks;
            ProgramVersion = Application.version;
            return this;
        }

        public override string ToString()
        {
            return base.ToString() + "[Name:" + Name + ",Time:" + Time.ToShortTimeString() + "]";
        }
    }
}
