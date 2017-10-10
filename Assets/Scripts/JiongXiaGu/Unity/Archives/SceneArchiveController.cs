using JiongXiaGu.Unity.Initializers;
using JiongXiaGu.Unity.Resources;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 挂载在场景中,控制存档输出输入;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SceneArchiveController : MonoBehaviour, IQuitSceneHandle
    {
        SceneArchiveController()
        {
        }

        /// <summary>
        /// 用户存档存放目录;
        /// </summary>
        [PathDefinition(ResourceTypes.ArchiveDirectory, "用户存档存放目录;")]
        internal const string SavesDirectoryName = "Saves";

        /// <summary>
        /// 存档输出的异步操作;
        /// </summary>
        internal Task WriteArchiveTask { get; private set; }

        ArchivedReader archivedReader;

        /// <summary>
        /// 当前异步操作取消传送器;
        /// </summary>
        CancellationTokenSource tokenSource;

        /// <summary>
        /// 是否正在输出存档?
        /// </summary>
        public bool IsWriting
        {
            get { return WriteArchiveTask != null ? !WriteArchiveTask.IsCompleted : false; }
        }

        void Awake()
        {
            archivedReader = new ArchivedReader();
        }

        /// <summary>
        /// 将存档内容输出到存档目录(仅在Unity线程调用);
        /// </summary>
        public async Task WriteArchive(Archive archive)
        {
            if (IsWriting)
                throw new ArgumentException("正在进行存档任务;");

            Debug.Log("正在进行存档任务:" + archive.ToString());

            ISceneArchiveHandle[] archivers = GetComponentsInChildren<ISceneArchiveHandle>();
            tokenSource = new CancellationTokenSource();
            SceneArchivalData sceneArchivalData = await archivedReader.Collect(archivers, tokenSource.Token);
            await archivedReader.Write(archive, sceneArchivalData, tokenSource.Token);
        }

        void OnDestroy()
        {
            Cancele();
        }

        /// <summary>
        /// 取消当前存档输出操作,若不存在则无效果;
        /// </summary>
        public void Cancele()
        {
            tokenSource?.Cancel();
        }

        /// <summary>
        /// 等待存档保存完成才允许退出;
        /// </summary>
        Task IQuitSceneHandle.OnQuitScene()
        {
            return WriteArchiveTask;
        }
    }
}
