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
            LandformManager.SaveLandforms();
        }

    }

}
