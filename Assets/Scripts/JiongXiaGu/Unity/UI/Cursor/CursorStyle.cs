using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.UI
{

    [DisallowMultipleComponent]
    public abstract class CursorStyle : MonoBehaviour
    {
        protected CursorStyle()
        {
        }

        /// <summary>
        /// 是否隐藏鼠标;
        /// </summary>
        protected bool Visible
        {
            get { return Cursor.visible; }
            set { Cursor.visible = value; }
        }

        /// <summary>
        /// 开始这个光标效果;
        /// 若在OnQuit() 之后调用,则代表重新开始这个效果;
        /// 若在未 OnQuit() 之后调用,则代表继续这个效果;
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 循环调用;
        /// </summary>
        public abstract void MoveNext();

        /// <summary>
        /// 退出这个光标效果;
        /// </summary>
        public abstract void OnQuit();

        /// <summary>
        /// 设置光标样式;
        /// </summary>
        protected void SetCursor(Texture2D texture, Vector2 hotspot, CursorMode cursorMode)
        {
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }
    }
}
