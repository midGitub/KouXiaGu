using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.RectTerrain.Resources
{

    public class RectTerrainResources
    {
        public RectTerrainResources()
        {
            Landform = new Dictionary<string, LandformResource>();
        }

        public Dictionary<string, LandformResource> Landform { get; private set; }


        public void Clear()
        {
            Landform.Clear();
        }
    }
}
