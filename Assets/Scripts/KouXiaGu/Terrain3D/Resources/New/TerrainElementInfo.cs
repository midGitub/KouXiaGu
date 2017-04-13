using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    public class TerrainElementInfo<T>
      where T : ElementInfo
    {
        public TerrainElementInfo(T info)
        {
            Info = info;
        }

        public T Info { get; private set; }

        public int ID
        {
            get { return Info.ID; }
        }
    }


}
