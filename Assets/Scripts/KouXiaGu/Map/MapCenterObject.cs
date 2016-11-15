using System;
using System.Collections;
using UniRx;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 地图根据这个位置初始化(每个场景只存在一个!);
    /// </summary>
    [DisallowMultipleComponent]
    public class MapCenterObject : MonoBehaviour, IBuildGameInCoroutine, IQuitInCoroutine
    {
        private static MapCenterObject singleinstance;

        public static MapCenterObject GetInstance
        {
            get { return singleinstance; }
        }

        private IDisposable updateMapDisposable;

        private GameHexMap buildMap
        {
            get { return GameHexMap.GetInstance; }
        }

        private void Awake()
        {
            if (singleinstance != null)
            {
                Debug.LogWarning("存在两个更新地图组件!" + this);
                return;
            }

            singleinstance = this;
            Addend();
        }

        private void OnDestroy()
        {
            if (updateMapDisposable != null)
            {
                updateMapDisposable.Dispose();
            }
        }

        private void Addend()
        {
            IBuildGameData buildGameData = Initializers.BuildGameData;

            buildGameData.AppendBuildGame.Add(this);
            buildGameData.AppendQuitGame.Add(this);
        }

        /// <summary>
        /// 更新地图;
        /// </summary>
        private void UpdateMap(Vector3 position)
        {
            buildMap.UpdateMap(position);
        }

        IEnumerator ICoroutineInitialize<BuildGameData>.Initialize(
            BuildGameData item, ICancelable cancelable, Action<Exception> onError)
        {
            updateMapDisposable =  this.ObserveEveryValueChanged(_ => transform.position).
                Subscribe(UpdateMap);
            yield break;
        }

        IEnumerator ICoroutineInitialize<Unit>.Initialize(
            Unit item, ICancelable cancelable, Action<Exception> onError)
        {
            updateMapDisposable.Dispose();
            yield break;
        }

    }

}
