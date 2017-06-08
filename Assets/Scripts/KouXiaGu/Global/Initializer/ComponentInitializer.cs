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
    /// 组建初始化;
    /// </summary>
    public class ComponentInitializer : AsyncInitializer
    {

        public ComponentInitializer()
        {
        }

        public override string Prefix
        {
            get { return "功能组件"; }
        }

        public void Start()
        {
            StartInitialize();
            Initialize();
        }

        void Initialize()
        {
            CustomInput.ReadOrDefault();
            IAsyncOperation[] missions = new IAsyncOperation[]
                {
                    Localization.InitializeAsync().Subscribe(this, OnLocalizationCompleted, OnFaulted),
                };
            (missions as IEnumerable<IAsyncOperation>).Subscribe(this, OnCompleted, OnFaulted);
        }

        void OnCustomInputCompleted(IAsyncOperation operation)
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

        void OnLocalizationCompleted(IAsyncOperation operation)
        {
            const string prefix = "[本地化]";
            string log = "初始化成功;条目总数:" + Localization.EntriesCount;
            Debug.Log(prefix + log);
        }
    }

}
