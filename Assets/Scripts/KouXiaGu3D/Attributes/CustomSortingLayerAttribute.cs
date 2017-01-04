using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// 在代码中定义的 SortingLayer;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class CustomSortingLayerAttribute : Attribute
    {

        public CustomSortingLayerAttribute()
        {
        }

        public CustomSortingLayerAttribute(string sortingLayerName)
        {
            this.SortingLayerName = sortingLayerName;
        }

        public string SortingLayerName { get; private set; }

    }

}
