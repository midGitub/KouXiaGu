using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace KouXiaGu.Terrain3D
{


    public class Test_Terrain : MonoBehaviour
    {

        [ContextMenu("Serialize")]
        void Serialize()
        {
            Landform[] Landforms = new Landform[]
            {
                new Landform(10),
                new Landform(20),
                new Landform(90),
            };
            LandformManager.Serialize(Landforms);
        }

        [ContextMenu("Deserialize")]
        void Deserialize()
        {
            IEnumerable<Landform> Landforms = LandformManager.Deserialize();

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
                Debug.Log("Done:" + LandformManager.GetInstance.Count);
                foreach (var item in LandformManager.GetInstance.initializedLandforms)
                {
                    Debug.Log(item.ToString());
                }
            };

            Observable.FromCoroutine(LandformManager.GetInstance.Initialize).Subscribe(null, done);
        }



    }

}
