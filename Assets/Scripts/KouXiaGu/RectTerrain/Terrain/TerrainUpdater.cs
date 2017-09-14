using System.Collections.Generic;
using System.Linq;

namespace KouXiaGu.RectTerrain
{
    /// <summary>
    /// 地形更新器;
    /// </summary>
    public class TerrainUpdater<TPoint, TChunk>
    {
        public TerrainUpdater(TerrainBuilder<TPoint, TChunk> builder, TerrainGuiderGroup<TPoint> guiderGroup)
        {
            Builder = builder;
            GuiderGroup = guiderGroup;
            needDestoryPoints = new List<TPoint>();
        }

        public TerrainBuilder<TPoint, TChunk> Builder { get; private set; }
        public TerrainGuiderGroup<TPoint> GuiderGroup { get; private set; }
        List<TPoint> needDestoryPoints;

        public void Update()
        {
            IReadOnlyCollection<TPoint> needDisplayPoints = GuiderGroup.GetPointsToDisplay();

            foreach (var chunk in Builder.Chunks)
            {
                if (!needDisplayPoints.Contains(chunk.Point))
                {
                    needDestoryPoints.Add(chunk.Point);
                }
            }

            foreach (var destoryPoint in needDestoryPoints)
            {
                Builder.DestroyAsync(destoryPoint);
            }
            needDestoryPoints.Clear();

            foreach (var createPoint in needDisplayPoints)
            {
                Builder.CreateAsync(createPoint);
            }
        }
    }
}
