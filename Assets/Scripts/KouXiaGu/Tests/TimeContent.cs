using KouXiaGu.World;
using KouXiaGu.World.TimeSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Tests
{

    [DisallowMultipleComponent]
    class TimeContent : MonoBehaviour
    {
        TimeContent()
        {
        }

        [SerializeField]
        Text textObject = null;
        IWorld world;

        WorldTime time
        {
            get { return world.WorldData.Time; }
        }

        void Update()
        {
            world = WorldSceneManager.World;
            if (world != null)
            {
                textObject.text = TextUpdate();
            }
        }

        string TextUpdate()
        {
            World.TimeSystem.WorldDateTime time = this.time.CurrentTime;
            return "当前时间:" + time.ToTimeString();
        }
    }
}
