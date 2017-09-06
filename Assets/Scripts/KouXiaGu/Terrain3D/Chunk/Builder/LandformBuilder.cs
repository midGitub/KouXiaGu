using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KouXiaGu.Concurrent;
using KouXiaGu.Grids;
using System.Collections;
using KouXiaGu.World;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 地形创建器;
    /// </summary>
    public class LandformBuilder : ChunkBuilder<RectCoord, LandformChunk>
    {
        public LandformBuilder(IWorldData worldData, IAsyncRequestDispatcher requestDispatcher) : base(requestDispatcher)
        {
            if (worldData == null)
            {
                throw new ArgumentNullException("worldData");
            }

            WorldData = worldData;
            chunkPool = new LandformChunkPool();
        }

        readonly LandformChunkPool chunkPool;
        public IWorldData WorldData { get; private set; }

        class LandformChunkData : ChunkData
        {
            public LandformChunkData(LandformBuilder parent, RectCoord point) : base(parent, point)
            {
                this.parent = parent;
                targets = BakeTargets.None;
            }

            IEnumerator coroutine;
            LandformBuilder parent;
            BakeTargets targets;

            LandformChunkPool chunkPool
            {
                get { return parent.chunkPool; }
            }

            IWorldData worldData
            {
                get { return parent.WorldData; }
            }

            static BakeCamera bakeCamera
            {
                get { return LandformSettings.Instance.bakeCamera; }
            }

            static BakeLandform bakeLandform
            {
                get { return LandformSettings.Instance.bakeLandform; }
            }

            static BakeRoad bakeRoad
            {
                get { return LandformSettings.Instance.bakeRoad; }
            }

            /// <summary>
            /// 更新所有内容;
            /// </summary>
            public override bool UpdateChunk()
            {
                if (base.UpdateChunk())
                {
                    targets = BakeTargets.All;
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 指定更新对应内容;
            /// </summary>
            public bool UpdateChunk(BakeTargets bakeTargets)
            {
                if (base.UpdateChunk())
                {
                    targets = bakeTargets;
                    return true;
                }
                return false;
            }

            protected override bool Prepare()
            {
                if (State == ChunkState.None || State == ChunkState.Completed)
                {
                    return false;
                }
                else if (State == ChunkState.Destroying)
                {
                    coroutine = DestroyCoroutine();
                    return true;
                }
                else
                {
                    coroutine = CreateOrUpdateCoroutine();
                    return true;
                }
            }

            protected override bool Operate()
            {
                return coroutine.MoveNext();
            }

            protected override void OnQuitQueue()
            {
                coroutine = null;
            }

            IEnumerator CreateOrUpdateCoroutine()
            {
                LandformChunk chunk;
                if (State == ChunkState.Creating)
                {
                    chunk = chunkPool.Get();
                    chunk.Position = LandformChunkInfo.ChunkGrid.GetCenter(Point);
                }
                else
                {
                    chunk = Chunk;
                }

                CubicHexCoord chunkCenter = Point.GetChunkHexCenter();
                if ((targets & BakeTargets.Landform) > 0)
                {
                    bakeLandform.BakeCoroutine(bakeCamera, worldData, chunkCenter, Chunk.Renderer);
                    yield return null;
                }
                if ((targets & BakeTargets.Road) > 0)
                {
                    bakeRoad.BakeCoroutine(bakeCamera, worldData, chunkCenter, Chunk.Renderer);
                    yield return null;
                }
                Chunk.Renderer.Apply();
                SetChunk(chunk);
            }

            IEnumerator DestroyCoroutine()
            {
                chunkPool.Release(Chunk);
                RemoveChunk();
                yield break;
            }
        }

        /// <summary>
        /// 地形块池;
        /// </summary>
        class LandformChunkPool : ObjectPool<LandformChunk>
        {
            const int defaultMaxCapacity = 100;

            public LandformChunkPool() : base(defaultMaxCapacity)
            {
            }

            public LandformChunkPool(int capacity) : base(capacity)
            {
            }

            public override LandformChunk Instantiate()
            {
                LandformChunk chunk = LandformChunk.Create();
                return chunk;
            }

            public override void ResetWhenOutPool(LandformChunk chunk)
            {
                chunk.gameObject.SetActive(true);
            }

            public override void ResetWhenEnterPool(LandformChunk chunk)
            {
                chunk.ResetData();
                chunk.gameObject.SetActive(false);
            }

            public override void Destroy(LandformChunk chunk)
            {
                chunk.Destroy();
            }
        }
    }
}
