using XGame.Running;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 用于初始化物体的类;
    /// 每个允许放置到游戏内的GameObject需要挂载这个组件;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class XGameObject : MonoBehaviour, IGameObjectState
    {

        private XGameObject() { }

        /// <summary>
        /// 在游戏中属于哪个模组,初始化的哪个物体;
        /// 这类物体的类型;
        /// </summary>
        [SerializeField]
        private string objectName;

        /// <summary>
        /// 是否已经加入到游戏了;
        /// </summary>
        [SerializeField]
        private bool m_IsStart = false;

        public bool IsStart { get { return m_IsStart; } }

        /// <summary>
        /// 覆盖父类,当初始化名字时也同时设置该物体的唯一名字;
        /// </summary>
        public new string name
        {
            get { return base.name; }
            set { base.name = value; objectName = value; }
        }

        /// <summary>
        /// 保存状态;
        /// </summary>
        /// <param name="state"></param>
        void IGameObjectState.Save(GameObjectState state)
        {
            state.Position = transform.position;
            state.quaternion = transform.rotation;
            state.ObjectName = objectName;
        }

        /// <summary>
        /// 恢复状态;
        /// </summary>
        /// <param name="state"></param>
        void IGameObjectState.Load(GameObjectState state)
        {
            transform.position = state.Position;
            transform.rotation = state.quaternion;
        }


        /// <summary>
        /// 获取到物体启用关闭接口;
        /// </summary>
        private static IXBehaviour[] GetXBehaviours(XGameObject xGameObject)
        {
            return xGameObject.GetComponentsInChildren<IXBehaviour>(true);
        }

        /// <summary>
        /// 开始该物体的生命(生命周期);
        /// </summary>
        /// <param name="activate"></param>
        /// <param name="prefabInfo"></param>
        public static bool XEnableAll(XGameObject xGameObject)
        {
            if (!xGameObject.m_IsStart)
            {
                CollectionHelper.Operating(GetXBehaviours(xGameObject), preview => preview.XOnEnable());
                xGameObject.m_IsStart = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 结束该物体的生命(生命周期),但是不销毁物体;
        /// </summary>
        public static bool XDisableAll(XGameObject xGameObject)
        {
            if (xGameObject.m_IsStart)
            {
                CollectionHelper.Operating(GetXBehaviours(xGameObject), preview => preview.XOnDisable());
                xGameObject.m_IsStart = false;
                return true;
            }
            return false;
        }

    }

}
