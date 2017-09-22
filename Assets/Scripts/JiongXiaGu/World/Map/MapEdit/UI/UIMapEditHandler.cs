using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiongXiaGu.OperationRecord;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public abstract class UIMapEditHandler : MonoBehaviour, IMapEditHandler
    {
        protected const string strUnknown = "Unknown";
        UIMapEditHandlerTitle title;

        protected UIMapEditHandlerTitle Title
        {
            get { return title; }
        }

        public abstract string GetMessage();
        public abstract bool Contrast(IMapEditHandler handler);
        public abstract IVoidable Execute(IEnumerable<EditMapNode> nodes);

        public void Initialize(UIMapEditHandlerTitle title)
        {
            this.title = title;
            title.SetMessage(GetMessage());
        }
    }
}
