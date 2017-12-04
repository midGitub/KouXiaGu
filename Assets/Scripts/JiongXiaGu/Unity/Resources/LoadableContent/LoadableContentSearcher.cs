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
        /// <summary>
        /// 忽略符,置于名称前缀,用于忽略某文件/文件夹;
        /// </summary>
        private const string IgnoreSymbol = "#ignore_";
        private LoadableContentFactory factory;

        public LoadableContentSearcher()
        {
            factory = new LoadableContentFactory();
        }

        /// <summary>
        /// 枚举目录下的所有可读资源;
        /// </summary>
        /// <param name="modsDirectory">目标目录</param>
        /// <param name="type">指定找到的模组类型</param>
        /// <returns></returns>
        public List<LoadableContent> FindLoadableContent(string modsDirectory, LoadableContentType type)
        {
            List<LoadableContent> list = new List<LoadableContent>();

            list.AddRange(EnumerateDirectory(modsDirectory, type));
            list.AddRange(EnumerateZipFile(modsDirectory, type));

            return list;
        }

        public IEnumerable<LoadableContent> EnumerateDirectory(string modsDirectory, LoadableContentType type)
        {
            foreach (var directory in Directory.EnumerateDirectories(modsDirectory, "*", SearchOption.TopDirectoryOnly))
            {
                string directoryName = Path.GetDirectoryName(directory);
                if (directoryName.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                LoadableContent info = null;

                try
                {
                    info = factory.Read(directory);
                }
                catch(Exception)
                {
                    continue;
                }

                yield return info;
            }
        }

        public IEnumerable<LoadableContent> EnumerateZipFile(string modsDirectory, LoadableContentType type)
        {
            foreach (var filePath in Directory.EnumerateFiles(modsDirectory, "*.zmod", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.StartsWith(IgnoreSymbol, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                LoadableContent info = null;

                try
                {
                    info = factory.ReadZip(filePath);
                }
                catch(Exception)
                {
                    continue;
                }

                yield return info;
            }
        }
    }
}
