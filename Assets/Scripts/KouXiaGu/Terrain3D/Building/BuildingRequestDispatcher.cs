using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    class BuildingRequestDispatcher : RequestDispatcher
    {
        BuildingRequestDispatcher()
        {
        }

        public static BuildingRequestDispatcher Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }
    }

}
