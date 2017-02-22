using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{

    /// <summary>
    /// 资源合集;
    /// </summary>
    [Obsolete]
    public class ResourceGroup : IReadOnlyDictionary<int, Resource>
    {

        public ResourceGroup()
        {
            this.resources = new CustomDictionary<int, Resource>();
        }

        public ResourceGroup(IEnumerable<int> resIdentifications)
        {
            this.resources = resIdentifications.ToDictionary<Resource>();
        }


        /// <summary>
        /// 资源信息;
        /// </summary>
        CustomDictionary<int, Resource> resources;


        public int Count
        {
            get { return ((IReadOnlyDictionary<int, Resource>)this.resources).Count; }
        }

        public IEnumerable<int> Keys
        {
            get { return ((IReadOnlyDictionary<int, Resource>)this.resources).Keys; }
        }

        public IEnumerable<Resource> Values
        {
            get { return ((IReadOnlyDictionary<int, Resource>)this.resources).Values; }
        }

        public Resource this[int key]
        {
            get {  return ((IReadOnlyDictionary<int, Resource>)this.resources)[key]; }
        }


        public bool ContainsKey(int key)
        {
            return ((IReadOnlyDictionary<int, Resource>)this.resources).ContainsKey(key);
        }

        public bool TryGetValue(int key, out Resource value)
        {
            return ((IReadOnlyDictionary<int, Resource>)this.resources).TryGetValue(key, out value);
        }

        public IEnumerator<KeyValuePair<int, Resource>> GetEnumerator()
        {
            return ((IReadOnlyDictionary<int, Resource>)this.resources).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyDictionary<int, Resource>)this.resources).GetEnumerator();
        }

    }

}
