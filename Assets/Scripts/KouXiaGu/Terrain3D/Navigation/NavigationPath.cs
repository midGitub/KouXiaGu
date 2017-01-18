using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 导航路径;
    /// </summary>
    public class NavigationPath : INavigationPath
    {

        public NavigationPath(IMovable character, Path<CubicHexCoord, TerrainNode> path)
        {
            this.Character = character;
            this.path = path;
        }

        public IMovable Character { get; private set; }

        Path<CubicHexCoord, TerrainNode> path;

        float maxSpeed;

        Vector3 position;

        float INavigationPath.MaxSpeed
        {
            get { return maxSpeed; }
        }

        Vector3 INavigationPath.Position
        {
            get { return position; }
        }

        bool INavigationPath.MoveNext()
        {
            bool moveNext = path.MoveNext();

            this.position = GetPosition();
            this.maxSpeed = GetSpeed();

            return moveNext;
        }

        Vector3 GetPosition()
        {
            Vector3 pos = TerrainConvert.Grid.GetPixel(path.Current);
            pos.y = GetHeight(pos);
            return pos;
        }

        /// <summary>
        /// 获取到这个点的高度;
        /// </summary>
        float GetHeight(Vector3 point)
        {
            return TerrainData.GetHeight(point);
        }

        float GetSpeed()
        {
            TerrainNode node;
            if (path.WorldMap.TryGetValue(path.Current, out node))
            {
                var descr = NavigationRes.GetNavigationDescr(node.LandformInfo.ID);
                return descr.SpeedOfTravel * Character.MovingSpeed;
            }
            Debug.LogError("路径点超出地图范围;");
            return Character.MovingSpeed;
        }

        void INavigationPath.Complete()
        {
            return;
        }

        public override string ToString()
        {
            return path.ToString();
        }

    }

}
