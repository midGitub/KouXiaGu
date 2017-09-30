using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Resources.Archives;
using System;
using System.Collections.Generic;
using System.IO;
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
        /// 存档输出的异步操作;
        /// </summary>
        internal Task WriteArchiveTask { get; private set; }

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

        /// <summary>
        /// 将存档内容输出到存档目录(仅在Unity线程调用);
        /// </summary>
        public Task WriteArchive(Archive archive)
        {
            if (IsWriting)
                throw new ArgumentException("正在进行存档任务;");

            Debug.Log("正在进行存档任务:" + archive.ToString());

            ISceneArchiveHandle[] archivers = GetComponentsInChildren<ISceneArchiveHandle>();
            tokenSource = new CancellationTokenSource();
            archive.Info = archive.Info.Update();
            Archive tempArchive = GetTempArchive(archive.Info);

            WriteArchiveTask = Task.Run(delegate ()
            {
                try
                {
                    tempArchive.Create();

                    //通知开始进行存档操作;
                    foreach (var archiver in archivers)
                    {
                        archiver.Prepare(tokenSource.Token);
                    }

                    //准备存档内容;
                    foreach (var archiver in archivers)
                    {
                        archiver.Begin();
                    }

                    var tasks = new List<Task>();
                    //输出存档内容;
                    foreach (var archiver in archivers)
                    {
                        Task task = archiver.Write(tempArchive);
                        if (task != null)
                        {
                            tasks.Add(task);
                        }
                    }
                    Task.WaitAll(tasks.ToArray());
                    tempArchive.MoveTo(archive);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("存档时发生错误:" + ex);
                    tokenSource.Cancel();
                    tempArchive.Delete();
                    throw ex;
                }
            }, tokenSource.Token);
            return WriteArchiveTask;
        }

        /// <summary>
        /// 获取到临时存档目录;
        /// </summary>
        Archive GetTempArchive(ArchiveInfo info)
        {
            string tempArchivePath = Path.Combine(Resource.UserConfigDirectoryPath, "TempSave");
            var tempArchive = new Archive(tempArchivePath, info);
            return tempArchive;
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
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
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
