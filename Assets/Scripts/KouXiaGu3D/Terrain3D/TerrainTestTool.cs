using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class TerrainTestTool : MonoBehaviour
    {

        [SerializeField, Range(0, 1)]
        float height = 0.5f;

        //void Update()
        //{
        //    Vector3 mousePoint = Camera.main.MouseToPixel();

        //}

    }

}
