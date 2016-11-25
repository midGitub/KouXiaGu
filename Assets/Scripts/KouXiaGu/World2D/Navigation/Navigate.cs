using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu.World2D.Navigation
{

    /// <summary>
    /// 提供解读地图路径信息,在地图上行走的方法;
    /// </summary>
    [DisallowMultipleComponent]
    public class Navigate : MonoBehaviour
    {
        Navigate() { }

        /// <summary>
        /// 移动的最大速度;
        /// </summary>
        [SerializeField]
        float maxSpeed = 1;

        /// <summary>
        /// 
        /// </summary>
        [SerializeField]
        float smoothTime = 0.2f;

        /// <summary>
        /// 在跟随路径时,当与目标点相距多少距离进行跟换一下个目标点;
        /// </summary>
        [SerializeField, Range(0.01f, WorldConvert.HexOuterDiameter)]
        float alternate = 0.3f;

        /// <summary>
        /// 下一次行动到的目标点;
        /// </summary>
        [SerializeField]
        Vector2 targetPoint;

        /// <summary>
        /// 当前的速度;
        /// </summary>
        Vector2 velocity;

        /// <summary>
        /// 更随路线行动协程;
        /// </summary>
        IDisposable followPathCoroutine;

        /// <summary>
        /// 是否正在跟随路线而行动;
        /// </summary>
        public bool IsFollowPath { get { return followPathCoroutine != null; } }

        /// <summary>
        /// 当前的速度;
        /// </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
        }

        /// <summary>
        /// 移动的最大速度;
        /// </summary>
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        /// <summary>
        /// 更新挂在物体位置;
        /// </summary>
        void FixedUpdate()
        {
            transform.position = Vector2.SmoothDamp(transform.position, targetPoint, ref velocity, smoothTime, maxSpeed);
        }

        /// <summary>
        /// 开始跟随路径点移动到,不做起点检查;
        /// </summary>
        public void StartFollowWayPath(NavPath navPath)
        {
            if (IsFollowPath)
            {
                this.followPathCoroutine.Dispose();
            }
            this.followPathCoroutine = Observable.FromMicroCoroutine(() => FollowWayPath(navPath)).Subscribe();
        }

        /// <summary>
        /// 更随路径行走到;
        /// </summary>
        IEnumerator FollowWayPath(NavPath navPath)
        {
            while(navPath.TryGoNext(out this.targetPoint, out maxSpeed))
            {
                while (!IsClose(transform.position, this.targetPoint))
                {
                    yield return null;
                }
            }
        }

        /// <summary>
        /// 这两个点是否足够接近;
        /// </summary>
        bool IsClose(Vector2 point1, Vector2 point2)
        {
            return (Vector2.Distance(point1, point2) < alternate);
        }

        /// <summary>
        /// 地图坐标转换成平面坐标;
        /// </summary>
        Vector2 MapPointToPlanePoint(ShortVector2 mapPoint)
        {
            return WorldConvert.MapToHex(mapPoint);
        }

    }

}
