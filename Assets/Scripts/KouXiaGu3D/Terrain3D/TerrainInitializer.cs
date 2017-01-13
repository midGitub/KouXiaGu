using System.Collections;
using KouXiaGu.Grids;
using KouXiaGu.Initialization;
using UnityEngine;
using KouXiaGu.Collections;
using System;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形资源初始化,负责初始化次序;
    /// 控制整个地形初始化;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class TerrainInitializer : Initializer
    {

        static TerrainInitializer()
        {
            IsRunning = false;
            IsSaving = false;
            IsPause = false;
        }

        TerrainInitializer() { }


        public static bool IsRunning { get; private set; }
        public static bool IsSaving { get; private set; }
        public static bool IsPause { get; private set; }

        /// <summary>
        /// 当前游戏使用的地图;
        /// </summary>
        public static TerrainMap Map { get; private set; }



        public override void Initialize()
        {
            throw new NotImplementedException();
        }


        //protected override void Awake()
        //{
        //    base.Awake();
        //    if (IsRunning)
        //    {
        //        IsCompleted = true;
        //        IsFaulted = true;
        //    }

        //}

        /// <summary>
        /// 使用类信息初始化;
        /// </summary>
        public static IEnumerator Begin()
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return ResInitializer.Initialize();

            Map = MapFiler.Read();

            MapArchiver.Initialize(TerrainInitializer.Map);

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

            Map = MapFiler.Read();

            MapArchiver.Initialize(archive, Map);

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
