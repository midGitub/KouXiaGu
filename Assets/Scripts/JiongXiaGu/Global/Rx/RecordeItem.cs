

namespace JiongXiaGu
{

    /// <summary>
    /// 记录条目;
    /// </summary>
    public struct RecordeItem<TKey, TValue>
    {
        public RecordeItem(TKey key)
        {
            Key = key;
            OriginalValue = default(TValue);
            IsAdd = true;
        }

        public RecordeItem(TKey key, TValue originalValue)
        {
            Key = key;
            OriginalValue = originalValue;
            IsAdd = false;
        }

        /// <summary>
        /// 键值;
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// 原本的值;
        /// </summary>
        public TValue OriginalValue { get; private set; }

        /// <summary>
        /// 是否为新建?
        /// </summary>
        public bool IsAdd { get; private set; }
    }
}
