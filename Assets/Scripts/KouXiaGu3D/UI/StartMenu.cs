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

        [SerializeField]
        Button quitGame;

        void Start()
        {
            startGame.OnClickAsObservable().
                Subscribe(_ => GameStage.Start(ArchiveTemplet.DefaultArchived));

            returnStart.OnClickAsObservable().
                Subscribe(_ => GameStage.End());

            quitGame.OnClickAsObservable().
                Subscribe(_AppDomain => Application.Quit());

            return;
        }


    }

}
