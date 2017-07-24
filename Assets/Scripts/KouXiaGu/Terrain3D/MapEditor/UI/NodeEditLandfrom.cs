using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.MapEditor
{

    [DisallowMultipleComponent]
    public class NodeEditLandform : MonoBehaviour, INodeEditer
    {
        NodeEditLandform()
        {
        }

        void INodeEditer.OnSelectNodeChanged(List<NodePair> data)
        {
            throw new NotImplementedException();
        }
    }
}
