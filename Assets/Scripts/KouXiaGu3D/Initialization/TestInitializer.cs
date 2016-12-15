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
    public class TestInitializer : MonoBehaviour, IPreservable
    {

        public int time = 1000;

        [ShowOnlyProperty]
        GameStages Stages
        {
            get { return Initializer.Stages; }
        }

        void Awake()
        {
            ArchiveStages.Subscribe(this);
        }

        [ContextMenu("进行存档;")]
        void ON_SAVE()
        {
            ArchiveStages.Save();
        }

        public IEnumerator OnSave(Archiver archive)
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
