using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using UnityEngine;
using System.Xml.Serialization;

namespace KouXiaGu.World.Resources
{

    [XmlType("Tag")]
    public class LandformTag
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 地形标签管理;
    /// </summary>
    public class LandformTagConverter
    {
        public LandformTagConverter(LandformTag[] tags)
        {
            if (tags.Length > 32)
            {
                Debug.LogWarning("定义地形标签大于32个,将会舍弃多余的标签;");
            }

            this.tags = tags;
            Tags = tags.AsReadOnlyList();
        }

        readonly LandformTag[] tags;

        public IReadOnlyList<LandformTag> Tags { get; private set; }

        public int TagsToMask(string tags)
        {
            if (string.IsNullOrEmpty(tags))
            {
                return 0;
            }

            IEnumerable<string> enumerateTags = EnumerateTags(tags);
            return TagsToMask(enumerateTags);
        }

        static readonly char[] tagSeparator = new char[]
            {
                ',',
            };

        IEnumerable<string> EnumerateTags(string tags)
        {
            tags = tags.ToLower();
            string[] tagArray = tags.Split(tagSeparator, StringSplitOptions.RemoveEmptyEntries);
            return tagArray.Select(item => item.Trim());
        }

        public int TagsToMask(IEnumerable<string> enumerateTags)
        {
            int mask = 0;
            foreach (var tag in enumerateTags)
            {
                int index = tags.FindIndex(item => item.Name == tag);
                if (index != -1)
                {
                    mask |= 1 << index;
                }
            }
            return mask;
        }
    }

    public class LandformTagFile : SingleFilePath
    {
        [CustomFilePath("地形标签定义;")]
        public const string fileName = "World/Resources/Tags.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    /// <summary>
    /// 地形标签读取;
    /// </summary>
    public class LandformTagXmlSerializer : FileReaderWriter<LandformTag[]>
    {
        static readonly IOFileSerializer<LandformTag[]> fileSerializer = new XmlFileSerializer<LandformTag[]>();

        public LandformTagXmlSerializer() : base(new LandformTagFile(), fileSerializer)
        {
        }
    }
}
