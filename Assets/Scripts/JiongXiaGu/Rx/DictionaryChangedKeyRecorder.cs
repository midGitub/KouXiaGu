using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    /// <summary>
    /// 监视字典结构内容变化,并且记录发生变化的Key;
    /// </summary>
    public class DictionaryChangedKeyRecorder<TKey, Tvalue> : IDictionaryObserver<TKey, Tvalue>
    {
        public DictionaryChangedKeyRecorder(ICollection<TKey> changedPositions)
        {
            ChangedPositions = changedPositions;
        }

        public ICollection<TKey> ChangedPositions { get; private set; }

        void IDictionaryObserver<TKey, Tvalue>.OnAdded(TKey key, Tvalue newValue)
        {
            ChangedPositions.Add(key);
        }

        void IDictionaryObserver<TKey, Tvalue>.OnRemoved(TKey key, Tvalue originalValue)
        {
            ChangedPositions.Remove(key);
        }

        void IDictionaryObserver<TKey, Tvalue>.OnUpdated(TKey key, Tvalue originalValue, Tvalue newValue)
        {
            ChangedPositions.Add(key);
        }
    }
}
