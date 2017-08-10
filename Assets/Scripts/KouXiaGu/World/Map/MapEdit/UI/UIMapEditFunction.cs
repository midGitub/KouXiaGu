using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIMapEditFunction : MonoBehaviour
    {
        public UIMapEditPanle MapEditPanle;
        public UIMapEditHandler ChangeLandform;
        public UIMapEditHandler ChangeRoad;

        public void AddChangeLandform()
        {
            MapEditPanle.CurrentView.CreateAndAdd(ChangeLandform);
        }

        public void AddChangeRoad()
        {
            MapEditPanle.CurrentView.CreateAndAdd(ChangeRoad);
        }
    }
}
