using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.Test
{


    public class TestTerrainHeight : MonoBehaviour
    {


        void Update()
        {
            transform.position = new Vector3(transform.position.x, Terrain3D.TerrainData.GetHeight(transform.position), transform.position.z);
        }


    }

}
