using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Scenarios;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JiongXiaGu.Unity.RunTime
{

    /// <summary>
    /// 游戏管理;
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// 游戏模组加载顺序;
        /// </summary>
        public static IReadOnlyList<Modification> Modifications { get; private set; }

        /// <summary>
        /// 游戏使用资源;
        /// </summary>
        public static GameResource Resource { get; private set; }

        /// <summary>
        /// 当前游戏世界;
        /// </summary>
        public static Wrold Wrold { get; private set; }

        /// <summary>
        /// 从 欢迎场景 转到 主菜单场景;
        /// </summary>
        public static void GoMainMenuScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从 主菜单场景 转到 游戏场景;
        /// </summary>
        public static void GoGameScene()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从 游戏场景 转到 主菜单场景;
        /// </summary>
        public static void BackMainMenuScene()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 设置模组加载顺序;
        /// </summary>
        public static void SetModifications(IEnumerable<Modification> modifications)
        {
            if (modifications == null)
                throw new ArgumentNullException(nameof(modifications));

            Modifications = new List<Modification>(modifications);
        }

        /// <summary>
        /// 加载资源,若资源已经加载则无操作;
        /// </summary>
        public static void LoadResource(IProgress<string> progress = null)
        {
            if (Resource == null)
            {
                ReloadResource(progress);
            }
        }

        /// <summary>
        /// 重新加载资源,若资源未加载,则直接加载;
        /// </summary>
        public static void ReloadResource(IProgress<string> progress = null)
        {
            GameResourceFactroy resourceFactroy = new GameResourceFactroy();
            Resource = resourceFactroy.Read(Modifications, progress);
        }

        /// <summary>
        /// 结束游戏场景;
        /// </summary>
        public static void CloseWorld()
        {
            throw new ArgumentNullException();
        }
    }
}
