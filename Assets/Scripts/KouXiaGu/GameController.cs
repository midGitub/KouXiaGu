using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
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
