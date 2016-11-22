using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [Serializable]
    public class TopographyCreater : IObserver<KeyValuePair<IntVector2, WorldNode>>
    {


        public TopographiessData TopographiessData { get; set; }

        public void OnNext(KeyValuePair<IntVector2, WorldNode> value)
        {
            Transform prefab = TopographiessData.GetWithID(value.Value.Topography).prefab;
            Vector2 planePoint = WorldConvert.MapToHex(value.Key);
            GameObject.Instantiate(prefab, planePoint, new Quaternion());
        }

        public void OnCompleted()
        {
            return;
        }

        public void OnError(Exception error)
        {
            return;
        }

    }

}
