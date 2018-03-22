using JiongXiaGu.Unity.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JiongXiaGu.Unity.RunTime
{


    public sealed class ModificationController : IGameResourceProvider
    {
        private static readonly ModificationController _default = new ModificationController();
        public static ModificationController Default => _default;

        private static readonly List<ModificationInfo> modificationInfos = new List<ModificationInfo>();
        private static readonly List<ModificationInfo> activatedModificationInfos = new List<ModificationInfo>();
        private static readonly List<Modification> loadedModifications = new List<Modification>();
        private static readonly ModificationFactory modificationFactory = new ModificationFactory();

        /// <summary>
        /// 核心资源;
        /// </summary>
        public static ModificationInfo Core { get; private set; }

        /// <summary>
        /// 所有模组信息,不包括核心资源;
        /// </summary>
        public static IReadOnlyList<ModificationInfo> ModificationInfos => modificationInfos;

        /// <summary>
        /// 所有激活模组信息,不包括核心资源;
        /// </summary>
        public static IReadOnlyList<ModificationInfo> ActivatedModificationInfos => activatedModificationInfos;

        /// <summary>
        /// 已读取的模组,包括核心资源;
        /// </summary>
        public static IReadOnlyList<Modification> Modifications => loadedModifications;

        /// <summary>
        /// 模组数据;
        /// </summary>
        public static GameResource GameResource { get; private set; }

        /// <summary>
        /// 初始化;
        /// </summary>
        internal static void Initialize()
        {
            SearcheAll();
            ReadDefaultLoadOrder();
        }

        /// <summary>
        /// 寻找所有模组;
        /// </summary>
        private static void SearcheAll()
        {
            ModificationFactory factory = new ModificationFactory();

            string directory = Path.Combine(Resource.StreamingAssetsPath, "Data");
            Core = factory.ReadInfo(directory);

            var mods = factory.EnumerateModifications(Resource.ModDirectory);
            modificationInfos.AddRange(mods);

            var userMods = factory.EnumerateModifications(Resource.UserModDirectory);
            modificationInfos.AddRange(userMods);
        }

        /// <summary>
        /// 读取到默认的模组读取顺序;
        /// </summary>
        private static void ReadDefaultLoadOrder()
        {
            try
            {
                ModificationLoadOrderSerializer serializer = new ModificationLoadOrderSerializer();

                ModificationLoadOrder order = serializer.Deserialize();
                if (order.IDList != null)
                {
                    SetLoadOrder(order.IDList);
                }
            }
            catch (FileNotFoundException)
            {
            }
        }

        /// <summary>
        /// 设置为默认的模组读取顺序;
        /// </summary>
        public static void SetDefaultLoadOrder()
        {
            try
            {
                ModificationLoadOrderSerializer serializer = new ModificationLoadOrderSerializer();

                ModificationLoadOrder order = serializer.Deserialize();
                if (order.IDList != null)
                {
                    SetLoadOrder(order.IDList);
                }
                else
                {
                    SetLoadCoreOnly();
                }
            }
            catch (FileNotFoundException)
            {
                SetLoadCoreOnly();
            }
        }

        /// <summary>
        /// 设置只读核心资源;
        /// </summary>
        public static void SetLoadCoreOnly()
        {
            activatedModificationInfos.Clear();
        }

        /// <summary>
        /// 设置模组读取顺序(仅Unity线程调用);
        /// </summary>
        public static void SetLoadOrder(IEnumerable<string> modificationIDs)
        {
            if (modificationIDs == null)
                throw new ArgumentNullException(nameof(modificationIDs));

            activatedModificationInfos.Clear();

            foreach (var id in modificationIDs.Distinct())
            {
                int index = modificationInfos.FindIndex(info => info.Description.ID == id);
                if (index >= 0)
                {
                    ModificationInfo info = ModificationInfos[index];
                    activatedModificationInfos.Add(info);
                }
            }
        }

        /// <summary>
        /// 获取到未启用的模组;
        /// </summary>
        public static void GetIdleModificationInfos(List<ModificationInfo> idleModificationInfos)
        {
            GetIdleModificationInfos(idleModificationInfos, activatedModificationInfos);
        }

        /// <summary>
        /// 获取到未启用的模组;
        /// </summary>
        public static void GetIdleModificationInfos(List<ModificationInfo> idleModificationInfos, IReadOnlyList<ModificationInfo> activeModificationInfos)
        {
            if (idleModificationInfos == null)
                throw new ArgumentNullException(nameof(idleModificationInfos));
            if (activeModificationInfos == null)
                throw new ArgumentNullException(nameof(activeModificationInfos));

            if (activeModificationInfos.Count == 0)
            {
                idleModificationInfos.AddRange(modificationInfos);
            }
            else
            {
                foreach (var modificationInfo in ModificationInfos)
                {
                    if (!activeModificationInfos.Contains(modificationInfo))
                    {
                        idleModificationInfos.Add(modificationInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 所有激活模组信息,包括核心资源;
        /// </summary>
        public static IEnumerable<ModificationInfo> GetActivatedModificationInfos()
        {
            return new ModificationInfo[] { Core }.Concat(activatedModificationInfos);
        }



        GameResource IGameResourceProvider.GetResource()
        {
            if (LoadModification())
            {
                GameResourceFactroy factroy = new GameResourceFactroy();
                GameResource = factroy.Read(loadedModifications);
                return GameResource;
            }
            else
            {
                return GameResource;
            }
        }

        /// <summary>
        /// 激活指定的模组,若未变化则返回false,模组发生变化则返回true;
        /// </summary>
        private static bool LoadModification()
        {
            IEnumerable<ModificationInfo> infos = GetActivatedModificationInfos();

            if (loadedModifications == null)
            {
                foreach (var info in infos)
                {
                    var modification = modificationFactory.Read(info.ModificationDirectory);
                    loadedModifications.Add(modification);
                }

                return true;
            }
            else
            {
                bool isChanged = false;
                List<Modification> newModifications = new List<Modification>();
                int startIndex = 0;

                foreach (var info in infos)
                {
                    var modification = loadedModifications[startIndex];
                    if (Equals(modification, info))
                    {
                        loadedModifications[startIndex] = null;
                        newModifications.Add(modification);
                    }
                    else
                    {
                        isChanged = true;

                        var targetIndex = loadedModifications.FindIndex(startIndex, delegate (Modification value)
                        {
                            if (value == null)
                                return false;

                            return value.Description.ID == info.Description.ID;
                        });

                        if (targetIndex >= 0)
                        {
                            modification = loadedModifications[targetIndex];
                            loadedModifications[targetIndex] = null;
                            newModifications.Add(modification);
                        }
                        else
                        {
                            modification = modificationFactory.Read(info.ModificationDirectory);
                            newModifications.Add(modification);
                        }
                    }
                }

                foreach (var old in loadedModifications)
                {
                    if (old != null)
                    {
                        old.Dispose();
                    }
                }

                loadedModifications.Clear();
                loadedModifications.AddRange(newModifications);

                return isChanged;
            }
        }
    }
}
