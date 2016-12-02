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
            LandformManager.Save(Landforms);
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
            LandformManager.Append(Landforms);
        }

        [ContextMenu("Deserialize")]
        void Deserialize()
        {
            IEnumerable<Landform> Landforms = LandformManager.Load();

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
                Debug.Log("Done:" + LandformManager.InitializedCount);
                foreach (var item in LandformManager.InitializedLandforms)
                {
                    Debug.Log(item.ToString());
                }
            };

            Observable.FromCoroutine(LandformManager.Initialize).Subscribe(null, done);
        }



    }

}
