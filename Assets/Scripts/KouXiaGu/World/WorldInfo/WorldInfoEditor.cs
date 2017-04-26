using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World
{


    [DisallowMultipleComponent, RequireComponent(typeof(WorldInitializer))]
    public class WorldInfoEditor : MonoBehaviour
    {

        public bool UseRandomMap;

        class WorldInfoReader : AsyncOperation<WorldInfo>
        {
            public WorldInfoReader(WorldInfoEditor info)
            {
                this.info = info;
                worldInfo = new WorldInfo();
            }

            WorldInfoEditor info;
            WorldInfo worldInfo;

            void Start()
            {

            }

        }
    }

}
