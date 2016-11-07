//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 障碍物;
//    /// </summary>
//    public class Obstacle : MonoBehaviour, IObstacle, IXBehaviour
//    {

//        /// <summary>
//        /// 旋转组件;
//        /// </summary>
//        [SerializeField]
//        protected RotationAbstract rotation;

//        /// <summary>
//        /// 障碍物特性;
//        /// </summary>
//        [SerializeField]
//        protected ObstacleMode obstacleMode = ObstacleMode.Static;

//        /// <summary>
//        /// 存在障碍物的方向;
//        /// </summary>
//        [SerializeField]
//        private Aspect2D aspect = Aspect2D.North;

//        /// <summary>
//        /// 已经加入到了寻路组件;
//        /// </summary>
//        private bool m_AddInNavigator = false;

//        /// <summary>
//        /// 是否在在旋转时初始化数据;
//        /// </summary>
//        private bool m_RotationReset = false;

//        /// <summary>
//        /// 导航类;
//        /// </summary>
//        protected Navigator navigator
//        {
//            get { return Navigator.GetInstance; }
//        }

//        /// <summary>
//        /// 加入的位置;
//        /// </summary>
//        protected IntVector2 position
//        {
//            get { return (IntVector2)transform.position; }
//        }

//        /// <summary>
//        /// 障碍物加入的方向;
//        /// </summary>
//        public Aspect2D AspectObstacle
//        {
//            get { return aspect; }
//            set { rotation.Rotation = value; }
//        }

//        /// <summary>
//        /// 障碍物属性\特性;
//        /// </summary>
//        ObstacleMode IObstacle.ObstacleMode
//        {
//            get { return obstacleMode; }
//        }

//        /// <summary>
//        /// Unity函数;
//        /// </summary>
//        protected virtual void Awake()
//        {
//            if (rotation != null)
//            {
//                rotation.onRotationChangeEvent += OnRotationChange;
//            }
//        }

//        /// <summary>
//        /// Unity函数;
//        /// </summary>
//        protected virtual void OnDestroy()
//        {
//            if (rotation != null)
//            {
//                rotation.onRotationChangeEvent -= OnRotationChange;
//            }
//        }

//        ///// <summary>
//        ///// Unity函数;
//        ///// </summary>
//        //protected virtual void OnEnable()
//        //{
//        //        AddObstacle();
//        //}

//        ///// <summary>
//        ///// Unity函数;
//        ///// </summary>
//        //protected virtual void OnDisable()
//        //{
//        //        RemoveObstacle();
//        //}

//        /// <summary>
//        /// 注册障碍物;
//        /// </summary>
//        protected virtual void AddObstacle()
//        {
//            if (!m_AddInNavigator)
//            {
//                try
//                {
//                    navigator.Add(position, aspect, this);
//                    m_AddInNavigator = true;
//                }
//                catch (Exception e)
//                {
//                    Exception(e);
//                }
//            }
//        }

//        /// <summary>
//        /// 撤销障碍物;
//        /// </summary>
//        protected virtual void RemoveObstacle()
//        {
//            if (m_AddInNavigator)
//            {
//                navigator.Remove(position, aspect, this);
//                m_AddInNavigator = false;
//            }
//        }

//        /// <summary>
//        /// 当发生异常时调用;
//        /// </summary>
//        /// <param name="e">异常</param>
//        protected virtual void Exception(Exception e)
//        {
//            Debug.Log(e);
//            //Destroy(gameObject);
//        }

//        /// <summary>
//        /// 当旋转时调用;
//        /// </summary>
//        protected virtual void OnRotationChange(Aspect2D oldRotation, Aspect2D newRotation)
//        {
//            //当标记销毁时停止调用;
//            if (transform == null)
//                return;

//            m_RotationReset = m_AddInNavigator;

//            if(m_RotationReset)
//                RemoveObstacle();

//            OnRotationChange_Direction(oldRotation, newRotation);

//            if(m_RotationReset)
//                AddObstacle();
//        }

//        /// <summary>
//        /// 旋转时改变障碍物方向;
//        /// </summary>
//        /// <param name="oldRotation"></param>
//        /// <param name="newRotation"></param>
//        private void OnRotationChange_Direction(Aspect2D oldRotation, Aspect2D newRotation)
//        {
//            aspect = AspectHelper.TransfromDirection(oldRotation, newRotation, aspect);
//        }

//        /// <summary>
//        /// 查询是否允许通过这个障碍物;
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns>允许通过为true,否则为false;</returns>
//        public virtual bool IsOpen(IKey key)
//        {
//            return false;
//        }

//        /// <summary>
//        /// 设置为预览模式;
//        /// </summary>
//        /// <param name="prefabInfo"></param>
//        public virtual void XOnEnable()
//        {
//            AddObstacle();
//        }

//        /// <summary>
//        /// 取消设置为预览模式;
//        /// </summary>
//        public virtual void XOnDisable()
//        {
//            RemoveObstacle();
//        }

//        /// <summary>
//        /// 隐藏障碍物;
//        /// </summary>
//        public virtual void Concealment()
//        {
//            return;
//        }

//        /// <summary>
//        /// 显示障碍物;
//        /// </summary>
//        public virtual void Show()
//        {
//            return;
//        }

//    }

//}
