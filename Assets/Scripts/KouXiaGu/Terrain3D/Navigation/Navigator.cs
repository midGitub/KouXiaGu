using System;
using UnityEngine;
using System.Collections;

namespace KouXiaGu.Terrain3D.Navigation
{

    /// <summary>
    /// 挂载于需要导航的物体;
    /// 路径点在协程内更新;
    /// </summary>
    [DisallowMultipleComponent]
    public class Navigator : MonoBehaviour, INavigator
    {
        const float smoothTime = 0.3f;

        protected Navigator() { }

        float maxSpeed = 1;

        /// <summary>
        /// 在跟随路径时,当与目标点相距多少距离进行跟换一下个目标点;
        /// </summary>
        [SerializeField, Range(0.01f, TerrainConvert.OuterRadius)]
        float alternateDistance = 0.3f;

        /// <summary>
        /// 下一次行动到的目标点;
        /// </summary>
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
        /// 目标点更新协程;
        /// </summary>
        UnityEngine.Coroutine targetPointUpdateCoroutine;

        public float AlternateDistance
        {
            get { return alternateDistance; }
            set { alternateDistance = value; }
        }

        public Vector3 Position
        {
            get { return transform.position; }
        }

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

        protected virtual void Update()
        {
            Vector3 pos = transform.position;

            pos.y = GetHeight(pos);

            transform.position = pos;
        }

        /// <summary>
        /// 获取到这个点的高度;
        /// </summary>
        float GetHeight(Vector3 point)
        {
            return TerrainData.GetHeight(point);
        }

        protected virtual void FixedUpdate()
        {
            Vector3 current = transform.position;
            transform.position = Vector3.SmoothDamp(current, targetPoint, ref velocity, smoothTime, maxSpeed, Time.deltaTime);
        }

        /// <summary>
        /// 这只导航路径,若已经存在导航路径则替换;
        /// </summary>
        public void Follow(INavigationPath path)
        {
            if (path == null)
                throw new ArgumentNullException("空的导航路径;");

            Stop();

            this.path = path;

            targetPointUpdateCoroutine = StartCoroutine(UpdateTargetPoint());
        }

        /// <summary>
        /// 停止跟随路径;
        /// </summary>
        [ContextMenu("StopFollow")]
        public void Stop()
        {
            if (IsFollowPath)
            {
                StopCoroutine(targetPointUpdateCoroutine);
                CompletePath();
            }
        }

        /// <summary>
        /// 更新目标点;
        /// </summary>
        IEnumerator UpdateTargetPoint()
        {
            while (path.MoveNext())
            {
                maxSpeed = path.MaxSpeed;
                targetPoint = path.Position;

                while (!IsClose(transform.position, targetPoint))
                    yield return null;
            }

            CompletePath();
            targetPointUpdateCoroutine = null;
            yield break;
        }

        /// <summary>
        /// 这两个点是否足够接近;
        /// </summary>
        bool IsClose(Vector3 point1, Vector3 point2)
        {
            return (Vector3.Distance(point1, point2) < alternateDistance);
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
