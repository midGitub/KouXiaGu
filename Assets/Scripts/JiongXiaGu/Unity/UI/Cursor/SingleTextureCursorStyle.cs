using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.UI
{

    [DisallowMultipleComponent]
    public class SingleTextureCursorStyle : CursorStyle
    {
        SingleTextureCursorStyle()
        {
        }

        public Texture2D texture;
        public Vector2 hotspot;
        public CursorMode cursorMode;

        public override void OnEnter()
        {
            SetCursor(texture, hotspot, cursorMode);
        }

        public override void MoveNext()
        {
            return;
        }

        public override void OnQuit()
        {
            return;
        }
    }
}
