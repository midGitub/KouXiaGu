using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;

namespace KouXiaGu.Navigation
{

    class LnadformTagFilePath : SingleFilePath
    {
        public override string FileName
        {
            get { return "Navigation/Tags.xml"; }
        }
    }

    /// <summary>
    /// 地形标签读取;
    /// </summary>
    class LnadformTagSerializer : FileXmlSerializer<NavTagInfo[]>
    {
        public LnadformTagSerializer() : base(new LnadformTagFilePath())
        {
        }

        public LnadformTagSerializer(MultipleFilePath file) : base(file)
        {
        }

        public List<string> oRead()
        {
            List<string> tags = new List<string>()
            {
                "town",
                "desert",
                "lake",
                "sea",
            };

            if (tags.Count > 32)
            {
                throw new ArgumentException("定义地形标签大于32个!");
            }

            ToLower(ref tags);
            return tags;
        }

        void ToLower(ref List<string> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i] = tags[i].ToLower();
            }
        }
    }
}
