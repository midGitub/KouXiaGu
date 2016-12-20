using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 游戏一开始运行;
    /// </summary>
    [DisallowMultipleComponent]
    public class Initial : MonoBehaviour
    {

        void Start()
        {
            InitialStage.Start();
        }

    }

}
