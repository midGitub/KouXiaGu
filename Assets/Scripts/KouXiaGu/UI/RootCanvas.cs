using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class RootCanvas : MonoBehaviour
    {
        RootCanvas()
        {
        }

        [SerializeField]
        EdgeAlignment edgeAlignment;

        public EdgeAlignment EdgeAlignment
        {
            get { return edgeAlignment; }
        }
    }
}
