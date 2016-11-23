using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World2D.Map;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class TopographyMap : UnitySingleton<TopographyMap>, IStartGameEvent, IFollowTargetMap
    {
        [SerializeField]
        ShortVector2 partitionSizes = new ShortVector2(10, 10);

        [SerializeField]
        TopographiessData topographiessData;
        [SerializeField]
        LoadBlockByRange loadyRange;
        ReadableBlockMap<TopographyNode, Block<TopographyNode>> blockMap;
        TopographyIO topographyIO;

        [ShowOnlyProperty]
        public bool IsReady { get; private set; }

        void Awake()
        {
            blockMap = new ReadableBlockMap<TopographyNode, Block<TopographyNode>>(partitionSizes);
            topographyIO = new TopographyIO();
        }

        void IFollowTargetMap.OnMapDataUpdate(Vector3 targetPlanePoint, IntVector2 targetMapPoint)
        {
            loadyRange.UpdateCenterPoint(targetMapPoint);
        }

        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
            topographyIO.BlockMap = blockMap.BlockMap;
            topographyIO.ReadOnlyWorldMap = WorldMap.GetInstance.Map;
            topographyIO.TopographyPrefabDictionary = topographiessData.TopographyPrefabDictionary;

            loadyRange.MapBlockIO = topographyIO;
            loadyRange.BlockMap = blockMap.BlockMap;

            IsReady = true;
            yield break;
        }

        [Serializable]
        private class LoadBlockByRange : LoadBlockByRange<Block<TopographyNode>> { }


    }

}
