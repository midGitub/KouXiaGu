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
        /// 设置当前鼠标样式;
        /// </summary>
        public static IDisposable SetCursor(ICursorStyle cursorStyle)
        {
            if (cursorStyle == null)
                throw new ArgumentNullException(nameof(cursorStyle));

            throw new NotImplementedException();
        }
    }
}
