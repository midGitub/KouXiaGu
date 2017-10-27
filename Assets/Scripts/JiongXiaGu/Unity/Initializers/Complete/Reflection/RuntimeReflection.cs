using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 统一对代码进行反射操作;
    /// </summary>
    internal class RuntimeReflection
    {
        /// <summary>
        /// 所有反射处置器;
        /// </summary>
        public List<ReflectionHandler> ReflectionHandlers { get; private set; }

        /// <summary>
        /// 临时的合集;
        /// </summary>
        private List<ReflectionHandler> tempEffectiveHandlers;

        public RuntimeReflection()
        {
            ReflectionHandlers = new List<ReflectionHandler>();
            tempEffectiveHandlers = new List<ReflectionHandler>();
        }

        public RuntimeReflection(IEnumerable<ReflectionHandler> reflectionHandlers)
        {
            ReflectionHandlers = new List<ReflectionHandler>(reflectionHandlers);
            tempEffectiveHandlers = new List<ReflectionHandler>(ReflectionHandlers.Count);
        }

        /// <summary>
        /// 进行反射处理;
        /// </summary>
        public void Implement(params Assembly[] assemblys)
        {
            Implement(assemblys as IEnumerable<Assembly>);
        }

        /// <summary>
        /// 进行反射处理;
        /// </summary>
        public void Implement(IEnumerable<Assembly> assemblys)
        {
            if (assemblys == null)
                throw new ArgumentNullException(nameof(assemblys));

            foreach (var assembly in assemblys)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    Do(type);
                }
            }
        }

        /// <summary>
        /// 获取Type信息,整理提供给处置程序;
        /// </summary>
        private void Do(Type type)
        {
            BindingAttrGroup bindingAttrs;
            List<ReflectionHandler> effectiveHandlers = GetEffectiveHandlers(type, out bindingAttrs);
            if (effectiveHandlers.Count != 0)
            {
                FieldInfo[] fields = null;
                MethodInfo[] methods = null;
                PropertyInfo[] properties = null;

                if (bindingAttrs.Field != 0)
                {
                    fields = type.GetFields(bindingAttrs.Field);
                }

                if (bindingAttrs.Method != 0)
                {
                    methods = type.GetMethods(bindingAttrs.Method);
                }

                if (bindingAttrs.Property != 0)
                {
                    properties = type.GetProperties(bindingAttrs.Property);
                }

                foreach (var reflectionHandler in effectiveHandlers)
                {
                    BindingAttrGroup bindingFlagsInfo = reflectionHandler.BindingFlagsInfo;

                    if (reflectionHandler.BindingFlagsInfo.IsSearchField)
                    {
                        reflectionHandler.Do(Where(fields, bindingFlagsInfo.Field));
                    }

                    if (reflectionHandler.BindingFlagsInfo.IsSearchMethod)
                    {
                        reflectionHandler.Do(Where(methods, bindingFlagsInfo.Method));
                    }

                    if (reflectionHandler.BindingFlagsInfo.IsSearchProperty)
                    {
                        reflectionHandler.Do(Where(properties, bindingFlagsInfo.Property));
                    }

                    reflectionHandler.OnCompleted();
                }
            }
        }

        internal const BindingFlags DefineFieldBindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        internal const BindingFlags DefineMethodBindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        internal const BindingFlags DefinetPropertyBindingAttr = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 获取到所有生效的处理器和它们的搜索方式;
        /// </summary>
        private List<ReflectionHandler> GetEffectiveHandlers(Type type, out BindingAttrGroup bindingAttrs)
        {
            tempEffectiveHandlers.Clear();
            bindingAttrs = new BindingAttrGroup();

            foreach (var reflectionHandler in ReflectionHandlers)
            {
                if (reflectionHandler.IsEffective(type))
                {
                    bindingAttrs |= reflectionHandler.BindingFlagsInfo;
                    tempEffectiveHandlers.Add(reflectionHandler);
                }
            }

            bindingAttrs.Field &= DefineFieldBindingAttr;
            bindingAttrs.Method &= DefineMethodBindingAttr;
            bindingAttrs.Property &= DefinetPropertyBindingAttr;
            return tempEffectiveHandlers;
        }

        private IEnumerable<FieldInfo> Where(IEnumerable<FieldInfo> fields, BindingFlags bindingAttr)
        {
            foreach (var field in fields)
            {
                if (field.IsPublic != (bindingAttr & BindingFlags.Public) > 0)
                    continue;
                if (field.IsStatic != (bindingAttr & BindingFlags.Static) > 0)
                    continue;

                yield return field;
            }
        }

        private IEnumerable<MethodInfo> Where(IEnumerable<MethodInfo> methods, BindingFlags bindingAttr)
        {
            foreach (var method in methods)
            {
                if (method.IsPublic != (bindingAttr & BindingFlags.Public) > 0)
                    continue;
                if (method.IsStatic != (bindingAttr & BindingFlags.Static) > 0)
                    continue;

                yield return method;
            }
        }

        private IEnumerable<PropertyInfo> Where(IEnumerable<PropertyInfo> properties, BindingFlags bindingAttr)
        {
            foreach (var property in properties)
            {
                if (property.GetMethod.IsPublic != (bindingAttr & BindingFlags.Public) > 0)
                    continue;
                if (property.GetMethod.IsStatic != (bindingAttr & BindingFlags.Static) > 0)
                    continue;

                yield return property;
            }
        }
    }
}
