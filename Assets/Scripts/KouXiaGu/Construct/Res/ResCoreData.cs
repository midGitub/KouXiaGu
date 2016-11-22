using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;

namespace KouXiaGu
{

    /// <summary>
    /// 定义核心资源文件的目录;
    /// </summary>
    public class ResCoreData
    {

        /// <summary>
        /// 核心资源文件夹名;
        /// </summary>
        const string coreDataDirectoryName = "Data";

        /// <summary>
        /// 完整的核心资源目录路径;
        /// </summary>
        static readonly string coreDataDirectoryPath = Path.Combine(Application.dataPath, coreDataDirectoryName);



        /// <summary>
        /// 完整的核心资源目录路径;
        /// </summary>
        public static string CoreDataDirectoryPath
        {
            get { return coreDataDirectoryPath; }
        }



    }

}
