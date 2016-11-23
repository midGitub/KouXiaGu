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
        TopographiessData topographiessData;

        

        [ShowOnlyProperty]
        public bool IsReady { get; private set; }

        void Awake()
        {
          
        }

        void IFollowTargetMap.OnMapDataUpdate(Vector3 targetPlanePoint, IntVector2 targetMapPoint)
        {
            
        }

        IEnumerator IConstruct<BuildGameData>.Construction(BuildGameData item)
        {
           
            IsReady = true;
            yield break;
        }

    }

}
