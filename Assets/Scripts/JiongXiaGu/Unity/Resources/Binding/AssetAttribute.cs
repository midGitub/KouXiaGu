using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 资源定义特性;需要挂载到公共的变量或者属性上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class AssetAttribute : Attribute
    {
        internal const bool defaultUseDefaultExtension = false;

        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; private set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 标签;
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 类型默认的文件后缀;
        /// </summary>
        protected abstract string defaultFileExtension { get; }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public AssetAttribute(string relativePath) : this(relativePath, null, defaultUseDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="useDefaultExtension">是否使用格式默认的后缀?</param>
        public AssetAttribute(string relativePath, bool useDefaultExtension) : this (relativePath, null, useDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="message">预留消息</param>
        public AssetAttribute(string relativePath, string message) : this(relativePath, message, defaultUseDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="message">预留消息</param>
        /// <param name="useDefaultExtension">是否使用格式默认的后缀?</param>
        public AssetAttribute(string relativePath, string message, bool useDefaultExtension)
        {
            RelativePath = useDefaultExtension ? Path.ChangeExtension(relativePath, defaultFileExtension) : relativePath;
            Message = message;
        }

        /// <summary>
        /// 创建到对应序列化器(保证线程安全);
        /// </summary>
        public abstract ISerializer CreateSerializer(Type type);
    }
}
