using KouXiaGu.OperationRecord;
using KouXiaGu.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图节点编辑面板;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIMapEditPanle : MonoBehaviour
    {
        UIMapEditPanle()
        {
        }

        [SerializeField]
        Transform contentTransform;
        [SerializeField]
        UIMapEditHandlerTitle titlePrefab;
        [SerializeField]
        ContentSizeFitterEx contentSizeFitter;
        List<UIMapEditHandlerTitle> handlerTitles;
        MapEditPen mapEditer;

        public int HandlerCount
        {
            get { return handlerTitles.Count; }
        }

        /// <summary>
        /// 所有标题;
        /// </summary>
        internal IList<UIMapEditHandlerTitle> HandlerTitles
        {
            get { return handlerTitles; }
        }

        void Awake()
        {
            handlerTitles = new List<UIMapEditHandlerTitle>();
            //mapEditer = new MapEditPen()
        }

        /// <summary>
        /// 添加到;
        /// </summary>
        public UIMapEditHandlerTitle Create(UIMapEditHandler handlerPrefab)
        {
            if (Contains(handlerPrefab))
            {
                throw new ArgumentException();
            }
            var title = Instantiate(titlePrefab, contentTransform);
            var handler = Instantiate(handlerPrefab, contentTransform);
            title.Initialize(this, handler, contentSizeFitter);
            handler.Initialize(title);
            handlerTitles.Add(title);
            return title;
        }

        /// <summary>
        /// 确认是否存在;
        /// </summary>
        public bool Contains(IMapEditHandler handler)
        {
            return handlerTitles.Contains(item => item.EditHandler.Contrast(handler));
        }

        /// <summary>
        /// 对所有节点执行操作;
        /// </summary>
        public IVoidable Execute(IEnumerable<EditMapNode> nodes)
        {
            throw new NotImplementedException();
        }
    }
}
