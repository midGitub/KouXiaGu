using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.KeyInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KouXiaGu
{

    /// <summary>
    /// 负责游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        GameManager()
        {
        }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Start()
        {
            InitCustomInput();

        }

        /// <summary>
        /// 初始化自定义按键信息;
        /// </summary>
        void InitCustomInput()
        {
            CustomInput.ReadFromFile();

            var emptyKeys = CustomInput.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                Debug.LogWarning("未定义的按键:" + emptyKeys.ToLog());
            }
        }

    }

}
