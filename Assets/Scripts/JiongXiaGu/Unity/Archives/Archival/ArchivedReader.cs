using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 存档文件读写器;
    /// </summary>
    public class ArchivedReader
    {
        const string ArchiveDirectoryPrefix = "Map_";

        ArchiveDescriptionReader archiveDescriptionReader;

        public ArchivedReader()
        {
            archiveDescriptionReader = new ArchiveDescriptionReader();
        }

        static string ArchiveDirectorySearchPattern
        {
            get { return ArchiveDirectoryPrefix + "*"; }
        }

        /// <summary>
        /// 读取存档;
        /// </summary>
        public static Task<SceneArchivalData> Read(ArchiveInfo archive, IEnumerable<ISceneArchiveHandle> sceneArchiveHandles, CancellationToken cancellationToken)
        {
            return Task.Run(delegate ()
            {
                List<Task> list = new List<Task>();
                SceneArchivalData sceneArchivalData = new SceneArchivalData();
                foreach (var sceneArchiveHandle in sceneArchiveHandles)
                {
                    Task task = sceneArchiveHandle.Read(archive, sceneArchivalData, cancellationToken);
                    if (task != null)
                    {
                        list.Add(task);
                    }
                }
                Task.WaitAll(list.ToArray());
                return sceneArchivalData;
            });
        }

        /// <summary>
        /// 收集存档内容;
        /// </summary>
        public Task<SceneArchivalData> Collect(IEnumerable<ISceneArchiveHandle> sceneArchiveHandles, CancellationToken cancellationToken)
        {
            return Task.Run(delegate ()
            {
                foreach (var sceneArchiveHandle in sceneArchiveHandles)
                {
                    sceneArchiveHandle.Prepare(cancellationToken);
                }

                SceneArchivalData sceneArchivalData = new SceneArchivalData();
                List<Task> list = new List<Task>();
                foreach (var sceneArchiveHandle in sceneArchiveHandles)
                {
                    Task task = sceneArchiveHandle.Collect(sceneArchivalData, cancellationToken);
                    if (task != null)
                    {
                        list.Add(task);
                    }
                }

                Task.WaitAll(list.ToArray());
                return sceneArchivalData;
            });
        }

        /// <summary>
        /// 进行存档内容收集,并且进行输出;
        /// </summary>
        public async Task<SceneArchivalData> Write(ArchiveInfo archive, IEnumerable<ISceneArchiveHandle> sceneArchiveHandles, CancellationToken cancellationToken)
        {
            SceneArchivalData sceneArchivalData = await Collect(sceneArchiveHandles, cancellationToken);
            await Write(archive, sceneArchivalData, cancellationToken);
            return sceneArchivalData;
        }

        /// <summary>
        /// 输出存档;
        /// </summary>
        public Task Write(ArchiveInfo archive, SceneArchivalData sceneArchivalData, CancellationToken cancellationToken)
        {
            return Task.Run(delegate ()
            {
                //若存档已经存在,则备份现有存档;
                DirectoryInfo backupDirectory = null;
                if (archive.DirectoryInfo.Exists)
                {
                    backupDirectory = new DirectoryInfo(archive.ArchiveDirectory);
                    string backupDirectoryPath = archive.ArchiveDirectory + "_backup";
                    backupDirectory.MoveTo(backupDirectoryPath);
                }

                try
                {
                    archive.DirectoryInfo.Create();

                    List<Task> list = new List<Task>();
                    foreach (var archivalData in sceneArchivalData)
                    {
                        Task task = archivalData.Write(archive, cancellationToken);
                        if (task != null)
                        {
                            list.Add(task);
                        }
                    }

                    Task.WaitAll(list.ToArray());
                    archiveDescriptionReader.Write(archive.ArchiveDirectory, archive.Description);

                    backupDirectory?.Delete(true);
                }
                catch(Exception ex)
                {
                    archive.DirectoryInfo.Delete(true);
                    backupDirectory?.MoveTo(archive.ArchiveDirectory);
                    throw ex;
                }
            });
        }

        /// <summary>
        /// 迭代获取到所有存档文件;
        /// </summary>
        public IEnumerable<ArchiveInfo> EnumerateArchives(string directory, SearchOption searchOption)
        {
            foreach (var dir in Directory.EnumerateDirectories(directory, ArchiveDirectorySearchPattern, searchOption))
            {
                ArchiveInfo archiveInfo;
                try
                {
                    ArchiveDescription description = archiveDescriptionReader.Read(dir);
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    archiveInfo = new ArchiveInfo(description, directoryInfo);
                }
                catch
                {
                    continue;
                }
                yield return archiveInfo;
                archiveInfo = null;
            }
        }
    }
}
