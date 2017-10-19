using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Initializers;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 模组资源读取;
    /// </summary>
    public class ModReader
    {
        private List<ModContentReader> modContentReader;

        public ModReader()
        {
            modContentReader = GetReader();
        }

        /// <summary>
        /// 读取到模组资源;
        /// </summary>
        public ModContent Read(ModContent modContent, ModContentTypes contentType, CancellationToken token = default(CancellationToken))
        {
            var contentReaders = modContentReader.Where(reader => reader.ContentType == contentType);
            foreach (var contentReader in contentReaders)
            {
                contentReader.Read(modContent, token);
            }
            return modContent;
        }

        /// <summary>
        /// 获取到所有模组内容读取器;
        /// </summary>
        private static List<ModContentReader> GetReader()
        {
            return ModContentReaderSearcher.GetReader();
        }
    }

    /// <summary>
    /// 模组内容类型;
    /// </summary>
    public enum ModContentTypes
    {
        /// <summary>
        /// 游戏可选择的内容;如 游戏语言包(非模组语言补充包),游戏地图文件;
        /// </summary>
        OptionalContent,

        /// <summary>
        /// 游戏资源;如 地形贴图定义,建筑模型定义;
        /// </summary>
        ResourceDefinition,
    }

    /// <summary>
    /// 模组内容读取;
    /// </summary>
    public abstract class ModContentReader
    {
        /// <summary>
        /// 读取器所读取的内容;
        /// </summary>
        public ModContentTypes ContentType { get; private set; }

        /// <summary>
        /// 读取模组资源,若已经读取了,则不进行任何操作;
        /// </summary>
        public abstract void Read(ModContent content, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// 重新读取模组资源;
        /// </summary>
        public abstract void Reread(ModContent content, CancellationToken token = default(CancellationToken));
    }

    /// <summary>
    /// 模组内容;
    /// </summary>
    public struct ModContent
    {
        /// <summary>
        /// 模组信息;
        /// </summary>
        public ModInfo ModInfo { get; private set; }

        /// <summary>
        /// 资源合集;
        /// </summary>
        public ComponentCollection<ModComponentContent> Content { get; private set; }

        public ModContent(ModInfo modInfo)
        {
            if (modInfo == null)
                throw new ArgumentNullException(nameof(modInfo));

            ModInfo = modInfo;
            Content = new ComponentCollection<ModComponentContent>();
        }

        public ModContent(ModInfo modInfo, ComponentCollection<ModComponentContent> content)
        {
            if (modInfo == null)
                throw new ArgumentNullException(nameof(modInfo));
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            ModInfo = modInfo;
            Content = content;
        }
    }

    /// <summary>
    /// 模组组件内容;
    /// </summary>
    public abstract class ModComponentContent
    {
        public ModContentStatus Status { get; protected set; }
    }

    /// <summary>
    /// 模组组件内容状态;
    /// </summary>
    public enum ModContentStatus
    {
        /// <summary>
        /// 未找到对应的资源信息;
        /// </summary>
        NotFound,

        /// <summary>
        /// 读取失败;
        /// </summary>
        Faulted,

        /// <summary>
        /// 所有资源信息读取完毕;
        /// </summary>
        Finished,
    }
}
