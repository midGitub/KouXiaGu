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
    [DisallowMultipleComponent]
    public sealed class GameController : MonoBehaviour
    {

        public const string Tag = "GameController";

        void Awake()
        {
            this.tag = Tag;
            DontDestroyOnLoad(gameObject);
        }

        public static GameObject GetGameObject()
        {
            return GameObject.FindWithTag(Tag);
        }

    }

}
