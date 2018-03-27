using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources.BindingSerialization
{

    /// <summary>
    /// 资源定义特性;需要挂载到公共的变量或者属性上;
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class AssetAttribute : ModificationSubpathInfoAttribute
    {
        internal const bool defaultUseDefaultExtension = false;
        internal const bool defaultIsNecessaryAsset = true;

        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 标签;
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 是否为必要的资源,若未false,在序列化时若未找到此资源,则不返回异常;
        /// </summary>
        public bool IsNecessaryAsset { get; set; } = defaultIsNecessaryAsset;

        /// <summary>
        /// 类型默认的文件后缀;
        /// </summary>
        public abstract string DefaultFileExtension { get; }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        public AssetAttribute(string relativePath) : this(relativePath, null, null, defaultUseDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="useDefaultExtension">是否使用格式默认的后缀?</param>
        public AssetAttribute(string relativePath, bool useDefaultExtension) : this (relativePath, null, null, useDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="message">预留消息</param>
        public AssetAttribute(string relativePath, string name, string message) : this(relativePath, name, message, defaultUseDefaultExtension)
        {
        }

        /// <summary>
        /// 构造函数;
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <param name="message">预留消息</param>
        /// <param name="useDefaultExtension">是否使用格式默认的后缀?</param>
        public AssetAttribute(string relativePath, string name, string message, bool useDefaultExtension) : base(name, message)
        {
            RelativePath = useDefaultExtension ? Path.ChangeExtension(relativePath, DefaultFileExtension) : relativePath;
        }

        /// <summary>
        /// 创建到对应序列化器(线程安全);
        /// </summary>
        public abstract ISerializer CreateSerializer(Type type);
    }
}
