using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public class NodeEditLandform : MonoBehaviour
    {
        NodeEditLandform()
        {
        }

        public InputField landfromID;
        public InputField landfromName;
    }
}
