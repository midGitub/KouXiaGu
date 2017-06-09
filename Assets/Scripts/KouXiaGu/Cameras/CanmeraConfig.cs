using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Cameras
{

    [XmlType("Canmera")]
    [Serializable]
    public class WorldCanmeraConfig
    {
        public bool IsLockTarget;
        public float Zoom;
        public float ZoomRatio;
        public float MovementRatio;
        public bool CanEdgeMovement;
        public float EdgeMovementRatio;
    }
}
