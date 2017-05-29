using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形标签管理;
    /// </summary>
    public class LandformTag
    {
        public LandformTag(string[] tags)
        {
            this.tags = tags;
        }

        readonly string[] tags;

        public string[] Tags
        {
            get { return tags; }
        }

        public int TagsToMask(string tags)
        {
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
                int index = tags.FindIndex(tag);
                if (index != -1)
                {
                    mask |= 1 << index;
                }
            }
            return mask;
        }
    }
}
