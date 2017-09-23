using JiongXiaGu.Concurrent;
using JiongXiaGu.Operations;
using JiongXiaGu.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图操作信息视窗;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIMapEditHandlerView : MonoBehaviour
    {
        UIMapEditHandlerView()
        {
        }

        [SerializeField]
        UIMapEditPanle panle;
        [SerializeField]
        Transform contentTransform;
        [SerializeField]
        UIMapEditHandlerTitle titlePrefab;
        [SerializeField]
        ContentSizeFitterEx contentSizeFitter;
        List<UIMapEditHandlerTitle> handlerTitles;

        public int Count
        {
            get { return handlerTitles.Count; }
        }

        void Awake()
        {
            handlerTitles = new List<UIMapEditHandlerTitle>();
        }

        void OnEnable()
        {
            panle.CurrentView = this;
        }

        public UIMapEditHandlerTitle CreateAndAdd(UIMapEditHandler handlerPrefab)
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

        public bool Remove(UIMapEditHandlerTitle title)
        {
            return handlerTitles.Remove(title);
        }

        public bool Contains(UIMapEditHandlerTitle title)
        {
            return handlerTitles.Contains(title);
        }

        public bool Contains(IMapEditHandler handler)
        {
            return handlerTitles.Contains(item => item.EditHandler.Contrast(handler));
        }

        /// <summary>
        /// 折叠所有操作细节;
        /// </summary>
        public void CollapseContentAll()
        {
            foreach (var title in handlerTitles)
            {
                title.HideContent();
            }
        }

        /// <summary>
        /// 展开所有操作细节;
        /// </summary>
        public void ExpandContentAll()
        {
            foreach (var title in handlerTitles)
            {
                title.DisplayContent();
            }
        }

        /// <summary>
        /// 清除所有操作;
        /// </summary>
        public void Clear()
        {
            handlerTitles.Clear();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// 对所有节点执行操作;
        /// </summary>
        public VoidableOperation Execute(WorldMap map, ICollection<EditMapNode> selectedArea)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            if (selectedArea == null)
                throw new ArgumentNullException("selectedArea");

            using (map.MapEditorLock.WriteLock())
            {
                var voidableGroup = new VoidableOperationGroup<VoidableOperation>();
                foreach (var handlerTitle in handlerTitles)
                {
                    if (handlerTitle.IsActivate)
                    {
                        var voidable = handlerTitle.EditHandler.Execute(selectedArea);
                        if (voidable != null)
                        {
                            voidableGroup.Add(voidable);
                        }
                    }
                }
                return new MapSetValue(map, selectedArea, voidableGroup);
            }
        }

        /// <summary>
        /// 撤销对地图的操作;
        /// </summary>
        sealed class MapSetValue : VoidableOperation
        {
            public MapSetValue(WorldMap map, IEnumerable<EditMapNode> editNodes, VoidableOperation voidableGroup)
            {
                Map = map;
                EditNodes = editNodes;
                this.voidableGroup = voidableGroup;
                Initialize();
            }

            public WorldMap Map { get; private set; }
            public IEnumerable<EditMapNode> EditNodes { get; private set; }
            readonly VoidableOperation voidableGroup;

            protected override void PerformDo_protected()
            {
                voidableGroup.PerformDo();
                Initialize();
            }

            void Initialize()
            {
                foreach (var editNode in EditNodes)
                {
                    Map.Map[editNode.Position] = editNode.Value;
                }
            }

            protected override void PerformUndo_protected()
            {
                voidableGroup.PerformUndo();
                Recovery();
            }

            void Recovery()
            {
                foreach (var editNode in EditNodes)
                {
                    Map.Map[editNode.Position] = editNode.Original;
                }
            }
        }
    }
}
