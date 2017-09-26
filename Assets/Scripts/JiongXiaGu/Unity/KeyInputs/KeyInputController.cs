using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.KeyInputs
{

    /// <summary>
    /// 输入组件控制;
    /// </summary>
    [DisallowMultipleComponent]
    class KeyInputController : MonoBehaviour, IGameInitializeHandle
    {
        KeyInputController()
        {
        }

        Task IGameInitializeHandle.StartInitialize(CancellationToken token)
        {
            return Task.Run((Action)ReadAndApplyKeyMap);
        }

        void ReadAndApplyKeyMap()
        {
            KeyMapReader keyMapReader = new KeyMapReader();
            KeyInput.CurrentKeyMap = keyMapReader.Deserialize();
        }

        [ContextMenu("输出默认按键模版")]
        void WriteTemplateDefaultKeyMap()
        {
            KeyMap keyMap = new KeyMap()
            {
                { "Up", new CombinationKey(KeyCode.UpArrow)},
                { "Down", new CombinationKey(KeyCode.DownArrow)},
                { "Save", new CombinationKey(KeyCode.LeftControl, KeyCode.S)}
            };

            DefaultKeyConfigReader reader = new DefaultKeyConfigReader();
            reader.IsAutoCreateDirectory = true;
            reader.Serialize(keyMap.ToArray());
        }
    }
}
