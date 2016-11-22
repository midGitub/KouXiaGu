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
        private Button continueGame;
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

        private void Awake()
        {
            initializers = GameObject.FindObjectOfType<Initializers>();
        }

        private void Start()
        {
            continueGame.onClick.AsObservable().Subscribe(OnContinueGame);
            startGame.onClick.AsObservable().Subscribe(OnStartGame);
            saveGame.onClick.AsObservable().Subscribe(OnSaveGame);
            quitGame.onClick.AsObservable().Subscribe(OnQuit);
        }

        private void OnContinueGame(UniRx.Unit unit)
        {
            BuildGameData buildGameData = ResGame.GetRecentBuildGameData();
            buildGameData.ArchivedData.Archived.World2D.PathPrefabMapDirectory = mapDir.text;
            initializers.Build(buildGameData);
        }

        private void OnStartGame(UniRx.Unit unit)
        {
            BuildGameData buildGameData = ResGame.GetBuildGameData();
            buildGameData.ArchivedData.Archived.World2D.PathPrefabMapDirectory = mapDir.text;
            initializers.Build(buildGameData);
        }

        private void OnSaveGame(UniRx.Unit unit)
        {
            var saveGameData = ResArchiver.CreateArchived();
            initializers.Save(saveGameData, () => ResArchiver.SaveInDisk(saveGameData));
        }

        private void OnQuit(UniRx.Unit unit)
        {
            initializers.Quit(new QuitGameData());
        }

    }

}
