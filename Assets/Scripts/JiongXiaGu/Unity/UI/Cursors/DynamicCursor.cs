using System;

namespace JiongXiaGu.Unity.UI.Cursors
{
    public struct DynamicCursor : ICustomCursor
    {
        public IDynamicCursor Cursor { get; private set; }

        public DynamicCursor(IDynamicCursor cursor)
        {
            if (cursor == null)
                throw new ArgumentNullException(nameof(cursor));

            Cursor = cursor;
        }

        public IDisposable Play()
        {
            var canceler = UnityCoroutine.Start(Cursor.GetCoroutine());
            return canceler;
        }
    }
}
