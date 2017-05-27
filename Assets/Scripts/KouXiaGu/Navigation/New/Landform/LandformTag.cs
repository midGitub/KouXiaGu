using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.Navigation
{

    /// <summary>
    /// 地形标签管理;
    /// </summary>
    public class LandformTag
    {
        public LandformTag(BasicTerrainResource basicResource)
        {
            var reader = new LnadformTagSerializer();
            tags = reader.Read();
        }

        readonly NavTagInfo[] tags;

        public IList<NavTagInfo> Tags
        {
            get { return tags; }
        }

        int TagsToMask(IList<string> definedTags, string tags)
        {
            IEnumerable<string> enumerateTags = EnumerateTags(tags);
            return TagsToMask(definedTags, enumerateTags);
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

        int TagsToMask(IList<string> definedTags, IEnumerable<string> tags)
        {
            int mask = 0;
            foreach (var tag in tags)
            {
                int index = definedTags.IndexOf(tag);
                if (index != -1)
                {
                    mask |= 1 << index;
                }
            }
            return mask;
        }
    }
}
