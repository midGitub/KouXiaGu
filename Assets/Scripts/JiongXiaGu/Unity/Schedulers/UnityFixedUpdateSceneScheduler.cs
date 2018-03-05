using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Schedulers
{

    /// <summary>
    /// 数据同步管理;
    /// </summary>
    public class UnityFixedUpdateSceneScheduler : MonoBehaviour
    {
        private static readonly SceneSingleton<UnityFixedUpdateSceneScheduler> singleton = new SceneSingleton<UnityFixedUpdateSceneScheduler>();
        public static UnityFixedUpdateSceneScheduler SceneInstance => singleton.GetInstance();

        private void Awake()
        {
            singleton.SetInstance(this);
        }

        private void OnDestroy()
        {
            singleton.RemoveInstance(this);
        }


        /// <summary>
        /// 首次更新;
        /// </summary>
        internal void FirstUpdate()
        {
            Debug.Log("1FirstUpdate");
        }

        /// <summary>
        /// 最后更新;
        /// </summary>
        internal void FinalUpdate()
        {
            Debug.Log("2FinalUpdate");
        }
    }
}
