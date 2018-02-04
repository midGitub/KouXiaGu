using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 预制按钮;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PrefabButton : MonoBehaviour
    {
        private PrefabButton()
        {
        }

        [SerializeField]
        private Button buttonObject;
        public Button ButtonObject => buttonObject;

        [SerializeField]
        private Text textObject;
        public Text TextObject => textObject;
    }
}
