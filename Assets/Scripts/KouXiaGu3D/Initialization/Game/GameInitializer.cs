using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 负责游戏场景初始化;
    /// </summary>
    public sealed class GameInitializer : UnitySington<GameInitializer>
    {
        static GameInitializer()
        {
            IsRunning = false;
            IsSaving = false;
            IsPause = false;
        }

        public static bool IsRunning { get; private set; }
        public static bool IsSaving { get; private set; }
        public static bool IsPause { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            IsRunning = true;
        }

        void Start()
        {
            
        }

        void OnDestroy()
        {
            IsRunning = false;
        }




        /// <summary>
        /// 开始阶段;
        /// </summary>
        public static IEnumerator Begin(Archive archive)
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return TerrainInitializer.Begin(archive);

            IsRunning = true;
            yield break;
        }

        public static IEnumerator Begin()
        {
            if (IsRunning)
                throw new PremiseNotInvalidException();

            yield return TerrainInitializer.Begin();

            IsRunning = true;
            yield break;
        }


        /// <summary>
        /// 保存到存档;
        /// </summary>
        public static IEnumerator Save(Archive archive)
        {
            IsSaving = true;

            yield return TerrainInitializer.Save(archive);

            IsSaving = false;
            yield break;
        }


        /// <summary>
        /// 结束游戏;
        /// </summary>
        public static IEnumerator Finish()
        {
            if (!IsRunning)
                throw new PremiseNotInvalidException();

            yield return TerrainInitializer.Finish();

            IsRunning = false;
            yield break;
        }


        /// <summary>
        /// 暂停游戏状态;
        /// </summary>
        public static void Pause()
        {
            TerrainInitializer.Pause();

            IsPause = true;
        }

        /// <summary>
        /// 从暂停状态继续;
        /// </summary>
        public static void Continue()
        {
            TerrainInitializer.Continue();

            IsPause = false;
        }

    }

}
