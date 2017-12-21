using JiongXiaGu.Collections;
using System;
using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformDescriptionDictionary
    {
        public Dictionary<string, LandformDescription> Descriptions { get; private set; }

        public LandformDescriptionDictionary()
        {
            Descriptions = new Dictionary<string, LandformDescription>();
        }

        /// <summary>
        /// 添加可读取的资源;
        /// </summary>
        public void Add(Description<LandformDescription> descriptions)
        {
            //var descrs = serializer.Deserialize(content.Content);
            //foreach (var descr in descrs)
            //{
            //    string key = descr.ID;
            //    Description<LandformDescription> value = new Description<LandformDescription>(content, descr);
            //    Descriptions.AddOrUpdate(key, value);
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清除所有可读取的资源;
        /// </summary>
        public void Clear()
        {
            Descriptions.Clear();
        }
    }
}
