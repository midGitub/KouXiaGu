using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    [DisallowMultipleComponent]
    public class ProgressBar : MonoBehaviour
    {
        private ProgressBar()
        {
        }

        [SerializeField]
        private Scrollbar scrollbar;
        [SerializeField]
        private Text messageControl;
        public IProgress<ProgressInfo> Progress { get; private set; }

        private void Awake()
        {
            if (scrollbar == null)
                Debug.LogWarning("丢失 scrollbar!");

            Progress = new Progress<ProgressInfo>(OnReport);
        }

        private void OnReport(ProgressInfo progress)
        {
            if (scrollbar != null)
            {
                scrollbar.size = progress.Progress;
            }
            if (messageControl != null && progress.Message != null)
            {
                messageControl.text = progress.Message;
            }
        }
    }
}
