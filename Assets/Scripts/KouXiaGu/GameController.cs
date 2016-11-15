using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏控制器;
    /// </summary>
    public static class GameController
    {

        public const string Tag = "GameController";

        public static T FindInstance<T>()
        {
            return GameObject.FindWithTag(Tag).GetComponent<T>();
        }

    }

}
