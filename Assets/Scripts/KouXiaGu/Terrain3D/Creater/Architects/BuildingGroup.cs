using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 构建的建筑物;
    /// </summary>
    public class BuildingGroup
    {

        public BuildingGroup(IRequest request, List<GameObject> buildings)
        {
            this.Request = request;
            this.buildings = buildings;
        }

        List<GameObject> buildings;

        public IRequest Request { get; private set; }

        /// <summary>
        /// 销毁所以实例化的建筑;
        /// </summary>
        public void Destroy()
        {
            foreach (var item in buildings)
            {
                GameObject.Destroy(item);
            }
            buildings.Clear();
        }

    }

}
