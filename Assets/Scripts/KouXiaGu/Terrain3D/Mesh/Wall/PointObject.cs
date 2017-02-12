using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [SelectionBase]
    public class PointObject : MonoBehaviour
    {
        PointObject()
        {
        }

        [SerializeField]
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

    }

}
