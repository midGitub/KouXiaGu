using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    class LandformTagFilePath : SingleFilePath
    {
        [CustomFilePath("地形标签定义;")]
        public const string fileName = "World/Navigation/LnadformTags.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    /// <summary>
    /// 地形标签读取;
    /// </summary>
    class LandformTagXmlSerializer : FileXmlSerializer<string[]>
    {
        public LandformTagXmlSerializer() : base(new LandformTagFilePath())
        {
        }

        public LandformTagXmlSerializer(MultipleFilePath file) : base(file)
        {
        }

        //public override NavTagInfo[] Read()
        //{
        //    NavTagInfo[] tags = base.Read();

        //    if (tags.Length > 32)
        //    {
        //        Debug.LogWarning("定义地形标签大于32个,将会舍弃多余的标签;");
        //    }

        //    return tags;
        //}

        //public List<string> oRead()
        //{
        //    List<string> tags = new List<string>()
        //    {
        //        "town",
        //        "desert",
        //        "lake",
        //        "sea",
        //    };

        //    if (tags.Count > 32)
        //    {
        //        throw new ArgumentException("定义地形标签大于32个!");
        //    }

        //    ToLower(ref tags);
        //    return tags;
        //}

        //void ToLower(ref List<string> tags)
        //{
        //    for (int i = 0; i < tags.Count; i++)
        //    {
        //        tags[i] = tags[i].ToLower();
        //    }
        //}
    }
}
