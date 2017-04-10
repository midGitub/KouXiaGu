using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.World.Map
{

    /// <summary>
    /// 地图存在的元素;
    /// </summary>
    public class MapElement
    {

        internal Dictionary<int, RoadInfo> RoadTypes { get; private set; }
        internal Dictionary<int, LandformInfo> LandformTypes { get; private set; }

        public static MapElement Create()
        {
            throw new NotImplementedException();
        }

        public static IAsync<MapElement> CreateAsync()
        {
            throw new NotImplementedException();
        }

        MapElement()
        {
        }

        class AsyncInitializer : MonoAsyncOperation<MapElement>
        {
            AsyncInitializer()
            {
            }

            void Start()
            {
                StartCoroutine(Initialize());
            }

            IEnumerator Initialize()
            {
                var terrainRequest = TerrainAssetBundle.LoadAsync();
                yield return terrainRequest;
                AssetBundle terrain = terrainRequest.assetBundle;

                throw new NotImplementedException();
            }

        }

        class RoadReader
        {
            public static IReader<RoadInfo[]> InfoReader { get; set; }

            static RoadReader()
            {
                InfoReader = new XmlRoadInfoReader();
            }


        }

        class LandformReader
        {
            public static IReader<LandformInfo[]> InfoReader { get; set; }
            public static IReader<LandformType, IEnumerable<LandformInfo>> TypeReader { get; set; }

            static LandformReader()
            {

            }
        }

    }

}
