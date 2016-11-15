using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace KouXiaGu
{

    public static class AttributeHelper
    {

        public static Dictionary<TKey, TValue> GetDictionaryFormField<TKey, TValue>(Type type, BindingFlags bindingFlags)
           where TValue : Attribute
        {
            Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
            FieldInfo[] fieldInfoArray = type.GetFields(bindingFlags);

            foreach (var fieldInfo in fieldInfoArray)
            {
                TValue t =
                     (TValue)Attribute.GetCustomAttribute(fieldInfo, typeof(TValue));
                if (t != null)
                {
                    dictionary.Add((TKey)fieldInfo.GetValue(null), t);
                }
            }
            return dictionary;
        }


        public static Dictionary<TKey, FieldInfo> GetFieldInfosFormField<TKey, TValue>(
            Type type, BindingFlags bindingFlags, Func<TValue, TKey> func)
             where TValue : Attribute
        {
            Dictionary<TKey, FieldInfo> dictionary = new Dictionary<TKey, FieldInfo>();
            FieldInfo[] fieldInfoArray = type.GetFields(bindingFlags);

            foreach (var fieldInfo in fieldInfoArray)
            {
                TValue t =
                     (TValue)Attribute.GetCustomAttribute(fieldInfo, typeof(TValue));
                if (t != null)
                {
                    dictionary.Add(func(t), fieldInfo);
                }
            }
            return dictionary;
        }


    }

}
