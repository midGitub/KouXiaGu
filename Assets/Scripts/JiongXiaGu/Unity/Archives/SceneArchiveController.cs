using JiongXiaGu.Resources;
using JiongXiaGu.Resources.Archives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.World.Archives
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
        public async Task WriteArchive(Archive archive)
        {
            if (IsWriting)
            {
                throw new ArgumentException("正在进行存档任务;");
            }

            Debug.Log("正在进行存档任务:" + archive.ToString());

            ISceneArchiveHandle[] archivers = GetComponentsInChildren<ISceneArchiveHandle>();
            List<Task> tasks = new List<Task>();
            tokenSource = new CancellationTokenSource();
            archive.Info = archive.Info.Update();
            Archive tempArchive = GetTempArchive(archive.Info);

            try
            {
                tempArchive.Create();

                foreach (var archiver in archivers)
                {
                    Task task = archiver.WriteArchive(tempArchive, tokenSource.Token);
                    if (task != null)
                    {
                        tasks.Add(task);
                    }
                }

                WriteArchiveTask = Task.WhenAll(tasks).ContinueWith(delegate (Task task)
                {
                    if (task.IsFaulted)
                    {
                        throw task.Exception;
                    }
                    else
                    {
                        tempArchive.MoveTo(archive);
                    }
                });
                await WriteArchiveTask;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("存档时发生错误:" + ex);
                tempArchive.Delete();
                throw ex;
            }
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
