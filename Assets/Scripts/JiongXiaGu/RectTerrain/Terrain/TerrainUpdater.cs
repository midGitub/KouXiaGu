using System.Collections.Generic;
using System.Linq;

namespace JiongXiaGu.RectTerrain
{
    /// <summary>
    /// 地形更新器;
    /// </summary>
    public class TerrainUpdater<TPoint, TChunk>
    {
        public TerrainUpdater(TerrainBuilder<TPoint, TChunk> builder) : this(builder, new TerrainGuiderGroup<TPoint>())
        {
        }

        public TerrainUpdater(TerrainBuilder<TPoint, TChunk> builder, TerrainGuiderGroup<TPoint> guiderGroup)
        {
            Builder = builder;
            GuiderGroup = guiderGroup;
            needCreatePoints = new List<TPoint>();
            needDestoryPoints = new List<TPoint>();
            needUpdatePoints = new List<TPoint>();
        }

        protected ICollection<TPoint> needCreatePoints;
        protected ICollection<TPoint> needDestoryPoints;
        protected ICollection<TPoint> needUpdatePoints;
        public TerrainBuilder<TPoint, TChunk> Builder { get; private set; }
        public TerrainGuiderGroup<TPoint> GuiderGroup { get; private set; }

        public void Update()
        {
            GetPointsToCreate(ref needCreatePoints);
            GetPointsToDestory(ref needDestoryPoints);
            GetPointsToUpdate(ref needUpdatePoints);
            Apply();
        }

        /// <summary>
        /// 设置需要创建的点;
        /// </summary>
        protected virtual void GetPointsToCreate(ref ICollection<TPoint> needCreatePoints)
        {
            GuiderGroup.GetPointsToDisplay(ref needCreatePoints);
        }

        /// <summary>
        /// 设置需要销毁的点;
        /// </summary>
        protected virtual void GetPointsToDestory(ref ICollection<TPoint> needDestoryPoints)
        {
            foreach (var chunk in Builder.Chunks)
            {
                if (!needCreatePoints.Contains(chunk.Point))
                {
                    needDestoryPoints.Add(chunk.Point);
                }
            }
        }

        /// <summary>
        /// 设置需要更新的点;
        /// </summary>
        protected virtual void GetPointsToUpdate(ref ICollection<TPoint> needUpdatePoints)
        {
        }

        /// <summary>
        /// 将坐标应用到构建器;
        /// </summary>
        protected void Apply()
        {
            foreach (var destoryPoint in needDestoryPoints)
            {
                Builder.DestroyAsync(destoryPoint);
            }
            needDestoryPoints.Clear();

            foreach (var createPoint in needCreatePoints)
            {
                Builder.CreateAsync(createPoint);
            }
            needCreatePoints.Clear();

            foreach (var updatePoint in needUpdatePoints)
            {
                Builder.UpdateAsync(updatePoint);
            }
            needUpdatePoints.Clear();
        }
    }
}
