using JiongXiaGu.Unity.GameConsoles;
using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Collections;
using System.Threading;

namespace JiongXiaGu.Unity.Initializers
{

    /// <summary>
    /// 游戏程序初始化器;
    /// </summary>
    [DisallowMultipleComponent]
    internal class GameInitializer : MonoBehaviour
    {
        private GameInitializer()
        {
        }

        private const string InitializerName = "程序初始化";

        private void Awake()
        {
            try
            {
                XiaGu.Initialize();
                ResourcePath.Initialize();
                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnCompleted()
        {
            EditorHelper.LogComplete(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            EditorHelper.LogFault(InitializerName, ex);
        }
    }
}
