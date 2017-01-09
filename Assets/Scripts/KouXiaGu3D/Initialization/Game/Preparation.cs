using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    [DisallowMultipleComponent]
    public sealed class Preparation : MonoBehaviour
    {

        static Preparation()
        {
            IsPreparation = false;
        }


        public static bool IsPreparation { get; private set; }


        static IEnumerator StageStart()
        {
            yield return StageStartAction();
            IsPreparation = true;
        }

        static IEnumerator StageStartAction()
        {
            return TerrainInitializer.PreparationStart();
        }


        void Start()
        {
            StartCoroutine(StageStart());
        }

    }

}
