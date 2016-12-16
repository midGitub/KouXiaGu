using System.Collections;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainStageObserver : MonoBehaviour
    {

        void Awake()
        {
            InitialStage.Subscribe(LandformInit.instance);
        }


        class LandformInit : IStageObserver<object>
        {
            public static readonly LandformInit instance = new LandformInit();

            IEnumerator IStageObserver<object>.OnEnter(object item)
            {
                return Landform.Initialize();
            }

            void IStageObserver<object>.OnEnterCompleted()
            {
                return;
            }

            IEnumerator IStageObserver<object>.OnEnterRollBack(object item)
            {
                return Landform.ClearRes();
            }

            IEnumerator IStageObserver<object>.OnLeave(object item)
            {
                return Landform.ClearRes();
            }

            void IStageObserver<object>.OnLeaveCompleted()
            {
                return;
            }

            IEnumerator IStageObserver<object>.OnLeaveRollBack(object item)
            {
                yield break;
            }

        }

    }

}
