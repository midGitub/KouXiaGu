using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 游戏数据初始化接口;
    /// </summary>
    public interface IDataInitializeHandle
    {
        /// <summary>
        /// 进行初始化,对于可进行异步的工作,通过异步Task进行;
        /// </summary>
        Task Initialize(CancellationToken token);
    }

    /// <summary>
    /// 游戏数据初始化器(在游戏开始时进行初始化,若初始化失败意味着游戏无法开始);
    /// </summary>
    [DisallowMultipleComponent]
    public class DataInitializeScheduler : MonoBehaviour
    {
        private static readonly GlobalSingleton<DataInitializeScheduler> singleton = new GlobalSingleton<DataInitializeScheduler>();

        private const string InitializerName = "游戏组件初始化";
        private IDataInitializeHandle[] initializeHandles;
        private CancellationTokenSource initializeCancellation;
        public Task InitializeTask { get; private set; }

        public static DataInitializeScheduler Instance
        {
            get { return singleton.GetInstance(); }
        }

        /// <summary>
        /// 初始化内容数目,若还未初始化则返回 -1;
        /// </summary>
        public int InitializeHandleCount
        {
            get { return initializeHandles != null ? initializeHandles.Length : -1; }
        }

        private void Awake()
        {
            singleton.SetInstance(this);
            initializeHandles = GetComponentsInChildren<IDataInitializeHandle>();
            initializeCancellation = new CancellationTokenSource();
        }
    }
}
