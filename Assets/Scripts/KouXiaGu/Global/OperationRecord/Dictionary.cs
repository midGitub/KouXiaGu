using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.OperationRecord
{


    public static class DictionaryExtensions
    {
        public static IVoidable VoidableSetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            var item = new DictionarySetValue<TKey, TValue>(dictionary, key, newValue);
            return item;
        }

        public static IVoidable VoidableAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            var item = new DictionaryAdd<TKey, TValue>(dictionary, key, value);
            return item;
        }

        public static IVoidable VoidableRemove<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var item = new DictionaryRemove<TKey, TValue>(dictionary, key);
            return item;
        }
    }

    sealed class DictionarySetValue<TKey, TValue> : SafeVoidable
    {
        public DictionarySetValue(IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue)
        {
            this.dictionary = dictionary;
            this.key = key;
            this.newValue = newValue;

            original = dictionary[key];
            dictionary[key] = newValue;
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        readonly TValue newValue;
        readonly TValue original;

        public override void PerformRedo()
        {
            dictionary[key] = newValue;
        }

        public override void PerformUndo()
        {
            dictionary[key] = original;
        }
    }

    sealed class DictionaryAdd<TKey, TValue> : SafeVoidable
    {
        public DictionaryAdd(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dictionary = dictionary;
            this.key = key;
            this.value = value;
            dictionary.Add(key, value);
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        readonly TValue value;

        public override void PerformRedo()
        {
            dictionary.Add(key, value);
        }

        public override void PerformUndo()
        {
            dictionary.Remove(key);
        }
    }

    sealed class DictionaryRemove<TKey, TValue> : SafeVoidable
    {
        public DictionaryRemove(IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            this.dictionary = dictionary;
            this.key = key;
            value = dictionary[key];
            dictionary.Remove(key);
        }

        readonly IDictionary<TKey, TValue> dictionary;
        readonly TKey key;
        readonly TValue value;

        public override void PerformRedo()
        {
            dictionary.Remove(key);
        }

        public override void PerformUndo()
        {
            dictionary.Add(key, value);
        }
    }
}
