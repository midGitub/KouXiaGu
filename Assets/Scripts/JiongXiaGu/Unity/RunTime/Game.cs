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
    /// 表示游戏生命周期中的当前阶段;
    /// </summary>
    public enum GameStates
    {
        /// <summary>
        /// 等待模块初始化;
        /// </summary>
        WaitingForModule,

        /// <summary>
        /// 等待世界创建;
        /// </summary>
        WaitingCreateWorld,

        /// <summary>
        /// 世界正在运行;
        /// </summary>
        Running,
    }

    /// <summary>
    /// 游戏管理;
    /// </summary>
    public static class Game
    {
        public static GameStates State { get; private set; } = GameStates.WaitingForModule;

        private static List<ModificationInfo> modificationLoadOrder;
        public static IReadOnlyList<ModificationInfo> ModificationLoadOrder => modificationLoadOrder;

        /// <summary>
        /// 模组资源;
        /// </summary>
        public static ActivatedModification ActivatedModification { get; private set; }

        /// <summary>
        /// 游戏使用资源;
        /// </summary>
        public static GameResource Resource { get; private set; }

        /// <summary>
        /// 世界资源提供者;
        /// </summary>
        public static IWroldResourceProvider WroldResourceProvider { get; private set; }

        /// <summary>
        /// 当前游戏世界;
        /// </summary>
        public static World Wrold { get; private set; }

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
        public static void SetModificationLoadOrder(IEnumerable<ModificationInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException(nameof(infos));
            if (State == GameStates.Running)
                throw new InvalidOperationException();

            if (modificationLoadOrder == null)
            {
                modificationLoadOrder = new List<ModificationInfo>(infos);
            }
            else
            {
                modificationLoadOrder.Clear();
                modificationLoadOrder.AddRange(infos);
            }
        }

        /// <summary>
        /// 加载资源,若资源未发生变化则无操作;
        /// </summary>
        public static void LoadResource(IProgress<string> progress = null)
        {
            if(State == GameStates.Running)
                throw new InvalidOperationException();

            if (ActivatedModification == null)
            {
                ActivatedModification = new ActivatedModification(ModificationLoadOrder);
                ReloadResource(progress);
            }
            else
            {
                if (ActivatedModification.Activate(ModificationLoadOrder))
                {
                    ReloadResource(progress);
                }
            }
        }

        /// <summary>
        /// 重新加载资源,若资源未加载,则直接加载;
        /// </summary>
        public static void ReloadResource(IProgress<string> progress = null)
        {
            GameResourceFactroy resourceFactroy = new GameResourceFactroy();
            Resource = resourceFactroy.Read(ActivatedModification.Modifications, progress);
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
