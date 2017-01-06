using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    public static class UICamera
    {

        [CustomUnityTag]
        const string UI_CAMERA_TAG = "UICamera"; 

        static Camera camera;

        public static Camera Main
        {
            get { return camera ?? (camera = GameObject.FindWithTag(UI_CAMERA_TAG).GetComponent<Camera>()); }
        }

    }

}
