using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;

namespace KouXiaGu.Initialization
{

    [DisallowMultipleComponent, CustomEditorTool]
    public class InitializerTest : MonoBehaviour, IStageObserver<ArchiveFile>
    {

        public int time = 1000;

        [ShowOnlyProperty]
        Stages Stages
        {
            get { return Initializer.Stages; }
        }

        void Awake()
        {
            ArchiveStage.Subscribe(this);
        }

        [ContextMenu("初始化游戏;")]
        void ON_START()
        {
            InitialStage.Start();
        }

        [ContextMenu("进行游戏;")]
        void ON_GAME()
        {
            GameStage.Start(new ArchiveFile());
        }

        [ContextMenu("进行存档;")]
        void ON_SAVE()
        {
            ArchiveStage.Start();
        }

        IEnumerator IStageObserver<ArchiveFile>.OnEnter(ArchiveFile item)
        {
            while (time != 0)
            {
                time--;
                yield return null;
            }
            Directory.CreateDirectory(Path.Combine(item.DirectoryPath, "123"));
            yield break;
        }

        IEnumerator IStageObserver<ArchiveFile>.OnLeave(ArchiveFile item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IStageObserver<ArchiveFile>.OnEnterRollBack(ArchiveFile item)
        {
            Debug.Log("OnEnterRollBack");
            yield break;
        }

        IEnumerator IStageObserver<ArchiveFile>.OnLeaveRollBack(ArchiveFile item)
        {
            throw new NotImplementedException();
        }

        void IStageObserver<ArchiveFile>.OnEnterCompleted()
        {
            Debug.Log("OnEnterCompleted");
        }

        void IStageObserver<ArchiveFile>.OnLeaveCompleted()
        {
            throw new NotImplementedException();
        }

    }

}
