using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 响应
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SelectablePanel))]
    public class ObstaclePanle : MonoBehaviour
    {
        ObstaclePanle()
        {
        }

        /// <summary>
        /// 射线阻挡物体;
        /// </summary>
        [SerializeField]
        RayObstacle rayObstacleObject;
        [SerializeField]
        SelectablePanelTitle panelTile;
        SelectablePanel panel;

        void Awake()
        {
            if (rayObstacleObject == null)
            {
                Debug.LogWarning("未设置 rayObstacleObject 参数!");
                return;
            }

            panel = GetComponent<SelectablePanel>();
            panel.OnFocusEvent += OnFocus;
            panel.OnBlurEvent += OnBlur;

            rayObstacleObject.PointerDownEvent += OnPointerDown;
        }

        void OnFocus()
        {
            rayObstacleObject.SetActive(true);
            (rayObstacleObject.transform as RectTransform).SetAsLastSibling();
        }

        void OnBlur()
        {
            rayObstacleObject.SetActive(false);
        }

        void OnPointerDown(PointerEventData eventData)
        {
            if (panelTile != null)
            {
                panelTile.ApplyWarning();
            }
        }
    }
}
