using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.UI
{


    public abstract class DisplaySwitcher : MonoBehaviour
    {
        public abstract bool IsDisplay();
        public abstract void Display();
        public abstract void Hide();

        public virtual void SwitchDisplay()
        {
            if (IsDisplay())
            {
                Hide();
            }
            else
            {
                Display();
            }
        }
    }
}
