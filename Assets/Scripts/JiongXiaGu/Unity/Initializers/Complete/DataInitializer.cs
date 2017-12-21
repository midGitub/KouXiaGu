using JiongXiaGu.Collections;
using JiongXiaGu.Unity.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 游戏数据初始化接口;
    /// </summary>
    public interface IDataInitializeHandle
    {
        /// <summary>
        /// 读取到对应内容;
        /// </summary>
        void Read(LoadableContent loadableContent, ITypeDictionary info, CancellationToken token);

        /// <summary>
        /// 准备内容;
        /// </summary>
        void Prepare(ITypeDictionary info, CancellationToken token);

        /// <summary>
        /// 清除所有内容;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 游戏数据初始化器(在游戏开始时进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    public class DataInitializer : MonoBehaviour
    {
        private static readonly GlobalSingleton<DataInitializer> singleton = new GlobalSingleton<DataInitializer>();
        public static DataInitializer Instance => singleton.GetInstance();

        private const string InitializerName = "游戏组件初始化";
        private IDataInitializeHandle[] initializeHandles;

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IDataInitializeHandle>();
        }

        /// <summary>
        /// 开始读取,若已经在读取则返回异常;
        /// </summary>
        public Task StartLoad(LoadOrder loadOrder)
        {
            throw new NotImplementedException();
        }
    }
}
