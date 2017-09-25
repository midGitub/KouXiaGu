using System.Collections.Generic;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 显示组;
    /// </summary>
    public class TerrainGuiderGroup<TPoint>
    {
        public TerrainGuiderGroup()
        {
            guiderCollection = new List<IDisplayGuider<TPoint>>();
        }

        List<IDisplayGuider<TPoint>> guiderCollection;

        public bool Add(IDisplayGuider<TPoint> guider)
        {
            if (!guiderCollection.Contains(guider))
            {
                guiderCollection.Add(guider);
                return true;
            }
            return false;
        }

        public bool Remove(IDisplayGuider<TPoint> guider)
        {
            return guiderCollection.Remove(guider);
        }

        /// <summary>
        /// 获取到需要显示的坐标;
        /// </summary>
        public void GetPointsToDisplay(ref ICollection<TPoint> displayPoints)
        {
            foreach (var chunkGuider in guiderCollection)
            {
                foreach (var display in chunkGuider.GetPointsToDisplay())
                {
                    if (!displayPoints.Contains(display))
                    {
                        displayPoints.Add(display);
                    }
                }
            }
        }
    }
}
