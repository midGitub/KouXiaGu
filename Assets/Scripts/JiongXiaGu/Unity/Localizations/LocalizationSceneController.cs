using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Localizations
{

    /// <summary>
    /// 本地化组件的场景控制器;
    /// </summary>
    [DisallowMultipleComponent]
    public class LocalizationSceneController : SceneSington<LocalizationSceneController>, IComponentInitializeHandle, IQuitSceneHandle
    {
        LocalizationSceneController()
        {
        }

        Task IComponentInitializeHandle.StartInitialize(CancellationToken token)
        {
            Localization.IsLockLanguage = true;
            return null;
        }

        Task IQuitSceneHandle.OnQuitScene()
        {
            Localization.IsLockLanguage = false;
            return null;
        }
    }
}
