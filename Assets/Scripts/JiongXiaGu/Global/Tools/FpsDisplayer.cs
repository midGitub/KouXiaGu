using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu
{

    public class FpsDisplayer : MonoBehaviour
    {

        public Text textObject;
        int fps;
        float time;

        void Update()
        {
            if ((Time.realtimeSinceStartup - time) >= 1)
            {
                textObject.text = "Fps:" + fps;
                fps = 0;
                time = Time.realtimeSinceStartup;
            }
            fps++;
        }
    }
}
