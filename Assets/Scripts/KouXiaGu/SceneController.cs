using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu
{

    [DisallowMultipleComponent]
    public class SceneController : MonoBehaviour
    {
        SceneController()
        {
        }

        [CustomUnityTag("场景控制器标签;")]
        public const string Tag = "SceneController";
        static GameObject sceneController;

        void Awake()
        {
            if (sceneController != null)
            {
                Debug.LogError("场景已经存在 SceneController:" + sceneController.ToString() + ";尝试加入新的:" + ToString());
            }
            else
            {
                sceneController = gameObject;
            }
        }

        void OnDestroy()
        {
            sceneController = null;
        }

        public static T GetSington<T>()
            where T : class
        {
            if (sceneController != null)
            {
                return sceneController.GetComponentInChildren<T>();
            }
            return default(T);
        }
    }
}
