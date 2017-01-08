using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public sealed class LocalizationUI : MonoBehaviour, IResponsive
    {




        void IResponsive.OnApply()
        {
            throw new NotImplementedException();
        }

        void IResponsive.OnReset()
        {
            throw new NotImplementedException();
        }
    }

}
