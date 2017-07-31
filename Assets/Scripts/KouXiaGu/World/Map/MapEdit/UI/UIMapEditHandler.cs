using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public abstract class UIMapEditHandler : MonoBehaviour
    {
        UIMapEditHandler()
        {
        }

        public abstract IMapEditPenHandler MapEditHandler { get; }
    }
}
