using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace JiongXiaGu.Unity.UI
{

    public struct ButtonInfo
    {
        public string Name { get; set; }
        public UnityAction Action { get; set; }
    }
}
