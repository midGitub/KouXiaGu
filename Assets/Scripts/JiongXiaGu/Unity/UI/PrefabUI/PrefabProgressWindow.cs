using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{


    public class PrefabProgressWindow : PrefabMessageWindow
    {
        private PrefabProgressWindow()
        {
        }

        [SerializeField]
        private Scrollbar progressBar;
        public Scrollbar ProgressBar { get; private set; }

    }
}
