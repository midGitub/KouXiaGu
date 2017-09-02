using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    [DisallowMultipleComponent]
    public class LocalizationInitializer : MonoBehaviour, IInitializer
    {
        LocalizationInitializer()
        {
        }

        Task IInitializer.StartInitialize()
        {
            return Task.Run(delegate ()
            {
                Localization.Initialize();
                OnLocalizationCompleted();
            });
        }

        [Conditional("EDITOR_LOG")]
        void OnLocalizationCompleted()
        {
            const string prefix = "[本地化组件]";
            string log = "初始化完成;";
            UnityEngine.Debug.Log(prefix + log);
        }
    }
}
