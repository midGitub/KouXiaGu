using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.UI.Cursors
{

    /// <summary>
    /// 鼠标样式控制;(仅Unity线程安全)
    /// </summary>
    public static class CursorController
    {
        /// <summary>
        /// 自定义鼠标样式映射表;
        /// </summary>
        public static IDictionary<string, ICustomCursor> Map { get; internal set; }

        /// <summary>
        /// 设置当前鼠标样式;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public static IDisposable SetCursor(CursorType type)
        {
            return SetCursor(type.ToString());
        }
        
        /// <summary>
        /// 设置当前鼠标样式;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public static IDisposable SetCursor(string name)
        {
            ICustomCursor cursor = Map[name];
            return SetCursor(cursor);
        }


        /// <summary>
        /// 鼠标样式栈,顶部为当前鼠标样式;
        /// </summary>
        private static readonly LinkedList<ICustomCursor> customCursorStack = new LinkedList<ICustomCursor>();

        /// <summary>
        /// 当前鼠标样式;
        /// </summary>
        private static IDisposable currentCursor;

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
            currentCursor = cursor.Play();
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
                    currentCursor = last.Value.Play();
                }
                else
                {
                    currentCursor = null;
                    ResetCursor();
                }
            }
            else
            {
                customCursorStack.Remove(node);
            }
        }

        /// <summary>
        /// 重置鼠标样式;
        /// </summary>
        public static void ResetCursor()
        {
            Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
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



        /// <summary>
        /// 设置当前鼠标样式,并返回取消方法;
        /// </summary>
        public static void SetCursor(CursorInfo cursor)
        {
            Cursor.SetCursor(cursor.Texture, cursor.Hotspot, cursor.CursorMode);
        }
    }
}
