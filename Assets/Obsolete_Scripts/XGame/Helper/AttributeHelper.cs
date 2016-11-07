using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XGame
{

    /// <summary>
    /// 特性工具类;
    /// </summary>
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

    }

}
