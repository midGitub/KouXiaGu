using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Resources;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectTerrain
{

    public class LandformDescrDictionary
    {
        private readonly LandformDescrSerializer serializer;
        public Dictionary<string, Description<LandformDescription>> Descriptions { get; private set; }

        public LandformDescrDictionary()
        {
            Descriptions = new Dictionary<string, Description<LandformDescription>>();
            serializer = new LandformDescrSerializer();
        }

        /// <summary>
        /// 添加可读取的资源;
        /// </summary>
        public void Add(LoadableContent content)
        {
            var descrs = serializer.Deserialize(content);
            foreach (var descr in descrs)
            {
                string key = descr.ID;
                Description<LandformDescription> value = new Description<LandformDescription>(content, descr);
                Descriptions.AddOrUpdate(key, value);
            }
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
