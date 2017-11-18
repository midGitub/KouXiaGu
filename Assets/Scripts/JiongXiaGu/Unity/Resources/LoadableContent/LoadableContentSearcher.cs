using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 负责对可读的资源进行搜索;
    /// </summary>
    public class LoadableContentSearcher
    {
        private LoadableDirectoryReader contentReader;

        public LoadableContentSearcher()
        {
            contentReader = new LoadableDirectoryReader();
        }

        /// <summary>
        /// 枚举目录下的所有模组;
        /// </summary>
        /// <param name="modsDirectory">目标目录</param>
        /// <param name="type">指定找到的模组类型</param>
        /// <returns></returns>
        public IEnumerable<LoadableContentInfo> EnumerateModInfos(string modsDirectory, LoadableContentType type)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory))
            {
                LoadableContentInfo modInfo = null;
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    modInfo = contentReader.Create(directoryInfo, type);
                }
                catch (FileNotFoundException)
                {
                }

                if (modInfo != null)
                    yield return modInfo;
            }
        }
    }
}
