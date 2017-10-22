using System;
using System.Collections.Generic;
using System.Reflection;

namespace JiongXiaGu.Unity.Initializers
{
    /// <summary>
    /// 反射处理抽象类;
    /// </summary>
    public abstract class ReflectionHandler
    {
        /// <summary>
        /// 类型搜索方法信息,所有搜索方法类型都有 BindingFlags.DeclaredOnly,若不搜索此类型则置为 BindingFlags.Default;
        /// Field 可选 BindingFlags.Static, BindingFlags.Instance, BindingFlags.Public, BindingFlags.NonPublic
        /// Method 可选 BindingFlags.Static, BindingFlags.Instance, BindingFlags.Public, BindingFlags.NonPublic
        /// Property 可选 BindingFlags.Static, BindingFlags.Instance, BindingFlags.Public, BindingFlags.NonPublic
        /// </summary>
        public BindingAttrGroup BindingFlagsInfo { get; private set; }

        public ReflectionHandler(BindingAttrGroup bindingAttrs)
        {
            BindingFlagsInfo = bindingAttrs;
        }

        /// <summary>
        /// 是否检查字段?
        /// </summary>
        public virtual bool IsSearchField
        {
            get { return (BindingFlagsInfo.Field & RuntimeReflection.DefineFieldBindingAttr) > 0; }
        }

        /// <summary>
        /// 是否检查方法?
        /// </summary>
        public virtual bool IsSearchMethod
        {
            get { return (BindingFlagsInfo.Method & RuntimeReflection.DefineMethodBindingAttr) > 0; }
        }

        /// <summary>
        /// 是否检查属性?
        /// </summary>
        public virtual bool IsSearchProperty
        {
            get { return (BindingFlagsInfo.Property & RuntimeReflection.DefinetPropertyBindingAttr) > 0; }
        }

        /// <summary>
        /// 进行对应操作,并返回是否继续处理这个Type类型?
        /// </summary>
        public abstract bool Do(Type type);

        /// <summary>
        /// 进行对应操作;
        /// </summary>
        public virtual void Do(IEnumerable<FieldInfo> fieldInfos)
        {
            return;
        }

        /// <summary>
        /// 进行对应操作;
        /// </summary>
        public virtual void Do(IEnumerable<MethodInfo> methodInfos)
        {
            return;
        }

        /// <summary>
        /// 进行对应操作;
        /// </summary>
        public virtual void Do(IEnumerable<PropertyInfo> propertyInfos)
        {
            return;
        }

        /// <summary>
        /// 仅当 Do(Type type) 返回true时,周期完成时调用;
        /// </summary>
        public virtual void OnCompleted()
        {
            return;
        }
    }
}
