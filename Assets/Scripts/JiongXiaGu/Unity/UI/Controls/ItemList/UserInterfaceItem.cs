//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace JiongXiaGu.UI
//{

//    /// <summary>
//    /// 条目;
//    /// </summary>
//    public abstract class UserInterfaceItem<T> : MonoBehaviour
//    {

//        protected abstract ItemList<T> parent { get; set; }
//        public abstract T Value { get; set; }

//        protected virtual void Start()
//        {
//            if (parent != null)
//            {
//                parent.Add(this);
//            }
//        }

//        public UserInterfaceItem<T> Create(T value, ItemList<T> parent, Transform uiParent)
//        {
//            var uiItem = Instantiate(this, uiParent);
//            uiItem.Value = value;
//            uiItem.parent = parent;
//            return uiItem;
//        }
//    }
//}
