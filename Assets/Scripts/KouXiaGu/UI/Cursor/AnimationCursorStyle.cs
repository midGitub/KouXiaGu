using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 鼠标样式;
    /// </summary>
    public class AnimationCursorStyle : CursorStyle
    {
        protected AnimationCursorStyle()
        {
        }

        [SerializeField]
        TextureInfo[] textures;
        int index;
        float time;

        /// <summary>
        /// 是否正在使用这个样式?
        /// </summary>
        public bool IsRunning { get; private set; }

        public override void OnEnter()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                SetCursor(0);
            }
        }

        public override void MoveNext()
        {
            var info = textures[index];
            if (Time.realtimeSinceStartup - time >= info.WaitTime)
            {
                index++;
                if (index >= textures.Length)
                {
                    index = 0;
                }
                SetCursor(index);
            }
        }

        /// <summary>
        /// 设置光标样式;
        /// </summary>
        void SetCursor(int index)
        {
            var info = textures[index];
            Cursor.SetCursor(info.Texture, info.Hotspot, info.CursorMode);
            time = Time.realtimeSinceStartup;
            this.index = index;
        }

        public override void OnQuit()
        {
            IsRunning = false;
        }

        [Serializable]
        struct TextureInfo
        {
            public Texture2D Texture;
            public Vector2 Hotspot;
            public CursorMode CursorMode;
            public float WaitTime; //秒
        }
    }
}
