using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 对地形地图的描述;
    /// </summary>
    [Serializable, XmlType("TerrainMap")]
    public struct TerrainMapDescr
    {

        [XmlElement("id")]
        public int id;

        [XmlElement("Name")]
        public string name;

        [XmlElement("Time")]
        public long time;

        [XmlElement("Version")]
        public int version;

        [XmlElement("Description")]
        public string description;

        [XmlArray("Landforms")]
        public LandformRecord[] landformRecord;

        static readonly XmlSerializer TerrainMapInfoSerializer = new XmlSerializer(typeof(TerrainMapDescr));

        public static void Serialize(string filePath, TerrainMapDescr data)
        {
            TerrainMapInfoSerializer.SerializeXiaGu(filePath, data);
        }

        public static TerrainMapDescr Deserialize(string filePath)
        {
            TerrainMapDescr data = (TerrainMapDescr)TerrainMapInfoSerializer.DeserializeXiaGu(filePath);
            return data;
        }

    }

    /// <summary>
    /// 记录地图所有引用的地形;
    /// </summary>
    [Serializable, XmlType("Record")]
    public struct LandformRecord
    {
        [XmlAttribute("id")]
        public int ID;

        [XmlAttribute("count")]
        public int Count;

        public LandformRecord(int id, int count)
        {
            this.ID = id;
            this.Count = count;
        }
    }

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
