using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Globalization;
using KouXiaGu.KeyInput;
using UnityEngine;
using System.Threading;

namespace KouXiaGu
{

    /// <summary>
    /// 组建初始化;
    /// </summary>
    public class ComponentInitializer : AsyncOperation
    {
        public void Initialize()
        {
            Debug.Log("开始初始化游戏组件;");
            CustomInput.ReadOrDefault();
            Localization.InitializeAsync();
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
