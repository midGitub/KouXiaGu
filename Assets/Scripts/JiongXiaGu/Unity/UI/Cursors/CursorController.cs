using System;
using System.Collections.Generic;

namespace JiongXiaGu.Unity.UI.Cursors
{

    ///// <summary>
    ///// 全局鼠标样式控制;(仅Unity线程安全)
    ///// </summary>
    //public static class CursorController
    //{
    //    /// <summary>
    //    /// 窗口光标操作接口;
    //    /// </summary>
    //    public static ICursor Cursor => WindowCursor.Default;

    //    /// <summary>
    //    /// 鼠标样式栈,顶部为当前鼠标样式;
    //    /// </summary>
    //    private static readonly LinkedList<ICustomCursor> customCursorStack = new LinkedList<ICustomCursor>();

    //    /// <summary>
    //    /// 当前鼠标样式;
    //    /// </summary>
    //    private static IDisposable currentCursor;

    //    /// <summary>
    //    /// 设置当前鼠标样式,并返回取消方法;
    //    /// </summary>
    //    public static IDisposable SetCursor(ICustomCursor cursor)
    //    {
    //        if (cursor == null)
    //            throw new ArgumentNullException(nameof(cursor));

    //        var node = Add(cursor);
    //        return new Canceler(node);
    //    }

    //    private static LinkedListNode<ICustomCursor> Add(ICustomCursor cursor)
    //    {
    //        currentCursor?.Dispose();
    //        currentCursor = cursor.Play(WindowCursor.Default);
    //        var node = customCursorStack.AddLast(cursor);
    //        return node;
    //    }

    //    private static void Remove(LinkedListNode<ICustomCursor> node)
    //    {
    //        if (customCursorStack.Last == node)
    //        {
    //            currentCursor.Dispose();
    //            customCursorStack.Remove(node);
    //            var last = customCursorStack.Last;
    //            if (last != null)
    //            {
    //                currentCursor = last.Value.Play(WindowCursor.Default);
    //            }
    //            else
    //            {
    //                currentCursor = null;
    //                Cursor.ResetCursor();
    //            }
    //        }
    //        else
    //        {
    //            customCursorStack.Remove(node);
    //        }
    //    }

    //    private struct Canceler : IDisposable
    //    {
    //        private LinkedListNode<ICustomCursor> node;

    //        public Canceler(LinkedListNode<ICustomCursor> node)
    //        {
    //            this.node = node;
    //        }

    //        public void Dispose()
    //        {
    //            if (node != null)
    //            {
    //                Remove(node);
    //                node = null;
    //            }
    //        }
    //    }
    //}
}
