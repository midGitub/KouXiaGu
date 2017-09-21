using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 场景控制器;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SceneController : SceneSington<SceneController>
    {

        /// <summary>
        /// 获取到挂载在场景控制器上面的组件;
        /// </summary>
        public static TComponent GetSington<TComponent>()
            where TComponent : class
        {
            if (Instance != null)
            {
                return Instance.GetComponentInChildren<TComponent>();
            }
            return default(TComponent);
        }
    }
}
