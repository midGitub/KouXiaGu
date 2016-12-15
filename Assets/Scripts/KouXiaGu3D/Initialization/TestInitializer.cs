using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Initialization
{

    [DisallowMultipleComponent, CustomEditorTool]
    public class TestInitializer : MonoBehaviour, IStageObserver<Archiver>
    {

        public int time = 1000;

        [ShowOnlyProperty]
        Stages Stages
        {
            get { return Initializer.Stages; }
        }

        void Awake()
        {
            ArchiveStage.GetInstance.Subscribe(this);
        }

        [ContextMenu("初始化游戏;")]
        void ON_START()
        {
            Initializer.Add(StartStage.GetInstance);
        }

        [ContextMenu("进行游戏;")]
        void ON_GAME()
        {
            Initializer.Add(GameStage.GetInstance);
        }

        [ContextMenu("进行存档;")]
        void ON_SAVE()
        {
            ArchiveStage.Save();
        }

        IEnumerator IStageObserver<Archiver>.OnEnter(Archiver item)
        {
            while (time != 0)
            {
                time--;
                yield return null;
            }

            Directory.CreateDirectory(Path.Combine(item.DirectoryPath, "123"));
            yield break;
        }

        IEnumerator IStageObserver<Archiver>.OnLeave(Archiver item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IStageObserver<Archiver>.OnEnterRollBack(Archiver item)
        {
            Debug.Log("OnEnterRollBack");
            yield break;
        }

        IEnumerator IStageObserver<Archiver>.OnLeaveRollBack(Archiver item)
        {
            throw new NotImplementedException();
        }

        void IStageObserver<Archiver>.OnEnterCompleted()
        {
            Debug.Log("OnEnterCompleted");
        }

        void IStageObserver<Archiver>.OnLeaveCompleted()
        {
            throw new NotImplementedException();
        }

    }

}
