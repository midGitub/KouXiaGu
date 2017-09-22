using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.World.Map;
using JiongXiaGu.World.TimeSystem;
using UnityEngine;
using System.Collections;

namespace JiongXiaGu.World
{

    public class WroldData
    {
        public WroldData()
        {
        }

        public IGameResource BasicResource { get; private set; }
        public WorldMap MapData { get; private set; }
        public WorldTime Time { get; private set; }
    }

    /// <summary>
    /// 游戏数据初始化;
    /// </summary>
    [DisallowMultipleComponent]
    [Obsolete]
    public class WorldDataInitializer : MonoBehaviour, IWorldData
    {
        WorldDataInitializer()
        {
        }

        public bool IsCompleted { get; private set; }
        public IBasicData BasicData { get; private set; }
        public WorldMap MapData { get; private set; }
        public WorldTime Time { get; private set; }

        void Start()
        {
            StartCoroutine(WaitComplete());
        }

        IEnumerator WaitComplete()
        {
            while (!IsCompleted)
            {
                yield return null;
            }

            foreach (var item in GetComponentsInChildren<IComponentInitializeHandle>())
            {
                //item.WorldDataCompleted(this);
            }
            Debug.Log("游戏数据初始化完成;");
        }
    }
}
