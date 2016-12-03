using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace KouXiaGu.Terrain
{


    public class Test_Terrain : MonoBehaviour
    {

        [ContextMenu("Serialize")]
        void Serialize()
        {
            List<Landform> Landforms = new List<Landform>()
            {
                new Landform(10),
                new Landform(20),
                new Landform(90),
            };
            LandformInit.Save(Landforms);
        }

        [ContextMenu("Append")]
        void Append()
        {
            List<Landform> Landforms = new List<Landform>()
            {
                new Landform(10),
                new Landform(20),
                new Landform(90),
            };
            LandformInit.Append(Landforms);
        }

        [ContextMenu("Deserialize")]
        void Deserialize()
        {
            IEnumerable<Landform> Landforms = LandformInit.Load();

            foreach (var Landform in Landforms)
            {
                Debug.Log(Landform.ToString());
            }

        }

        [ContextMenu("Load")]
        void Test_Load()
        {

            Action done = delegate ()
            {
                Debug.Log("Done:" + LandformInit.InitializedCount);
                foreach (var item in LandformInit.InitializedLandforms)
                {
                    Debug.Log(item.ToString());
                }
            };

            Observable.FromCoroutine(LandformInit.Initialize).Subscribe(null, done);
        }



    }

}
