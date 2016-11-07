//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using XGame.Collections;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 游戏物体构建和存档;
//    /// </summary>
//    public sealed class XObjectManger : Manager<XObjectManger>, IGameSave
//    {
//        /// <summary>
//        /// 初始化次序;
//        /// </summary>
//        [SerializeField]
//        private CallOrder moduleType;

//        [Header("目录")]

//        /// <summary>
//        /// 存放预览物体的目录;
//        /// </summary>
//        [SerializeField]
//        private Transform previewDirectory;

//        /// <summary>
//        /// 已经完成创建的目录;
//        /// </summary>
//        [SerializeField]
//        private Transform insDirectory;

//        protected override XObjectManger This
//        {
//            get { return this; }
//        }

//        CallOrder ICallOrder.CallOrder
//        {
//            get { return moduleType; }
//        }

//        IEnumerator IGameSave.OnLoad(GameSaveInfo info, GameSaveData data)
//        {
//            GameObjectState[] gameObjectStates = data.GameObjectStates;
//            if (gameObjectStates != null)
//            {
//                InstantiateAll(gameObjectStates);
//            }
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        IEnumerator IGameSave.OnSave(GameSaveInfo info, GameSaveData data)
//        {
//            data.GameObjectStates = GetGameObjectStates(insDirectory).ToArray();
//            yield return AsyncHelper.WaitForFixedUpdate;
//        }

//        IEnumerator IGameO.OnStart()
//        {
//            yield break;
//        }

//        void IGameO.GameStart()
//        {
//            return;
//        }

//        /// <summary>
//        /// 实例化Unity物体;并且创建在物体insDirectory之下;
//        /// </summary>
//        /// <param name="original"></param>
//        /// <param name="position"></param>
//        /// <param name="rotation"></param>
//        /// <returns></returns>
//        public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
//        {
//            GameObject gameObject = Instantiate(original, position, rotation, insDirectory) as GameObject;
//            return gameObject;
//        }

//        /// <summary>
//        /// 获取到Unity物体状态,若不存在保存信息,则返回null;
//        /// </summary>
//        /// <param name="transform">需要获取的物体;</param>
//        /// <returns></returns>
//        private static IEnumerable<GameObjectState> GetGameObjectStates(Transform insDirectory)
//        {
//            foreach (Transform insObject in insDirectory)
//            {
//                IGameObjectState[] gameObjectStates = insObject.GetComponents<IGameObjectState>();
//                if (gameObjectStates.Length != 0)
//                {
//                    GameObjectState gameObjectState = new GameObjectState();
//                    CollectionHelper.Operating(gameObjectStates, gameObject => gameObject.Save(gameObjectState));
//                    yield return gameObjectState;
//                }
//            }
//        }

//        /// <summary>
//        /// 实例化Unity物体;并且创建在物体insDirectory之下;
//        /// </summary>
//        /// <param name="gameObjectState">状态信息,用于复原状态;</param>
//        private void InstantiateAll(IEnumerable<GameObjectState> gameObjectStates)
//        {
//            foreach (var gameObjectState in gameObjectStates)
//            {
//                Transform prefabObject = PrefabController.GetInstance.GetPrefab(gameObjectState.ObjectName).transform;
//                GameObject insObject = Instantiate(prefabObject.gameObject, gameObjectState.Position, gameObjectState.quaternion);

//                IGameObjectState[] ObjectStates = insObject.GetComponents<IGameObjectState>();
//                if (ObjectStates.Length != 0)
//                {
//                    CollectionHelper.Operating(ObjectStates, gameObject => gameObject.Load(gameObjectState));
//                }
//                insObject.name = prefabObject.name;
//            }
//        }

//    }

//}
