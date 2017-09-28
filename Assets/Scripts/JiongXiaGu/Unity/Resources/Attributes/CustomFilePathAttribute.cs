using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 在程序中定义的文件路径,需要放在 public 访问级别的静态变量上;
    /// </summary>
    [Obsolete]
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class CustomFilePathAttribute : Attribute
    {
        public CustomFilePathAttribute() 
            : this(string.Empty, false)
        {
        }

        public CustomFilePathAttribute(string message)
            : this(message, false)
        {
        }

        public CustomFilePathAttribute(bool isMultipleFile) 
            : this(string.Empty, isMultipleFile)
        {
        }

        public CustomFilePathAttribute(string message, bool isMultipleFile)
        {
            Message = message;
            IsMultipleFile = isMultipleFile;
        }

        public string Message { get; private set; }

        /// <summary>
        /// 是否关联多个文件?
        /// </summary>
        public bool IsMultipleFile { get; private set; }
    }
}
