using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 续地形烘培完毕后,在地形上放置建筑模型;
    /// </summary>
    public sealed class Architect : SceneSington<Architect>
    {
        Architect()
        {
        }

        /// <summary>
        /// 是否已经初始化?
        /// </summary>
        public static bool IsInitialised { get; private set; }

        /// <summary>
        /// 取首个请求进行放置;
        /// </summary>
        public static LinkedList<IBuildRequest> Requested
        {
            get { return GetInstance.requestQueue; }
        }

        /// <summary>
        /// 初始化;
        /// </summary>
        public static void Initialize()
        {
            if (!IsInitialised)
            {
                Architect instance = GetInstance;
                instance.StartCoroutine(instance.ProcessRequests());
                IsInitialised = true;
            }
        }


        [SerializeField]
        BuildingArchitect building;

        LinkedList<IBuildRequest> requestQueue;

        protected override void Awake()
        {
            base.Awake();
            requestQueue = new LinkedList<IBuildRequest>();
        }

        IEnumerator ProcessRequests()
        {
            CustomYieldInstruction bakingYieldInstruction = new WaitWhile(() => requestQueue.Count == 0);

            while (true)
            {
                yield return bakingYieldInstruction;

                IBuildRequest request = null;

                try
                {
                    request = Dequeue();
                    var overlayes = GetOverlaye(request.ChunkCoord);

                    var buildings = building.Build(request.Data.Building, overlayes);

                    request.OnComplete(new BuildingGroup(request, buildings));
                }
                catch (Exception e)
                {
                    if (request != null)
                        request.OnError(e);
                }
            }
        }

        /// <summary>
        /// 获取到首个请求,并且移除;
        /// </summary>
        IBuildRequest Dequeue()
        {
            LinkedListNode<IBuildRequest> first = requestQueue.First;
            requestQueue.Remove(first);
            IBuildRequest value = first.Value;
            return value;
        }

        /// <summary>
        /// 获取到覆盖的坐标;
        /// </summary>
        IEnumerable<CubicHexCoord> GetOverlaye(RectCoord coord)
        {
            return TerrainOverlayer.GetBuilding(coord);
        }

    }

}
