using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Globalization;
using KouXiaGu.KeyInput;
using UnityEngine;
using System.Threading;
using KouXiaGu.Concurrent;

namespace KouXiaGu
{

    /// <summary>
    /// 组建初始化;
    /// </summary>
    public class ComponentInitializer : AsyncOperation
    {
        public bool IsInitialized { get; private set; }

        public void InitializeAsync(IOperationState state)
        {
            if (IsInitialized)
                throw new ArgumentException("已经在初始化中;");

            ThreadPool.QueueUserWorkItem(Initialize, state);
        }

        public void Initialize(object s)
        {
            if (IsInitialized)
                throw new ArgumentException("已经在初始化中;");

            IsInitialized = true;

            CustomInput.ReadOrDefault();
            OnCustomInputCompleted();

            Localization.Initialize();
            OnLocalizationCompleted();

            GameConsole.Initialize();
        }

        void OnCustomInputCompleted()
        {
            const string prefix = "[输入映射]";
            var emptyKeys = CustomInput.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                Debug.LogWarning(prefix + "初始化成功;存在未定义的按键:" + emptyKeys.ToLog());
            }
            else
            {
                Debug.Log(prefix + "初始化成功;");
            }
        }

        void OnLocalizationCompleted()
        {
            const string prefix = "[本地化]";
            string log = "初始化成功;条目总数:" + Localization.EntriesCount;
            Debug.Log(prefix + log);
        }
    }

}
