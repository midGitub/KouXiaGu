using System;
using UnityEngine;
using XGame.Running;

namespace XGame
{

    /// <summary>
    /// 旋转;
    /// </summary>
    public abstract class RotateAbstract : MonoBehaviour, IGameObjectState
    {
        protected RotateAbstract() { }

        /// <summary>
        /// 当前物体的旋转角度;
        /// </summary>
        [SerializeField, Tooltip("当前旋转角度;")]
        private Aspect rotation = Aspect.North;

        /// <summary>
        /// 当前旋转方向;
        /// </summary>
        public Aspect Rotation
        {
            get { return rotation; }
            set { SetRotation(value); }
        }

        /// <summary>
        /// 当旋转时调用;
        /// </summary>
        public event Action<Aspect, Aspect> onRotationChangeEvent;

        /// <summary>
        /// 旋转到的这个方向(具体实现方法);
        /// </summary>
        /// <param name="rotation"></param>
        protected abstract void RotateTo(Aspect rotation);

        /// <summary>
        /// 当方向更新时进行旋转;
        /// </summary>
        protected virtual void OnValidate()
        {
            SetRotation(rotation);
        }

        //Test
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        RotateLeft();
        //    }
        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        RotateRigth();
        //    }
        //}

        /// <summary>
        /// 更新委托;
        /// </summary>
        /// <param name="oldRotation"></param>
        /// <param name="newRotation"></param>
        protected void OnRotationChange(Aspect oldRotation, Aspect newRotation)
        {
            if (onRotationChangeEvent != null)
            {
                onRotationChangeEvent(oldRotation, newRotation);
            }
        }

        /// <summary>
        /// 设置旋转到此方向;
        /// </summary>
        /// <param name="newRotation"></param>
        private void SetRotation(Aspect newRotation)
        {
            Aspect oldRotation = newRotation;

            RotateTo(newRotation);
            rotation = newRotation;
            OnRotationChange(oldRotation, newRotation);
        }

        /// <summary>
        /// 逆时针旋转一个单位;
        /// </summary>
        [ContextMenu("RotateLeft")]
        public void RotateLeft()
        {
            Aspect rotation = AspectHelper.GetLeft(this.rotation);
            SetRotation(rotation);
        }

        /// <summary>
        /// 顺时针旋转一个单位;
        /// </summary>
        [ContextMenu("RotateRigth")]
        public void RotateRigth()
        {
            Aspect rotation = AspectHelper.GetRight(this.rotation);
            SetRotation(rotation);
        }

        /// <summary>
        /// 读取传入的状态;
        /// </summary>
        /// <param name="state">状态信息;</param>
        public virtual void Load(GameObjectState state)
        {
            Rotation = state.Rotation;
        }

        /// <summary>
        /// 保存现在的状态信息;
        /// </summary>
        /// <param name="state">状态信息;</param>
        public virtual void Save(GameObjectState state)
        {
            state.Rotation = Rotation;
        }

    }

}
