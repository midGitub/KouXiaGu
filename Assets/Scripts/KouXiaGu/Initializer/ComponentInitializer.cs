
//是否在初始化时输出详细信息?
#define EDITOR_LOG

using System;
using System.Linq;
using KouXiaGu.Globalization;
using KouXiaGu.KeyInput;
using System.Threading;
using KouXiaGu.Concurrent;
using System.Diagnostics;

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

        [Conditional("EDITOR_LOG")]
        void OnCustomInputCompleted()
        {
            const string prefix = "[输入映射]";
            var emptyKeys = CustomInput.GetEmptyKeys().ToList();
            if (emptyKeys.Count != 0)
            {
                UnityEngine.Debug.LogWarning(prefix + "初始化成功;存在未定义的按键:" + emptyKeys.ToLog());
            }
            else
            {
                UnityEngine.Debug.Log(prefix + "初始化成功;");
            }
        }

        [Conditional("EDITOR_LOG")]
        void OnLocalizationCompleted()
        {
            const string prefix = "[本地化]";
            string log = "初始化成功;条目总数:" + Localization.EntriesCount;
            UnityEngine.Debug.Log(prefix + log);
        }
    }
}
