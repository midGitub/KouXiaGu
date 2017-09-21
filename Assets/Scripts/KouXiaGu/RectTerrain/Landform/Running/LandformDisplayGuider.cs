using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.RectTerrain
{


    [DisallowMultipleComponent]
    public class LandformDisplayGuider : BufferDisplayGuider
    {
        LandformDisplayGuider()
        {
        }

        void OnEnable()
        {
            LandformController controller = LandformController.Instance;
            if (controller != null)
            {
                controller.GuiderGroup.Add(this);
            }
        }

        void OnDisable()
        {
            LandformController controller = LandformController.Instance;
            if (controller != null)
            {
                controller.GuiderGroup.Remove(this);
            }
        }

        void Update()
        {
            RectCoord chunkPos = transform.position.ToLandformChunkRect();
            SetCenter(chunkPos);
        }
    }
}
