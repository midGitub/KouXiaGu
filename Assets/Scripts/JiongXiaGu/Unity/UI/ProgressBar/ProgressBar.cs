using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    [DisallowMultipleComponent]
    [RequireComponent(typeof(Scrollbar))]
    public class ProgressBar : MonoBehaviour
    {
        private ProgressBar()
        {
        }

        private Scrollbar scrollbar;
        [SerializeField]
        private Text messageControl;
        public IProgress<ProgressInfo> Progress { get; private set; }

        private void Awake()
        {
            scrollbar = GetComponent<Scrollbar>();
            Progress = new Progress<ProgressInfo>(OnReport);
        }

        private void OnReport(ProgressInfo progress)
        {
            scrollbar.size = progress.Progress;
            if (messageControl != null && progress.Message != null)
            {
                messageControl.text = progress.Message;
            }
        }
    }
}
