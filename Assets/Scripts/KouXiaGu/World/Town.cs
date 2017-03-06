using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace KouXiaGu.World
{

    /// <summary>
    /// 城镇信息;
    /// </summary>
    [XmlType("Town")]
    public class Town : IEquatable<Town>
    {

        public Town(int id)
        {
            this.ID = id;
            this.Neighbours = new List<int>();
        }

        public Town(int id, IEnumerable<int> neighbours)
        {
            this.ID = id;
            this.Neighbours = new List<int>(neighbours);
        }


        [XmlAttribute("id")]
        int id;

        /// <summary>
        /// 城镇编号;
        /// </summary>
        public int ID
        {
            get { return id; }
            private set { id = value; }
        }

        /// <summary>
        /// 人力资源;
        /// </summary>
        public DynamicResource HumanResource { get; private set; }



        [XmlArrayItem("Neighbours")]
        List<int> neighbours;

        /// <summary>
        /// 相邻城镇;
        /// </summary>
        public List<int> Neighbours
        {
            get { return neighbours; }
            private set { neighbours = value; }
        }


        public bool Equals(Town other)
        {
            return other.ID == this.ID;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Town;

            if (item == null)
                return false;

            return Equals(item);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }

    }

}
