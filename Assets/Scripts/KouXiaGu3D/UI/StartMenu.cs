using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using KouXiaGu.Initialization;

namespace KouXiaGu.UI
{


    public class StartMenu : MonoBehaviour
    {
        [SerializeField]
        Button startGame;

        [SerializeField]
        Button returnStart;

        void Start()
        {
            startGame.OnClickAsObservable().
                Subscribe(_ => GameStage.Start(ArchiveTemplet.DefaultArchived));

            returnStart.OnClickAsObservable().
                Subscribe(_ => GameStage.End());

            return;
        }


    }

}
