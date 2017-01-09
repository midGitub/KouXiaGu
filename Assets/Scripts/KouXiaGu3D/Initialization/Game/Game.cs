using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.Initialization
{


    public static class Game 
    {

        static Game()
        {
            IsStart = false;
            IsPause = false;
        }


        public static bool IsStart { get; private set; }
        public static bool IsPause { get; private set; }


        /// <summary>
        /// 开始游戏;
        /// </summary>
        public static IEnumerator StageStart(Archive archive)
        {
            if (IsStart)
                throw new PremiseNotInvalidException();

            yield return StageStartAction(archive);
            IsStart = true;
        }

        static IEnumerator StageStartAction(Archive archive)
        {
            return TerrainInitializer.GameStart(archive);
        }


        /// <summary>
        /// 结束游戏;
        /// </summary>
        public static IEnumerator StageEnd()
        {
            if (!IsStart)
                throw new PremiseNotInvalidException();

            yield return StageEndAction();
            IsStart = false;
        }

        static IEnumerator StageEndAction()
        {
            yield break;
        }


        /// <summary>
        /// 保存到存档;
        /// </summary>
        public static void Save(Archive archive)
        {

        }

        static void SaveAction()
        {

        }


        /// <summary>
        /// 暂停游戏状态;
        /// </summary>
        public static void Pause()
        {

        }

        /// <summary>
        /// 从暂停状态继续;
        /// </summary>
        public static void Continue()
        {

        }

    }

}
