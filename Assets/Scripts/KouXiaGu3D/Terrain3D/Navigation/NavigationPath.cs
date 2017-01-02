using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;

namespace KouXiaGu.Terrain3D.Navigation
{


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
            return GridConvert.Grid.GetPixel(path.Current);
        }

        float GetSpeed()
        {
            TerrainNode node;
            if (path.WorldMap.TryGetValue(path.Current, out node))
            {
                var descr = NavigationRes.GetNavigationDescr(node.Landform);
                return descr.SpeedOfTravel * Character.Speed;
            }
            Debug.LogError("路径点超出地图范围;");
            return Character.Speed;
        }

        void INavigationPath.Complete()
        {
            return;
        }

    }

}
