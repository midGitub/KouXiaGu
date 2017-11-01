//using System;
//using System.Reflection;

//namespace JiongXiaGu.Unity.Initializers
//{
//    /// <summary>
//    /// 包括 字段,方法, 属性的搜索方法;
//    /// </summary>
//    public struct BindingAttrGroup : IEquatable<BindingAttrGroup>
//    {
//        /// <summary>
//        /// 字段类型搜索方法;
//        /// </summary>
//        public BindingFlags Field { get; set; }

//        /// <summary>
//        /// 方法类型搜索方法;
//        /// </summary>
//        public BindingFlags Method { get; set; }

//        /// <summary>
//        /// 属性类型搜索方法;
//        /// </summary>
//        public BindingFlags Property { get; set; }

//        /// <summary>
//        /// 是否检查字段?
//        /// </summary>
//        public bool IsSearchField
//        {
//            get { return (Field & RuntimeReflection.DefineFieldBindingAttr) > 0; }
//        }

//        /// <summary>
//        /// 是否检查方法?
//        /// </summary>
//        public bool IsSearchMethod
//        {
//            get { return (Method & RuntimeReflection.DefineMethodBindingAttr) > 0; }
//        }

//        /// <summary>
//        /// 是否检查属性?
//        /// </summary>
//        public bool IsSearchProperty
//        {
//            get { return (Property & RuntimeReflection.DefinetPropertyBindingAttr) > 0; }
//        }

//        public override bool Equals(object obj)
//        {
//            return obj is BindingAttrGroup && Equals((BindingAttrGroup)obj);
//        }

//        public bool Equals(BindingAttrGroup other)
//        {
//            return Field == other.Field &&
//                   Method == other.Method &&
//                   Property == other.Property;
//        }

//        public override int GetHashCode()
//        {
//            var hashCode = 1861228943;
//            hashCode = hashCode * -1521134295 + Field.GetHashCode();
//            hashCode = hashCode * -1521134295 + Method.GetHashCode();
//            hashCode = hashCode * -1521134295 + Property.GetHashCode();
//            return hashCode;
//        }

//        public static BindingAttrGroup operator |(BindingAttrGroup a, BindingAttrGroup b)
//        {
//            a.Field |= b.Field;
//            a.Method |= b.Method;
//            a.Property |= b.Property;
//            return a;
//        }

//        public static BindingAttrGroup operator &(BindingAttrGroup a, BindingAttrGroup b)
//        {
//            a.Field &= b.Field;
//            a.Method &= b.Method;
//            a.Property &= b.Property;
//            return a;
//        }

//        public static bool operator ==(BindingAttrGroup a, BindingAttrGroup b)
//        {
//            return a.Equals(b);
//        }

//        public static bool operator !=(BindingAttrGroup a, BindingAttrGroup b)
//        {
//            return !(a == b);
//        }
//    }
//}
