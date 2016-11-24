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

        //[SerializeField]
        //Transform North;
        //[SerializeField]
        //Transform Northeast;
        //[SerializeField]
        //Transform Southeast;
        //[SerializeField]
        //Transform South;
        //[SerializeField]
        //Transform Southwest;
        //[SerializeField]
        //Transform Northwest;

        [SerializeField]
        RoadGroup[] roadGroup;

        //[ShowOnlyProperty]
        //string Dic { get { return roadDirection.ToString(); } }

        /// <summary>
        /// 本身是否存在道路;
        /// </summary>
        public bool HaveRoad
        {
            get { return (roadDirection & HexDirection.Self) > 0; }
        }

        void OnValidate()
        {
            if (roadGroup.Length > 0)
            {
                for (int i = 0; i <  roadGroup.Length; i++)
                {
                    roadGroup[i].name = roadGroup[i].direction.ToString();
                }
            }
        }

        public void SetState(HexDirection roadDirection)
        {
            this.roadDirection = roadDirection;

            //if ((roadDirection | HexDirection.North) > 0)
            //{
            //    Display(North);
            //}
            //if ((roadDirection | HexDirection.Northeast) > 0)
            //{
            //    Display(Northeast);
            //}
            //if ((roadDirection | HexDirection.Southeast) > 0)
            //{
            //    Display(Southeast);
            //}
            //if ((roadDirection | HexDirection.South) > 0)
            //{
            //    Display(South);
            //}
            //if ((roadDirection | HexDirection.Southwest) > 0)
            //{
            //    Display(Southwest);
            //}
            //if ((roadDirection | HexDirection.Northwest) > 0)
            //{
            //    Display(Northwest);
            //}
        }

        void Display(Transform item)
        {
            if (item != null)
            {
                item.gameObject.SetActive(true);
            }
        }

        [Serializable]
        struct RoadGroup
        {
            [HideInInspector]
            public string name;
            public HexDirection direction;
            public GameObject prefab;
        }

    }

}
