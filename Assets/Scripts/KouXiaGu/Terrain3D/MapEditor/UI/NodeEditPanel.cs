using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;

namespace KouXiaGu.Terrain3D.MapEditor
{

    public interface INodeEditer
    {
        void OnSelectNodeChanged(List<NodePair> data);
    }

    /// <summary>
    /// 对选中坐标进行编辑;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class NodeEditPanel : MonoBehaviour
    {
        NodeEditPanel()
        {
        }

        [SerializeField]
        SelectNodeList selectNodeList;

        void Awake()
        {
            selectNodeList.OnSelectValueChanged += OnSelectValueChanged;
        }

        void OnSelectValueChanged(IEnumerable<CubicHexCoord> data)
        {

        }

        public void Subscribe(INodeEditer observer)
        {
        }
    }

    public struct NodePair
    {
        public NodePair(CubicHexCoord position, MapNode data)
        {
            Position = position;
            Data = data;
        }

        public CubicHexCoord Position { get; private set; }
        public MapNode Data { get; set; }
    }
}
