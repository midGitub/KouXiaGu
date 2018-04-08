using System;

namespace JiongXiaGu.Unity.UI.Cursors
{

    public class StaticCursor : ICustomCursor, IDisposable
    {
        public CursorInfo Cursor { get; private set; }

        public StaticCursor(CursorInfo cursor)
        {
            Cursor = cursor;
        }

        public IDisposable Play()
        {
            CursorController.SetCursor(Cursor);
            return this;
        }

        void IDisposable.Dispose()
        {
            return;
        }
    }
}
