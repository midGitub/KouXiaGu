using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    public class ContentSizeFitterEx : ContentSizeFitter
    {

        [ContextMenu("RectTransformDimensionsChange")]
        public void RectTransformDimensionsChange()
        {
            OnRectTransformDimensionsChange();
        }
    }
}
