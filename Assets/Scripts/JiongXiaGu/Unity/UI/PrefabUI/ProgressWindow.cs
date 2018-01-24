using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{


    public class ProgressWindow : MessageWindow
    {
        private ProgressWindow()
        {
        }

        [SerializeField]
        private Scrollbar progressBar;
        public Scrollbar ProgressBar { get; private set; }

    }
}
