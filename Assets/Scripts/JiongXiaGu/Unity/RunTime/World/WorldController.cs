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
    /// 游戏世界管理;
    /// </summary>
    public static class WorldController
    {
        /// <summary>
        /// 游戏使用资源;
        /// </summary>
        public static GameResource Resource { get; private set; }

        /// <summary>
        /// 游戏世界资源;
        /// </summary>
        public static WorldResource WorldResource { get; private set; }

        /// <summary>
        /// 当前游戏世界;
        /// </summary>
        public static World World { get; private set; }

        /// <summary>
        /// 游戏世界是否已经创建?
        /// </summary>
        public static bool IsCreated { get; private set; } = false;

        /// <summary>
        /// 创建游戏世界;
        /// </summary>
        public static void CreateWorld(IWroldResourceProvider wroldResourceProvider)
        {
            CreateWorld(ModificationController.Default, wroldResourceProvider);
        }

        /// <summary>
        /// 创建游戏世界;
        /// </summary>
        public static void CreateWorld(IGameResourceProvider gameResourceProvider, IWroldResourceProvider wroldResourceProvider)
        {
            if (gameResourceProvider == null)
                throw new ArgumentNullException(nameof(gameResourceProvider));
            if (wroldResourceProvider == null)
                throw new ArgumentNullException(nameof(wroldResourceProvider));

            Resource = gameResourceProvider.GetResource();
            WorldResource = wroldResourceProvider.GetResource();
            World = new World(WorldResource);
            IsCreated = true;
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
