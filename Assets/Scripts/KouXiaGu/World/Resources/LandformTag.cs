using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KouXiaGu.World.Resources
{

    /// <summary>
    /// 地形标签管理;
    /// </summary>
    public class LandformTag
    {
        public LandformTag(string[] tags)
        {
            if (tags.Length > 32)
            {
                Debug.LogWarning("定义地形标签大于32个,将会舍弃多余的标签;");
            }

            this.tags = tags;
            Tags = tags.AsReadOnlyList();
        }

        readonly string[] tags;

        public IReadOnlyList<string> Tags { get; private set; }

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
