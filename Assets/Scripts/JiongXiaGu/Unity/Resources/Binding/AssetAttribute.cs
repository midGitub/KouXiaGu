using System;
using System.IO;

namespace JiongXiaGu.Unity.Resources.Binding
{

    /// <summary>
    /// 资源定义特性;需要挂载到公共的变量或者属性上;
    /// </summary>
    public abstract class AssetAttribute : Attribute
    {
        /// <summary>
        /// 相对路径;
        /// </summary>
        public string RelativePath { get; private set; }

        /// <summary>
        /// 预留消息;
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// 文件后缀;
        /// </summary>
        public abstract string FileExtension { get; }

        /// <summary>
        /// 传入路径会自动更改为对应路径;
        /// </summary>
        public AssetAttribute(string relativePath) : this(relativePath, null, true)
        {
        }

        /// <summary>
        /// 传入路径会自动更改为对应路径,除非 modifyExtension 则为false;
        /// </summary>
        public AssetAttribute(string relativePath, bool modifyExtension) : this (relativePath, null, modifyExtension)
        {
        }

        /// <summary>
        /// 传入路径会自动更改为对应路径;
        /// </summary>
        public AssetAttribute(string relativePath, string message) : this(relativePath, message, true)
        {
        }

        /// <summary>
        /// 传入路径会自动更改为对应路径,除非 modifyExtension 则为false;
        /// </summary>
        public AssetAttribute(string relativePath, string message, bool modifyExtension)
        {
            RelativePath = modifyExtension ? Path.ChangeExtension(relativePath, FileExtension) : relativePath;
            Message = message;
        }

        /// <summary>
        /// 获取到对应序列化器;
        /// </summary>
        public abstract ISerializer GetSerializer(Type type);
    }
}
