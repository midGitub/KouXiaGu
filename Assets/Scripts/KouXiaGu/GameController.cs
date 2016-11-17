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

        public static GameObject GameObject
        {
            get { return GameObject.FindWithTag(Tag); }
        }

        public static GameObject GetGameController(this UnityEngine.Object unityEngine)
        {
            return GameObject;
        }

        public static T FindInstance<T>()
        {
            return GameObject.GetComponent<T>();
        }

        public static T FindInstance<T>(this UnityEngine.Object unityEngine)
        {
            return GameObject.GetComponent<T>();
        }

    }

}
