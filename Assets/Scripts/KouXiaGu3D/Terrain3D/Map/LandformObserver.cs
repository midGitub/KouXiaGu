using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 观察加入到地图的地形,并且做记录;
    /// </summary>
    public sealed class LandformObserver : DictionaryObserver<CubicHexCoord, TerrainNode>
    {

        readonly Dictionary<int, int> landformID;

        public LandformObserver()
        {
            landformID = new Dictionary<int, int>();
        }

        public LandformObserver(IEnumerable<LandformRecord> landform)
        {
            if (landform == null)
                throw new ArgumentNullException();

            landformID = landform.ToDictionary(record => new KeyValuePair<int, int>(record.ID, record.Count));
        }

        public List<LandformRecord> ToLandformRecord()
        {
            List<LandformRecord> list = new List<LandformRecord>(landformID.Count);
            foreach (var item in landformID)
            {
                list.Add(new LandformRecord(item.Key, item.Value));
            }
            return list;
        }

        protected override void Add(CubicHexCoord key, TerrainNode newValue)
        {
            int id = newValue.Landform;
            int count;
            if (landformID.TryGetValue(id, out count))
            {
                landformID[id]++;
            }
            else
            {
                landformID.Add(id, 1);
            }
        }

        protected override void Remove(CubicHexCoord key, TerrainNode originalValue)
        {
            int id = originalValue.Landform;
            int count;
            if (landformID.TryGetValue(id, out count))
            {
                if (count == 0)
                    throw new ArgumentOutOfRangeException("计数出错;");

                count--;

                if (count == 0)
                {
                    landformID.Remove(id);
                }
                else
                {
                    landformID[id] = count;
                }
                return;
            }

            if (count == 0)
                throw new ArgumentOutOfRangeException("计数出错;");
        }

        protected override void Update(CubicHexCoord key, TerrainNode originalValue, TerrainNode newValue)
        {
            Remove(key, originalValue);
            Add(key, newValue);
        }


    }


}
