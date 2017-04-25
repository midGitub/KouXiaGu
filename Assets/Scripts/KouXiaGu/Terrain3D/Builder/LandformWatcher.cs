using System;
using System.Collections.Generic;
using KouXiaGu.Grids;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 根据挂载物体所在位置创建附近地形;
    /// </summary>
    [DisallowMultipleComponent]
    public class LandformWatcher : MonoBehaviour
    {



        //LandformWatcher() { }

        //[SerializeField]
        //Vision landform;

        ///// <summary>
        ///// 中心点;
        ///// </summary>
        //Vector3 position
        //{
        //    get { return transform.position; }
        //    set { transform.position = value; }
        //}

        //void Start()
        //{
        //    landform.Initialize(OLandformChunk.ChunkGrid);
        //}

        //void Update()
        //{
        //    if (TerrainData.Creater != null)
        //    {
        //        IEnumerable<RectCoord> displayCoords = landform.GetDisplay(position);

        //        foreach (var item in displayCoords)
        //        {
        //            TerrainData.Creater.Landform.Create(item);
        //        }
        //    }
        //}

        //void OnValidate()
        //{
        //    landform.Normalize();
        //}

        ///// <summary>
        ///// 根据设置半径返回对应视野;
        ///// </summary>
        //[Serializable]
        //class Vision
        //{
        //    Vision()
        //    {
        //    }

        //    /// <summary>
        //    /// 显示半径,在这个半径内的地形块会创建并显示;
        //    /// </summary>
        //    [SerializeField]
        //    RectCoord displayRadius = new RectCoord(2, 2);

        //    /// <summary>
        //    /// 隐藏半径,超出这个半径的地形块会被隐藏;
        //    /// </summary>
        //    [SerializeField]
        //    RectCoord concealRadius = new RectCoord(2, 2);

        //    public RectGrid Grid { get; private set; }
        //    HashSet<RectCoord> tempDisplayCoords;


        //    public void Initialize(RectGrid rectGrid)
        //    {
        //        this.Grid = rectGrid;
        //        tempDisplayCoords = new HashSet<RectCoord>();
        //        Normalize();
        //    }

        //    /// <summary>
        //    /// 使其符合要求;
        //    /// </summary>
        //    public void Normalize()
        //    {
        //        concealRadius.x = MathI.Clamp(concealRadius.x, displayRadius.x, short.MaxValue);
        //        concealRadius.y = MathI.Clamp(concealRadius.y, displayRadius.y, short.MaxValue);
        //    }

        //    /// <summary>
        //    /// 获取到需要显示到场景的坐标;
        //    /// </summary>
        //    public IEnumerable<RectCoord> GetDisplay(Vector3 pos)
        //    {
        //        var coord = Grid.GetCoord(pos);
        //        return GetDisplay(coord);
        //    }

        //    /// <summary>
        //    /// 获取到需要显示到场景的坐标;
        //    /// </summary>
        //    public IEnumerable<RectCoord> GetDisplay(RectCoord center)
        //    {
        //        return RectCoord.Range(center, displayRadius.x, displayRadius.y);
        //    }

        //    /// <summary>
        //    /// 获取到需要隐藏的坐标;
        //    /// </summary>
        //    public IEnumerable<RectCoord> GetConceal(Vector3 pos, IEnumerable<RectCoord> displays)
        //    {
        //        var coord = Grid.GetCoord(pos);
        //        return GetConceal(coord, displays);
        //    }

        //    /// <summary>
        //    /// 获取到需要隐藏的坐标;
        //    /// </summary>
        //    public IEnumerable<RectCoord> GetConceal(RectCoord center, IEnumerable<RectCoord> displays)
        //    {
        //        tempDisplayCoords.Clear();
        //        tempDisplayCoords.Add(displays);

        //        var needDisplays = RectCoord.Range(center, concealRadius.x, concealRadius.y);
        //        tempDisplayCoords.ExceptWith(needDisplays);

        //        return tempDisplayCoords;
        //    }

        //}

    }

}
