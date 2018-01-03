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
        public void GoMainMenuScene()
        {
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            Stage.GoMainMenuScene();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
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
