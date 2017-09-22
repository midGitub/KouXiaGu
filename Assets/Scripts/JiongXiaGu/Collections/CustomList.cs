using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Collections
{

    public class CustomList<T> : List<T>, IReadOnlyList<T>
    {
        public CustomList() : base() { }
        public CustomList(IEnumerable<T> collection) : base(collection) { }
        public CustomList(int capacity) : base(capacity) { }

    }

}
