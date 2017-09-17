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
    public class LandformDisplayGuider : MonoBehaviour
    {
        LandformDisplayGuider()
        {
        }

        [SerializeField]
        BufferDisplayGuider displayGuider;

        void Awake()
        {
            displayGuider.Awake();
        }

        void OnEnable()
        {
            LandformController controller = SceneController.GetSington<LandformController>();
            if (controller != null)
            {
                controller.GuiderGroup.Add(displayGuider);
            }
        }

        void OnDisable()
        {
            LandformController controller = SceneController.GetSington<LandformController>();
            if (controller != null)
            {
                controller.GuiderGroup.Remove(displayGuider);
            }
        }

        void Update()
        {
            RectCoord chunkPos = transform.position.ToLandformChunkRect();
            displayGuider.SetCenter(chunkPos);
        }

        void OnValidate()
        {
            displayGuider.OnValidate();
        }
    }
}
