using System;
using System.Collections.Generic;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 窗口光标;
    /// </summary>
    public class WindowCursor : ICursor
    {
        public static readonly WindowCursor _default = new WindowCursor();
        public static WindowCursor Default => _default;

        /// <summary>
        /// 鼠标样式栈,顶部为当前鼠标样式;
        /// </summary>
        private static readonly LinkedList<ICustomCursor> customCursorStack = new LinkedList<ICustomCursor>();

        /// <summary>
        /// 当前鼠标样式;
        /// </summary>
        private static IDisposable currentCursor;

        private WindowCursor()
        {
        }

        public void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }

        public void ResetCursor()
        {
            Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
        }

        /// <summary>
        /// 设置当前鼠标样式,并返回取消方法;
        /// </summary>
        public static IDisposable SetCursor(ICustomCursor cursor)
        {
            if (cursor == null)
                throw new ArgumentNullException(nameof(cursor));

            var node = Add(cursor);
            return new Canceler(node);
        }

        private static LinkedListNode<ICustomCursor> Add(ICustomCursor cursor)
        {
            currentCursor?.Dispose();
            currentCursor = cursor.Play(Default);
            var node = customCursorStack.AddLast(cursor);
            return node;
        }

        private static void Remove(LinkedListNode<ICustomCursor> node)
        {
            if (customCursorStack.Last == node)
            {
                currentCursor.Dispose();
                customCursorStack.Remove(node);
                var last = customCursorStack.Last;
                if (last != null)
                {
                    currentCursor = last.Value.Play(Default);
                }
                else
                {
                    currentCursor = null;
                    Default.ResetCursor();
                }
            }
            else
            {
                customCursorStack.Remove(node);
            }
        }

        private struct Canceler : IDisposable
        {
            private LinkedListNode<ICustomCursor> node;

            public Canceler(LinkedListNode<ICustomCursor> node)
            {
                this.node = node;
            }

            public void Dispose()
            {
                if (node != null)
                {
                    Remove(node);
                    node = null;
                }
            }
        }
    }
}
