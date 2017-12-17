using System;

namespace JiongXiaGu.Unity.Resources
{
    /// <summary>
    /// 提供调用 CommitUpdate() 方法的处置接口;
    /// </summary>
    internal struct ContentCommitUpdateDisposer : IDisposable
    {
        public Content Content { get; private set; }

        public ContentCommitUpdateDisposer(Content content)
        {
            Content = content;
        }

        public void Dispose()
        {
            if (Content != null)
            {
                Content.CommitUpdate();
                Content = null;
            }
        }
    }
}
