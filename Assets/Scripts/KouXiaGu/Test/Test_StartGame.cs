using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace KouXiaGu.Test
{

    [DisallowMultipleComponent]
    public class Test_StartGame : MonoBehaviour
    {
        [SerializeField]
        private Button startGame;
        [SerializeField]
        private Button saveGame;
        [SerializeField]
        private Button quitGame;

        [SerializeField]
        private Initializers initializers;

        [SerializeField]
        private InputField mapDir;

        private void Start()
        {
            startGame.onClick.AsObservable().Subscribe(OnStartGame);
            saveGame.onClick.AsObservable().Subscribe(OnSaveGame);
            quitGame.onClick.AsObservable().Subscribe(OnQuit);
        }

        private void OnStartGame(UniRx.Unit unit)
        {
            BuildGameData buildGameData = initializers.DataGame.GetBuildGameData();
            buildGameData.ArchivedData.Archived.World2D.PathPrefabMapDirectory = mapDir.text;
            initializers.Build(buildGameData);
        }

        private void OnSaveGame(UniRx.Unit unit)
        {
            var saveGameData = initializers.DataGame.ArchiveData.CreateArchived();
            initializers.Save(saveGameData);
        }

        private void OnQuit(UniRx.Unit unit)
        {
            initializers.Quit();
        }

    }

}
