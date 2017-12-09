using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using JiongXiaGu.Grids;
using System.Threading;

namespace JiongXiaGu.Unity.RectTerrain
{

    [DisallowMultipleComponent]
    public class LandformDisplayGuider : BufferDisplayGuider
    {
        private LandformDisplayGuider()
        {
        }

        private static readonly List<LandformDisplayGuider> guiders = new List<LandformDisplayGuider>();
        private static ReaderWriterLockSlim asyncLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 枚举所有需要显示的坐标,线程安全;
        /// </summary>
        public static IEnumerable<RectCoord> EnumeratePointsToDisplay()
        {
            using (asyncLock.ReadLock())
            {
                var array = guiders.ToArray();
                foreach (var guider in array)
                {
                    foreach (var point in guider.GetPointsToDisplay())
                    {
                        yield return point;
                    }
                }
            }
        }

        private void OnEnable()
        {
            Add(this);
        }

        private void OnDisable()
        {
            Remove(this);
        }

        private void Update()
        {
            RectCoord chunkPos = transform.position.ToLandformChunkRect();
            SetCenter(chunkPos);
        }

        private void Add(LandformDisplayGuider instance)
        {
            using (asyncLock.WriteLock())
            {
                guiders.Add(instance);
            }
        }

        private void Remove(LandformDisplayGuider instance)
        {
            using (asyncLock.WriteLock())
            {
                guiders.Remove(instance);
            }
        }
    }
}
