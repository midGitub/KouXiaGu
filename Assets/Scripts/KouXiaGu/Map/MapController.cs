using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 游戏地图,采用六边形的地图结构;
    /// </summary>
    public class MapController : MonoBehaviour
    {

        [SerializeField]
        Vector2[] ppap;

        [ContextMenu("Test")]
        public void Test()
        {
            Beehive Beehive = new Beehive(1200f);

            foreach (var item in ppap)
            {
                var vv = Beehive.WorldToNumber(item);

                Debug.Log(vv.Key.ToString() + vv.Value);
                //Debug.Log(/*"1" + dd.PointFind(item) + "\n2" +*/ dd.PointFind2(item));
                //dd.PointFind2(item);
                //Debug.Log(dd.WorldToMap(item));
            }

        }

    }

}
