using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.Test
{

    [DisallowMultipleComponent]
    public class TestRecGrids : MonoBehaviour
    {

        [SerializeField]
        GameObject prefab;


        [ContextMenu("测试广度遍历")]
        void TestBreadthTraversal()
        {
            StartCoroutine(BreadthTraversal());
        }

        IEnumerator BreadthTraversal()
        {
            foreach (var point in ShortVector2.Zero.BreadthTraversal(_ => true))
            {
                Instantiate(point);
                yield return new WaitForSeconds(2);
            }
        }

        void Instantiate(ShortVector2 position)
        {
            
        }

    }

}
