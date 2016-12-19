using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public struct DictionaryChange<TKey, TValue>
    {

        public DictionaryChange(Operation operation, TKey key, TValue value, TValue newValue)
        {
            this.Operation = operation;
            this.Key = key;
            this.OriginalValue = value;
            this.NewValue = newValue;
        }

        /// <summary>
        /// 操作的类型;
        /// </summary>
        public Operation Operation { get; private set; }

        /// <summary>
        /// 索引值;
        /// </summary>
        public TKey Key { get; private set; }

        /// <summary>
        /// 原本的值;
        /// </summary>
        public TValue OriginalValue { get; private set; }

        /// <summary>
        /// 变更为的值;
        /// </summary>
        public TValue NewValue { get; private set; }
    }

}
