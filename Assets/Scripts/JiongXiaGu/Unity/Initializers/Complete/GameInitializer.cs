using JiongXiaGu.Unity.GameConsoles;
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
        private RuntimeReflection runtimeReflection;

        private void Awake()
        {
            try
            {
                runtimeReflection = new RuntimeReflection();

                XiaGu.Initialize();
                Resource.Initialize();

                StartReflection();
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

        /// <summary>
        /// 开始进行反射内容;
        /// </summary>
        private Task StartReflection()
        {
            return Task.Run(delegate ()
            {
                runtimeReflection.ReflectionHandlers.Add(ConsoleMethodReflection.Default);
                runtimeReflection.Implement(typeof(GameInitializer).Assembly);
            });
        }
    }
}
