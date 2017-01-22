using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 构建的建筑物;
    /// </summary>
    public class BuildingGroup
    {

        public BuildingGroup(IRequest request)
        {
            this.Request = request;
        }

        public IRequest Request { get; private set; }

    }

}
