namespace JiongXiaGu.Unity.UI
{
    public struct ProgressInfo
    {
        public string Message { get; private set; }
        public float Progress { get; private set; }

        public ProgressInfo(float progress) : this(progress, null)
        {
        }

        public ProgressInfo(float progress, string message)
        {
            Progress = progress;
            Message = message;
        }
    }
}
