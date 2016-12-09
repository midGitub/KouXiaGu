using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.HexTerrain;
using UnityEngine;

namespace KouXiaGu.Test
{


    public class TestTerrainHeight : MonoBehaviour
    {


        void Update()
        {
            transform.position = new Vector3(transform.position.x, TerrainBlock.GetHeight(transform.position), transform.position.z);
        }


    }

}
