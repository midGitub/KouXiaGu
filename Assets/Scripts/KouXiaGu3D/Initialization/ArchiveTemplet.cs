using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 存档模板管理;
    /// </summary>
    public static class ArchiveTemplet
    {

        /// <summary>
        /// 默认存档位置;
        /// </summary>
        const string DEFAULT_ARCHIVE_DIRECTORY_NANE = "Templet\\DEFAULT";

        /// <summary>
        /// 用于编辑的存档位置,修改具体内容用于初始化游戏;
        /// </summary>
        const string EDIT_ARCHIVE_DIRECTORY_NANE = "Templet\\Temp";

        /// <summary>
        /// 测试时输出默认模板的位置;
        /// </summary>
        const string EMPTY_ARCHIVE_DIRECTORY_NANE = "Templet\\Empty";

        /// <summary>
        /// 默认存档;
        /// </summary>
        static ArchiveDirectory defaultArchived;

        /// <summary>
        /// 用于编辑的存档位置,修改具体内容用于初始化游戏;
        /// </summary>
        static ArchiveDirectory editArchived;

        /// <summary>
        /// 空模板;
        /// </summary>
        static ArchiveDirectory emptyArchived;

        /// <summary>
        /// 默认存档;
        /// </summary>
        static string DefaultArchivedDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, DEFAULT_ARCHIVE_DIRECTORY_NANE); }
        }

        /// <summary>
        /// 用于编辑的存档位置,修改具体内容用于初始化游戏的存档路径;
        /// </summary>
        static string EditArchivedDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, EDIT_ARCHIVE_DIRECTORY_NANE); }
        }

        /// <summary>
        /// 空模板路径;
        /// </summary>
        static string EmptyArchivedDirectory
        {
            get { return Path.Combine(ResourcePath.ConfigurationDirectoryPath, EMPTY_ARCHIVE_DIRECTORY_NANE); }
        }

        /// <summary>
        /// 默认存档;
        /// </summary>
        public static ArchiveDirectory DefaultArchived
        {
            get { return defaultArchived ?? (defaultArchived = new ArchiveDirectory(DefaultArchivedDirectory, false)); }
        }

        /// <summary>
        /// 用于编辑的存档;
        /// </summary>
        public static ArchiveDirectory EditArchived
        {
            get { return editArchived ?? (editArchived = new ArchiveDirectory(EditArchivedDirectory)); }
        }

        /// <summary>
        /// 空的模板存档位置;(测试使用);
        /// </summary>
        public static ArchiveDirectory EmptyArchived
        {
            get { return emptyArchived ?? (emptyArchived = new ArchiveDirectory(EmptyArchivedDirectory)); }
        }

        /// <summary>
        /// 输出测试使用的空模板到预定义的目录;
        /// </summary>
        public static void TempletOutput()
        {
            TempletOutput(EmptyArchived);
        }

        /// <summary>
        /// 输出测试使用的空模板到;
        /// </summary>
        public static void TempletOutput(ArchiveDirectory archive)
        {

        }

    }

}
