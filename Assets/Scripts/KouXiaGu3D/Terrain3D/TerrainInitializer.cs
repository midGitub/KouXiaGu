using System.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using UnityEngine;
using KouXiaGu.Collections;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化,负责初始化次序;
    /// 控制整个地形初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainInitializer : UnitySington<TerrainInitializer>
    {

        static TerrainInitializer()
        {
            IsRunning = false;
            IsSaving = false;
            IsPause = false;
        }

        TerrainInitializer() { }


        //#region 提供初始化;

        //static TerrainMapFile terrainMap;
        //static ArchiveDescription description;


        ///// <summary>
        ///// 当前游戏使用的地图;
        ///// </summary>
        //public static TerrainMapFile TerrainMap
        //{
        //    get { return terrainMap; }
        //    set
        //    {
        //        if (IsRunning)
        //            throw new CanNotEditException("在运行状态无法编辑!");
        //        terrainMap = value;
        //    }
        //}

        ///// <summary>
        ///// 预定义的信息;
        ///// </summary>
        //public static ArchiveDescription Description
        //{
        //    get { return description; }
        //    set
        //    {
        //        if (IsRunning)
        //            throw new CanNotEditException("在运行状态无法编辑!");
        //        description = value;
        //    }
        //}

        //#endregion


        public static bool IsRunning { get; private set; }
        public static bool IsSaving { get; private set; }
        public static bool IsPause { get; private set; }


        /// <summary>
        /// 使用类信息初始化;
        /// </summary>
        public static IEnumerator Begin()
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return ResInitializer.Initialize();

            MapFiler.Read();

            MapArchiver.Initialize(MapFiler.Map);

            TerrainCreater.Load();

            IsRunning = true;
            yield break;
        }

        /// <summary>
        /// 使用存档初始化;
        /// </summary>
        public static IEnumerator Begin(Archive archive)
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return ResInitializer.Initialize();

            MapFiler.Read();

            MapArchiver.Initialize(MapFiler.Map);

            TerrainCreater.Load();

            IsRunning = true;
            yield break;
        }


        /// <summary>
        /// 保存游戏内容;
        /// </summary>
        public static IEnumerator Save(Archive archive)
        {
            IsSaving = true;

            MapArchiver.Write(archive);
            yield return null;

            IsSaving = false;
        }


        /// <summary>
        /// 游戏结束;
        /// </summary>
        public static IEnumerator Finish()
        {
            if (!IsRunning)
                throw new PremiseNotInvalidException();

            MapArchiver.Clear();

            IsRunning = false;
            yield break;
        }


        /// <summary>
        /// 暂停游戏状态;
        /// </summary>
        public static void Pause()
        {
            IsPause = true;
        }


        /// <summary>
        /// 从暂停状态继续;
        /// </summary>
        public static void Continue()
        {
            IsPause = false;
        }

    }

}
