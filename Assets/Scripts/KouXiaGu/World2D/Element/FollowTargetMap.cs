using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 更随目标动态加载的地图;
    /// </summary>
    public interface IFollowTargetMap
    {
        /// <summary>
        /// 是否准备完成?;
        /// </summary>
        bool IsReady { get; }

        /// <summary>
        /// 更新地图数据;
        /// </summary>
        void OnMapDataUpdate(Vector3 targetPlanePoint, IntVector2 targetMapPoint);
    }

    /// <summary>
    /// 驱动挂载在同一物体上的地图组件;
    /// </summary>
    [DisallowMultipleComponent]
    public class FollowTargetMap : MonoBehaviour, IStartGameEvent
    {

        [SerializeField]
        Transform target;

        IFollowTargetMap[] followTargetMaps;

        public event Action<Vector3, IntVector2> UpdateAction;

        void Awake()
        {
            followTargetMaps = GetComponents<IFollowTargetMap>();
            SetUpdateAction(followTargetMaps);
        }

        bool IsAllReady()
        {
            var notReadyMap = followTargetMaps.FirstOrDefault(item => item.IsReady == false);
            return notReadyMap == null;
        }

        void SetUpdateAction(IFollowTargetMap[] followTargetMaps)
        {
            foreach (var followTargetMap in followTargetMaps)
            {
                UpdateAction += followTargetMap.OnMapDataUpdate;
            }
        }

        void OnMapDataUpdate(Vector3 targetPlanePoint)
        {
            IntVector2 targetMapPoint = WorldConvert.PlaneToHexPair(targetPlanePoint);
            if (UpdateAction != null)
            {
                UpdateAction(targetPlanePoint, targetMapPoint);
            }
        }

        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            while (!IsAllReady())
            {
                yield return null;
            }

            target.ObserveEveryValueChanged(_ => target.position).
                Subscribe(OnMapDataUpdate);

        }
    }

}
