using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    /// <summary>
    /// 显示合集\组;
    /// </summary>
    public abstract class OTerrainGuiderGroup<TPoint> : IDisplayGuider<TPoint>
    {
        public OTerrainGuiderGroup()
        {
        }

        protected abstract ICollection<IDisplayGuider<TPoint>> guiderCollection { get; }
        protected abstract ICollection<TPoint> pisplayPointCollection { get; }

        public virtual void Add(IDisplayGuider<TPoint> guider)
        {
            if (!guiderCollection.Contains(guider))
            {
                guiderCollection.Add(guider);
            }
        }

        public virtual bool Remove(IDisplayGuider<TPoint> guider)
        {
            return guiderCollection.Remove(guider);
        }

        public virtual IReadOnlyCollection<TPoint> GetPointsToDisplay()
        {
            pisplayPointCollection.Clear();
            foreach (var chunkGuider in guiderCollection)
            {
                foreach (var display in chunkGuider.GetPointsToDisplay())
                {
                    pisplayPointCollection.Add(display);
                }
            }
            return pisplayPointCollection.AsReadOnlyCollection();
        }

        IReadOnlyCollection<TPoint> IDisplayGuider<TPoint>.GetPointsToDisplay()
        {
            return GetPointsToDisplay();
        }

        public class GuiderGroup_HashSet : OTerrainGuiderGroup<TPoint>
        {
            public GuiderGroup_HashSet()
            {
                guiderList = new List<IDisplayGuider<TPoint>>();
                pisplayPoints = new HashSet<TPoint>();
            }

            List<IDisplayGuider<TPoint>> guiderList;
            HashSet<TPoint> pisplayPoints;

            protected override ICollection<IDisplayGuider<TPoint>> guiderCollection
            {
                get { return guiderList; }
            }

            protected override ICollection<TPoint> pisplayPointCollection
            {
                get { return pisplayPoints; }
            }
        }

        public class GuiderGroup_List : OTerrainGuiderGroup<TPoint>
        {
            public GuiderGroup_List()
            {
                guiderList = new List<IDisplayGuider<TPoint>>();
                pisplayPoints = new List<TPoint>();
            }

            List<IDisplayGuider<TPoint>> guiderList;
            List<TPoint> pisplayPoints;

            protected override ICollection<IDisplayGuider<TPoint>> guiderCollection
            {
                get { return guiderList; }
            }

            protected override ICollection<TPoint> pisplayPointCollection
            {
                get { return pisplayPoints; }
            }
        }
    }
}
