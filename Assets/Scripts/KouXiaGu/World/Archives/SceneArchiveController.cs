using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.Resources.Archives;
using System.Threading;

namespace KouXiaGu.World.Archives
{

    /// <summary>
    /// 挂载在场景中,控制存档输出输入;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class SceneArchiveController : MonoBehaviour
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
            {
                throw new ArgumentException("正在进行存档任务;");
            }

            archive.Create();
            ISceneArchiveHandle[] archivers = GetComponentsInChildren<ISceneArchiveHandle>();
            List<Task> tasks = new List<Task>();
            tokenSource = new CancellationTokenSource();

            foreach (var archiver in archivers)
            {
                Task task = archiver.WriteArchive(archive, tokenSource.Token);
                if (task != null)
                {
                    tasks.Add(task);
                }
            }

            return WriteArchiveTask = Task.WhenAll(tasks);
        }

        /// <summary>
        /// 取消当前存档输出操作,若不存在则无效果;
        /// </summary>
        public void CanceleWrite()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
        }
    }
}
