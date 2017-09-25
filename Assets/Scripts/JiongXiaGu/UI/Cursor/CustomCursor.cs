using JiongXiaGu.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JiongXiaGu.UI
{

    /// <summary>
    /// 光标样式管理;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CustomCursor : UnitySington<CustomCursor>
    {
        CustomCursor()
        {
        }

        [SerializeField]
        CursorStyle cursorDefalut;
        [SerializeField]
        CursorStyle cursorMove;
        [SerializeField]
        CursorStyle cursorWait;

        LinkedList<CursorState> cursorStyleStack;

        public int SubscriberCount
        {
            get { return cursorStyleStack == null ? 0 : cursorStyleStack.Count; }
        }

        void Awake()
        {
            cursorStyleStack = new LinkedList<CursorState>();
            SetCursor(cursorDefalut);
        }

        void Update()
        {
            var last = cursorStyleStack.Last.Value;
            last.CursorStyle.MoveNext();
        }

        /// <summary>
        /// 设置光标样式;
        /// </summary>
        public IDisposable SetCursor(CursorType cursorStyle)
        {
            switch (cursorStyle)
            {
                case CursorType.Default:
                    return SetCursor(cursorDefalut);
                case CursorType.Move:
                    return SetCursor(cursorMove);
                case CursorType.Wait:
                    return SetCursor(cursorWait);
                default:
                    Debug.Log("未定义光标样式:" + cursorStyle.ToString());
                    return null;
            }
        }

        /// <summary>
        /// 设置光标样式;
        /// </summary>
        public IDisposable SetCursor(CursorStyle cursorStyle)
        {
            var state = new CursorState(this, cursorStyle);
            return state;
        }

        class CursorState : IDisposable
        {
            public CursorState(CustomCursor parent, CursorStyle cursorStyle)
            {
                Parent = parent;
                CursorStyle = cursorStyle;
                Node = cursorStyleStack.AddLast(this);
                cursorStyle.OnEnter();
            }

            public CustomCursor Parent { get; private set; }
            public CursorStyle CursorStyle { get; private set; }
            public LinkedListNode<CursorState> Node { get; private set; }

            public LinkedList<CursorState> cursorStyleStack
            {
                get { return Parent.cursorStyleStack; }
            }

            public void Dispose()
            {
                if (Parent != null)
                {
                    if (cursorStyleStack.Last == Node)
                    {
                        var newLast = Node.Previous.Value;
                        if (newLast.CursorStyle != CursorStyle)
                        {
                            CursorStyle.OnQuit();
                        }
                        newLast.CursorStyle.OnEnter();
                        cursorStyleStack.Remove(Node);
                    }
                    else
                    {
                        CursorStyle.OnQuit();
                        cursorStyleStack.Remove(Node);
                    }

                    Parent = null;
                    CursorStyle = null;
                    Node = null;
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
