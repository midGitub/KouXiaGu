using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
                Resource.Initialize();

                OnCompleted();
            }
            catch (Exception ex)
            {
                OnFaulted(ex);
            }
        }

        private void OnCompleted()
        {
            InitializerHelper.LogComplete(InitializerName);
        }

        private void OnFaulted(Exception ex)
        {
            InitializerHelper.LogFault(InitializerName, ex);
        }
    }
}
