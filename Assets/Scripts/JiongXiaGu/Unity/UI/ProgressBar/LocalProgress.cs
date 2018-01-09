using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 对一段进度进行包装;
    /// </summary>
    public class LocalProgress : IProgress<ProgressInfo>
    {
        public IProgress<ProgressInfo> Main { get; private set; }
        public float Min { get; private set; }
        private float difference;
        public float Max => Min + difference;

        public LocalProgress(IProgress<ProgressInfo> main, float min, float max)
        {
            if (main == null)
                throw new ArgumentNullException(nameof(main));
            if (max < min)
                throw new ArgumentOutOfRangeException();

            Main = main;
            Min = min;
            difference = max = min;
        }

        void IProgress<ProgressInfo>.Report(ProgressInfo value)
        {
            float progress = (value.Progress * difference) + Min;
            ProgressInfo info = new ProgressInfo(progress, value.Message);
            Main.Report(info);
        }
    }
}
