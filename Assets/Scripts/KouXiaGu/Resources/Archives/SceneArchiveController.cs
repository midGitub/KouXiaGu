using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Resources.Archives
{

    public interface ISceneArchiver
    {
        /// <summary>
        /// 输出存档内容;
        /// </summary>
        Task WriteArchive(Archive archive);
    }

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
        /// 是否正在输出存档?
        /// </summary>
        public bool IsWriting { get; private set; }

        public async Task WriteArchive(Archive archive)
        {
            if (IsWriting)
                throw new ArgumentException("正在进行存档任务;");

            ISceneArchiver[] archivers = GetComponentsInChildren<ISceneArchiver>();
            List<Task> tasks = new List<Task>();
            foreach (var archiver in archivers)
            {
                Task task = archiver.WriteArchive(archive);
                if (task != null)
                {
                    tasks.Add(task);
                }
            }

            var waiter = Task.WhenAll(tasks);
            await waiter;
        }
    }
}
