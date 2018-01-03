using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    [DisallowMultipleComponent]
    public sealed class StageEntrance : MonoBehaviour
    {
        /// <summary>
        /// 转到主菜单场景;
        /// </summary>
        public async void GoMainMenuScene()
        {
            await Stage.GoMainMenuScene();
        }

        /// <summary>
        /// 转到游戏场景;
        /// </summary>
        public void GoGameScene()
        {
            Stage.GoGameScene();
        }
    }
}
