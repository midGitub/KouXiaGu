using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain
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
            LandformManager.SaveLandforms(Landforms);
        }

        [ContextMenu("Deserialize")]
        void Deserialize()
        {
            Landform[] Landforms = LandformManager.LoadLandforms();

            foreach (var Landform in Landforms)
            {
                Debug.Log(Landform.ToString());
            }

        }

    }

}
