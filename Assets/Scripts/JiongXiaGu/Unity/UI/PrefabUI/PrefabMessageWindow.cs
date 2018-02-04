using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 预制物体脚本 消息窗口;
    /// </summary>
    public class PrefabMessageWindow : MessageWindow
    {
        protected PrefabMessageWindow()
        {
        }

        [SerializeField]
        private Text messageText;
        public Text MessageText => messageText;
    }
}
