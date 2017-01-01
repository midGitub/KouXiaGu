using System;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 挂载于需要导航的物体;
    /// </summary>
    [DisallowMultipleComponent]
    public class NavObject : MonoBehaviour
    {
        protected NavObject() { }

        [SerializeField]
        float maxSpeed = 1;

        [SerializeField]
        float smoothTime = 0.3f;

        /// <summary>
        /// 在跟随路径时,当与目标点相距多少距离进行跟换一下个目标点;
        /// </summary>
        [SerializeField, Range(0.01f, GridConvert.OuterRadius)]
        float alternate = 0.3f;

        /// <summary>
        /// 下一次行动到的目标点;
        /// </summary>
        [SerializeField]
        Vector3 targetPoint;

        /// <summary>
        /// 当前的速度;
        /// </summary>
        Vector3 velocity;

        /// <summary>
        /// 跟随的线路;
        /// </summary>
        INavigationPath path;

        /// <summary>
        /// 是否正在跟随路线而行动;
        /// </summary>
        public bool IsFollowPath
        {
            get { return path != null; }
        }

        /// <summary>
        /// 当前的速度;
        /// </summary>
        public Vector3 Velocity
        {
            get { return velocity; }
        }

        /// <summary>
        /// 这只导航路径,若已经存在导航路径则替换;
        /// </summary>
        public void Follow(INavigationPath path)
        {
            if (path == null)
                throw new ArgumentNullException("空的导航路径;");
            if(IsFollowPath)
                CompletePath();

            this.path = path;
            MoveNext();
            enabled = true;
        }

        void Awake()
        {
            if (path == null)
                enabled = false;
        }

        void FixedUpdate()
        {
            Vector3 current = transform.position;
            Vector3 next = Vector3.SmoothDamp(current, targetPoint, ref velocity, smoothTime, maxSpeed, Time.deltaTime);

            next.y = GetHeight(next);
            transform.position = next;

            if (IsClose(current, next))
            {
                MoveNext();
            }
        }

        /// <summary>
        /// 这两个点是否足够接近;
        /// </summary>
        bool IsClose(Vector3 point1, Vector3 point2)
        {
            return (Vector3.Distance(point1, point2) < alternate);
        }

        /// <summary>
        /// 设置到下一步;
        /// </summary>
        void MoveNext()
        {
            if (path.MoveNext())
            {
                maxSpeed = path.MaxSpeed;
                targetPoint = path.Position;
            }
            else
            {
                Stop();
            }
        }

        /// <summary>
        /// 停止跟随路径;
        /// </summary>
        public void Stop()
        {
            CompletePath();
            enabled = false;
        }

        /// <summary>
        /// 获取到这个点的高度;
        /// </summary>
        float GetHeight(Vector3 point)
        {
            return TerrainData.GetHeight(point);
        }

        /// <summary>
        /// 完成路线;
        /// </summary>
        void CompletePath()
        {
            path.Complete();
            path = null;
        }

    }

}
