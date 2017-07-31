using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图节点编辑面板;
    /// </summary>
    [DisallowMultipleComponent]
    public class UIMapEditPenPanle : MonoBehaviour
    {
        UIMapEditPenPanle()
        {
        }

        [SerializeField]
        Transform contentTransform;
        [SerializeField]
        UIMapEditHandlerTitle editPenHandlerPrefab;
        [SerializeField]
        LinkedList<UIMapEditHandlerTitle> handlerTitles;

        /// <summary>
        /// 添加到第一个;
        /// </summary>
        public LinkedListNode<UIMapEditHandlerTitle> AddFirst(UIMapEditHandlerTitle handlerTitle)
        {
            return handlerTitles.AddFirst(handlerTitle);
        }

        /// <summary>
        /// 添加到最后;
        /// </summary>
        public LinkedListNode<UIMapEditHandlerTitle> AddLast(UIMapEditHandlerTitle handlerTitle)
        {
            return handlerTitles.AddLast(handlerTitle);
        }

        /// <summary>
        /// 获取到下标;
        /// </summary>
        public int FindIndex(UIMapEditHandlerTitle handlerTitle)
        {
            int index = 0;
            LinkedListNode<UIMapEditHandlerTitle> current = handlerTitles.First;
            while (current.Next != null)
            {
                if (handlerTitle.Equals(current.Next.Value))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
