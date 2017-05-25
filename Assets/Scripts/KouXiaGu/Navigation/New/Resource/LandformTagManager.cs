using KouXiaGu.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Navigation
{

    public class LandformTagManager
    {
        public LandformTagManager(WorldResource basicResource)
        {
            LnadformTagReader reader = new LnadformTagReader();
            List<string> tags = reader.Read();



        }

        readonly IReadOnlyDictionary<int, int> landformMaskDictionary;
        readonly IReadOnlyDictionary<int, int> buildingMaskDictionary;

        public IReadOnlyDictionary<int, int> LandformMaskDictionary
        {
            get { return landformMaskDictionary; }
        }

        public IReadOnlyDictionary<int, int> BuildingMaskDictionary
        {
            get { return buildingMaskDictionary; }
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

    class LnadformTagReader
    {
        public List<string> Read()
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
