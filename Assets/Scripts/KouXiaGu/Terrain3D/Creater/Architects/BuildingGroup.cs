//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using KouXiaGu.Grids;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 构建的建筑物;
//    /// </summary>
//    public class BuildingGroup
//    {

//        public BuildingGroup(IRequest request, List<GameObject> buildings)
//        {
//            this.Request = request;
//            this.buildings = buildings;
//        }

//        List<GameObject> buildings;

//        public IRequest Request { get; private set; }

//        public RectCoord ChunkCoord
//        {
//            get { return Request.ChunkCoord; }
//        }


//        public void ResetHeight(Texture2D heightMap)
//        {
//            foreach (var building in buildings)
//            {
//                ResetHeight(building.transform, heightMap);
//            }
//        }

//        void ResetHeight(Transform building, Texture2D heightMap)
//        {
//            Vector3 pos = building.position;
//            pos.y = TerrainData.GetHeight(ChunkCoord, pos, heightMap);
//            building.position = pos;
//        }

//        /// <summary>
//        /// 重置所有建筑实例的高度;
//        /// </summary>
//        public void ResetHeight(Action<Transform> setHeight)
//        {
//            foreach (var item in buildings)
//            {
//                setHeight(item.transform);
//            }
//        }

//        /// <summary>
//        /// 销毁所有建筑实例;
//        /// </summary>
//        public void Destroy()
//        {
//            foreach (var item in buildings)
//            {
//                GameObject.Destroy(item);
//            }
//            buildings.Clear();
//        }

//    }

//}
