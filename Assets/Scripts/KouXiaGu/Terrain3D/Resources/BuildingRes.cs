using System;
using System.Collections;
using System.Collections.Generic;
using KouXiaGu.Collections;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    public sealed class BuildingRes : IEquatable<BuildingRes>, IDisposable
    {

        #region 已初始化合集(静态);

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        static readonly CustomDictionary<int, BuildingRes> initializedDictionary = new CustomDictionary<int, BuildingRes>();

        /// <summary>
        /// 所有已经初始化完毕的道路信息;
        /// </summary>
        public static IReadOnlyDictionary<int, BuildingRes> initializedInstances
        {
            get { return initializedDictionary; }
        }

        public static void Clear()
        {
            foreach (var item in initializedDictionary.Values)
            {
                item.Dispose();
            }
            initializedDictionary.Clear();
        }

        #endregion


        #region 初始化方法;

        public static IEnumerator Load(IEnumerable<BuildingDescr> descriptions, AssetBundle asset)
        {
            foreach (var description in descriptions)
            {
                if (initializedDictionary.ContainsKey(description.ID))
                {
                    Debug.LogWarning("建筑:已经存在相同的ID;跳过此:" + description.ToString());
                    continue;
                }

                #region 读取方式;

                LoadAsync async = new LoadAsync(description, asset);
                yield return async;
                BuildingRes res = async.Res;

                #endregion

                if (res.IsLoadComplete)
                {
                    initializedDictionary.Add(res.ID, res);
                }
                else
                {
                    Debug.LogWarning("建筑:初始化失败,跳过此:" + description.ToString());
                    res.Dispose();
                }
                yield return null;
            }
            yield break;
        }

        class LoadAsync : CustomYieldInstruction
        {
            public LoadAsync(BuildingDescr description, AssetBundle asset)
            {
                this.description = description;
                this.asset = asset;
                Start();
            }

            AssetBundle asset;
            BuildingDescr description;
            AssetBundleRequest prefab;
            public BuildingRes Res { get; private set; }

            public override bool keepWaiting
            {
                get { return KeepWaiting(); }
            }

            void Start()
            {
                prefab = asset.LoadAssetAsync<GameObject>(description.PrefabName);
            }

            bool KeepWaiting()
            {
                if(prefab.isDone)
                {
                    Res = new BuildingRes(description)
                    {
                        Prefab = (GameObject)prefab.asset,
                    };
                    return false;
                }
                return true;
            }

        }

        #endregion


        #region 实例部分;


        BuildingRes(BuildingDescr description)
        {
            this.Description = description;
        }

        public BuildingDescr Description { get; private set; }
        public GameObject Prefab { get; private set; }

        public int ID
        {
            get { return Description.ID; }
        }

        /// <summary>
        /// 是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get { return Prefab == null; }
        }

        /// <summary>
        /// 是否完全读取?
        /// </summary>
        public bool IsLoadComplete
        {
            get { return Prefab != null; }
        }

        public void Dispose()
        {
            if (!IsEmpty)
            {
                GameObject.Destroy(Prefab);
                Prefab = null;
            }
        }

        public bool Equals(BuildingRes other)
        {
            if (other == null)
                return false;
            return Description.Equals(other.Description);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is BuildingRes))
                return false;
            return this.Equals((BuildingRes)obj);
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }

        public override string ToString()
        {
            return Description.ToString();
        }

        #endregion

    }

}
