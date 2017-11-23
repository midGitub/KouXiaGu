using JiongXiaGu.Unity.Resources;
using System.IO;
using System;

namespace JiongXiaGu.Unity.RectMaps
{

    /// <summary>
    /// 游戏地图文件信息;
    /// </summary>
    public class MapFileInfo
    {
        public MapDescription Description { get; internal set; }
        public LoadableContent Content { get; internal set; }
        public ILoadableEntry Entry { get; internal set; }

        public MapFileInfo(MapDescription description, LoadableContent content, ILoadableEntry entry)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            Description = description;
            Content = content;
            Entry = entry;
        }
    }
}
