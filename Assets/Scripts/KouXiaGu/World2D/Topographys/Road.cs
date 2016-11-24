using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    [DisallowMultipleComponent]
    public class Road : MonoBehaviour
    {

        [SerializeField]
        HexDirection roadDirection;

        [SerializeField]
        RoadGroup[] roadGroup;

        /// <summary>
        /// 本身是否存在道路;
        /// </summary>
        public bool HaveRoad
        {
            get { return (roadDirection & HexDirection.Self) > 0; }
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (roadGroup!= null && roadGroup.Length > 0)
            {
                for (int i = 0; i <  roadGroup.Length; i++)
                {
                    roadGroup[i].name = roadGroup[i].direction.ToString();
                }
            }
        }
#endif

        public void SetState(HexDirection roadDirection)
        {
            this.roadDirection = roadDirection;

            foreach (var item in roadGroup)
            {
                if ((item.direction & roadDirection) > 0)
                {
                    Show(item.prefab);
                }
                else
                {
                    Hide(item.prefab);
                }
            }
        }

        void Show(GameObject item)
        {
            if (item != null)
            {
                item.SetActive(true);
            }
        }

        void Hide(GameObject item)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }

        [Serializable]
        struct RoadGroup
        {
#if UNITY_EDITOR
            [HideInInspector]
            public string name;
#endif
            public HexDirection direction;
            public GameObject prefab;
        }

    }

}
