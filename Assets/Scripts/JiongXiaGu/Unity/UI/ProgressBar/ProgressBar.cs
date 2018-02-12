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
        private Text messageText;
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
                if (progress.Progress >= 0)
                {
                    scrollbar.size = progress.Progress;
                }
            }
            if (messageText != null && progress.Message != null)
            {
                messageText.text = progress.Message;
            }
        }
    }
}
