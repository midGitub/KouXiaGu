using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 当地图发生变化,且已经渲染到场景,则重新渲染;
    /// </summary>
    [SerializeField]
    public class TerrainChangedCreater : SceneSington<TerrainChangedCreater>, ITerrainMapObserver
    {


        public static void Initialize(TerrainMap map)
        {
            map.Subscribe(GetInstance);
        }


        void IObserver<DictionaryChange<CubicHexCoord, TerrainNode>>.OnCompleted()
        {
            return;
        }

        void IObserver<DictionaryChange<CubicHexCoord, TerrainNode>>.OnError(Exception error)
        {
            return;
        }

        void IObserver<DictionaryChange<CubicHexCoord, TerrainNode>>.OnNext(DictionaryChange<CubicHexCoord, TerrainNode> value)
        {
            switch (value.Operation)
            {
                case Operation.Add:
                    //if(TerrainCreater.GetInstance.ReadOnlyOnSceneChunks.Contains())
                    break;

                case Operation.Remove:

                    break;

                case Operation.Update:

                    break;
            }
        }

    }

}
