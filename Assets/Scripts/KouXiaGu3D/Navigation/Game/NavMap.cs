using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;

namespace KouXiaGu.Navigation.Game
{

    /// <summary>
    /// 游戏导航地图;
    /// </summary>
    public class NavMap : IDictionary<CubicHexCoord, NavNode>
    {
        public NavNode this[CubicHexCoord key]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<CubicHexCoord> Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection<NavNode> Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Add(KeyValuePair<CubicHexCoord, NavNode> item)
        {
            throw new NotImplementedException();
        }

        public void Add(CubicHexCoord key, NavNode value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<CubicHexCoord, NavNode> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(CubicHexCoord key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<CubicHexCoord, NavNode>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<CubicHexCoord, NavNode>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<CubicHexCoord, NavNode> item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(CubicHexCoord key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(CubicHexCoord key, out NavNode value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

}
