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
    public static class CustomCursor
    {
        /// <summary>
        /// 当前鼠标样式;
        /// </summary>
        private static IDisposable currentCursorStyle;

        /// <summary>
        /// 设置当前鼠标样式;
        /// </summary>
        public static IDisposable SetCursor(CursorStyleType cursorStyle)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置当前鼠标样式;
        /// </summary>
        public static IDisposable SetCursor(string cursorStyle)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 鼠标样式栈,顶部为当前鼠标样式;
        /// </summary>
        private static readonly Stack<ICursorStyle> cursorStyleStack;

        /// <summary>
        /// 设置当前鼠标样式,并返回取消方法;
        /// </summary>
        public static IDisposable SetCursor(IDynamicCursorStyle cursorStyle)
        {
            if (cursorStyle == null)
                throw new ArgumentNullException(nameof(cursorStyle));

            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置当前鼠标样式,并返回取消方法;
        /// </summary>
        public static void SetCursor(CursorStyle cursorStyle)
        {
            throw new NotImplementedException();
        }

        private interface ICursorStyle
        {
            void Replay();
            void Stop();
        }

        public struct DynamicCursorStyle : ICursorStyle
        {
            public DynamicCursorStyle(IDynamicCursorStyle cursorStyle)
            {

            }

            public void Replay()
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new NotImplementedException();
            }
        }

        public struct StaticCursorStyle : ICursorStyle
        {
            public void Replay()
            {
                throw new NotImplementedException();
            }

            public void Stop()
            {
                throw new NotImplementedException();
            }
        }
    }
}
