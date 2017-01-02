using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 对 INavigator 接口的拓展;
    /// </summary>
    public static class NavigatorExtensions
    {

        public static void NavigateTo(
            this INavigator navigator,
            CubicHexCoord destination,
            IPathFindingCost<CubicHexCoord, TerrainNode> obstruction,
            IRange<CubicHexCoord> searchRange,
            IMovable character)
        {
            CubicHexCoord starting = navigator.Position.GetCubic();
            var path = Pathfinding.FindPath(starting, destination, obstruction, searchRange);
            navigator.NavigateTo(character, path);
        }

        public static void NavigateTo(this INavigator navigator, IMovable character, Path<CubicHexCoord, TerrainNode> path)
        {
            var navPath = new NavigationPath(character, path);
            navigator.Follow(navPath);
        }

        static CubicHexCoord GetCubic(this Vector3 pos)
        {
            return GridConvert.Grid.GetCubic(pos);
        }

        static Vector3 GetPixel(this CubicHexCoord coord)
        {
            return GridConvert.Grid.GetPixel(coord);
        }

    }

}
