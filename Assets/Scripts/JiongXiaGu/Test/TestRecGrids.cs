using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.Grids;
using JiongXiaGu.Terrain3D;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Test
{

    [DisallowMultipleComponent]
    public class TestRecGrids : MonoBehaviour
    {

        [SerializeField]
        GameObject prefab;


        [ContextMenu("测试广度遍历1")]
        void TestBreadthTraversal1()
        {
            StartCoroutine(BreadthTraversal1());
        }

        IEnumerator BreadthTraversal1()
        {
            WaitForSeconds wait = new WaitForSeconds(1);
            foreach (var point in new BreadthTraversal().Traversal(RectCoord.Self, _ => false))
            {
                Instantiate(point);
                yield return wait;
            }
            Debug.Log("BreadthTraversal1 _Done");
        }


        [ContextMenu("测试广度遍历2")]
        void TestBreadthTraversal2()
        {
            StartCoroutine(BreadthTraversal2());
        }

        /// <summary>
        /// 不显示 x轴为奇数 而且 y轴不为2的;
        /// </summary>
        IEnumerator BreadthTraversal2()
        {
            WaitForSeconds wait = new WaitForSeconds(1);
            foreach (var point in new BreadthTraversal().Traversal(RectCoord.Self, point => (point.x & 1) == 1 && point.y != 2))
            {
                Instantiate(point);
                yield return wait;
            }
            Debug.Log("BreadthTraversal2 _Done");
        }


        void Instantiate(RectCoord position)
        {
            Vector3 pos = new Vector3(position.x, 0, position.y);
            var gObject = Instantiate(prefab, pos, prefab.transform.rotation, this.transform) as GameObject;
            gObject.SetActive(true);
            Text textObject = gObject.GetComponentInChildren<Text>();
            textObject.text = position.ToString();
        }

       

    }

}
