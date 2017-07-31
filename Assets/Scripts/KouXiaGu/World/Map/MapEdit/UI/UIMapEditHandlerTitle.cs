using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World.Map.MapEdit
{

    [DisallowMultipleComponent]
    public class UIMapEditHandlerTitle : MonoBehaviour
    {
        UIMapEditHandlerTitle()
        {
        }

        [SerializeField]
        Toggle activationToggle;
        [SerializeField]
        Text titleText;
        [SerializeField]
        UIMapEditPenPanle mapEditPen;
        [SerializeField]
        UIMapEditHandler editPenHandler;
        
        public UIMapEditHandler EditPenHandler
        {
            get { return editPenHandler; }
            set { editPenHandler = value; }
        }

        public void DisplayContent(bool isDisplay)
        {
            editPenHandler.gameObject.SetActive(isDisplay);
        }



        /// <summary>
        /// 移除这个操作
        /// </summary>
        public void RemoveHandler()
        {

        }
    }
}
