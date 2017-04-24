using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class LandformBakeQueue : MonoBehaviour
    {
        LandformBakeQueue()
        {
        }

        [SerializeField]
        Stopwatch runtimeStopwatch;
        CoroutineQueue<IEnumerator> requestQueue;

        void Awake()
        {
            requestQueue = new CoroutineQueue<IEnumerator>(runtimeStopwatch);
        }

        void Update()
        {
            requestQueue.Next();
        }

    }

}
