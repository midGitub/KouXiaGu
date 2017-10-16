using JiongXiaGu.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Unity.Initializers;

namespace JiongXiaGu.Unity.Resources
{

    [Flags]
    public enum ReadModOptions
    {
        /// <summary>
        /// 可选择的内容,如 游戏语言包(非模组语言补充包),游戏地图文件;
        /// </summary>
        OptionalContent = 1,

        /// <summary>
        /// 资源,如地形贴图定义,建筑模型定义;
        /// </summary>
        Resources = 2,
    }

    /// <summary>
    /// 模组资源读取器接口;
    /// </summary>
    public interface IModReadHandle
    {
        /// <summary>
        /// 读取模组资源;
        /// </summary>
        void Read(ModResource modResource, ReadModOptions options);
    }

    /// <summary>
    /// 获取到模组读取器(挂载到Unity物体);
    /// </summary>
    [DisallowMultipleComponent]
    internal class ModReadLeader : MonoBehaviour
    {
        private static IModReadHandle[] modResourceHandles;

        private void Awake()
        {
            modResourceHandles = GetComponentsInChildren<IModReadHandle>();
        }

        private void OnDestroy()
        {
            modResourceHandles = null;
        }

        /// <summary>
        /// 获取到所有读取器;
        /// </summary>
        public static IReadOnlyCollection<IModReadHandle> GetReadHandles()
        {
            if (modResourceHandles == null)
            {
                throw new MissingComponentException(string.Format("场景未挂载[{0}]脚本,或尝试在{0}组件初始化之前获取读取器;", nameof(ModReadLeader)));
            }
            return modResourceHandles;
        }
    }

    /// <summary>
    /// 模组资源读取(非线程安全的);
    /// </summary>
    internal static class ModReader
    {
        /// <summary>
        /// 读取到模组资源;
        /// </summary>
        public static ModResource Read(ModInfo modInfo, ReadModOptions options, CancellationToken token)
        {
            var readHandles = GetReadHandles();
            ModResource modResource = new ModResource(modInfo);

            foreach (var readHandle in readHandles)
            {
                readHandle.Read(modResource, options);
            }

            return modResource;
        }

        public static ModResource Read(ModResource modResource, ReadModOptions options, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private static IReadOnlyCollection<IModReadHandle> GetReadHandles()
        {
            return ModReadLeader.GetReadHandles();
        }
    }
}
