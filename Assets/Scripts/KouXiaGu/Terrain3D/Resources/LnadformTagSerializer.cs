using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Resources;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public class LandformTagFilePath : SingleFilePath
    {
        [CustomFilePath("地形标签定义;")]
        public const string fileName = "World/Terrain/Tags.xml";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    /// <summary>
    /// 地形标签读取;
    /// </summary>
    public class LandformTagXmlSerializer : FileXmlSerializer<string[]>
    {
        public LandformTagXmlSerializer() : base(new LandformTagFilePath())
        {
        }

        public LandformTagXmlSerializer(MultipleFilePath file) : base(file)
        {
        }
    }
}
