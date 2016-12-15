using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    [DisallowMultipleComponent, CustomEditorTool]
    public class TestInitializer : MonoBehaviour, IStageEnter<Archiver>
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


        public IEnumerator OnEnter(Archiver archive)
        {
            while (time != 0)
            {
                time--;
                yield return null;
            }

            Directory.CreateDirectory(Path.Combine(archive.DirectoryPath, "123"));
            yield break;
        }

    }

}
