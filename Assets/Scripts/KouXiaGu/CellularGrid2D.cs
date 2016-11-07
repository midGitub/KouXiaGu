using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 根据相机位置进行移动的网格;
    /// </summary>
    public class CellularGrid2D : MonoBehaviour
    {

        [SerializeField]
        private Transform targetCamera;

        [SerializeField]
        private Transform movingObject;

        /// <summary>
        /// (x, y) 相机超出这个范围进行移动,(z)每个贴图在z轴上的位置;
        /// </summary>
        [SerializeField]
        private Vector3 movingOffset;


        private void Start()
        {
            targetCamera.ObserveEveryValueChanged(tran => tran.position, FrameCountType.Update).
                Subscribe(GridUpdateX);
        }

        public void GridUpdateX(Vector3 cameraPosition)
        {
            Debug.Log(cameraPosition);
            Vector3 move = movingObject.position;
            float distanceX = cameraPosition.x - movingObject.position.x;
            float multiple = distanceX / movingOffset.x;

            if (multiple < 0)
            {
                return;
            }
            else if (distanceX > 0)
            {
                move = new Vector3(movingObject.position.x + (movingOffset.x * multiple), movingObject.position.y, 0);
            }
            else if(distanceX < 0)
            {
                move = new Vector3(movingObject.position.x + (movingOffset.x * -multiple), movingObject.position.y, 0);
            }
            movingObject.position = move;
        }



    }

}
