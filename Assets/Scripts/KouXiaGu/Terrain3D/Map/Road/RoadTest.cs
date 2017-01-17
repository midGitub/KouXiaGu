using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D
{


    public class RoadTest : MonoBehaviour
    {

        public Text textObject;

        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //RoadEdit edit = new RoadEdit(TerrainInitializer.Map);
            }
        }

    }

}
