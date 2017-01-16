using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    [DisallowMultipleComponent]
    public class PageUI : MonoBehaviour
    {
        protected PageUI() { }

        public virtual void Display()
        {
            gameObject.SetActive(true);
        }

        public virtual void Conceal()
        {
            gameObject.SetActive(false);
        }

    }

}
