using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 获取到模组读取器(挂载到Unity物体);
    /// </summary>
    [DisallowMultipleComponent]
    internal class ModContentReaderSearcher : MonoBehaviour
    {
        private static IModContentReaderProvider[] readerProviders;

        private void Awake()
        {
            readerProviders = GetComponentsInChildren<IModContentReaderProvider>();
        }

        private void OnDestroy()
        {
            readerProviders = null;
        }

        public static List<ModContentReader> GetReader()
        {
            if (readerProviders == null)
            {
                throw new MissingComponentException(string.Format("场景未挂载[{0}]脚本,或尝试在{0}组件初始化之前获取读取器;", nameof(ModContentReaderSearcher)));
            }

            List<ModContentReader> readers = new List<ModContentReader>();
            foreach (var readerProvider in readerProviders)
            {
                var reader = readerProvider.GetReader();
                if (reader != null)
                {
                    readers.Add(reader);
                }
            }
            return readers;
        }

        /// <summary>
        /// 提供挂载在Unity脚本上,提供模组内容读取器;
        /// </summary>
        public interface IModContentReaderProvider
        {
            ModContentReader GetReader();
        }
    }
}
