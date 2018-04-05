using System;

namespace JiongXiaGu.Unity.UI.Cursors
{
    public struct StaticCursor : ICustomCursor, IDisposable
    {
        public CursorInfo Cursor { get; private set; }

        public StaticCursor(CursorInfo cursor)
        {
            this.Cursor = cursor;
        }

        public IDisposable Play()
        {
            CustomCursor.SetCursor(Cursor);
            return this;
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}
