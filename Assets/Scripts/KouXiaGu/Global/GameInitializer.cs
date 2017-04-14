using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Globalization;
using KouXiaGu.KeyInput;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 负责对游戏资源初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public class GameInitializer : MonoBehaviour
    {
        GameInitializer()
        {
        }

        public GameData Data { get; private set; }

        /// <summary>
        /// 对内容进行初始化;
        /// </summary>
        void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            CustomInput.ReadAsync().Subscribe(_ => LogEmptyKeys(), OnFaulted);

            InitLanguage();
            InitGameData();
        }

        void OnFaulted(IAsyncOperation operation)
        {
            Debug.LogError(operation.Exception);
        }

        /// <summary>
        /// 输出未定义的按键;
        /// </summary>
        void LogEmptyKeys()
        {
            var emptyKeys = CustomInput.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                Debug.LogWarning("未定义的按键:" + emptyKeys.ToLog());
            }
        }



        /// <summary>
        /// 初始化语言模块;
        /// </summary>
        void InitLanguage()
        {
            var operater = Localization.ReadAsync();
        }

        void InitGameData()
        {
            var operater = GameData.Create();
            operater.Subscribe(completed => Debug.Log(completed.Result.ToLog()), f => Debug.LogError(f.Exception));
        }

    }

}
