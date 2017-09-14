using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.RectTerrain
{

    /// <summary>
    /// 显示合集\组;
    /// </summary>
    public abstract class TerrainGuiderGroup<TPoint> : IDisplayGuider<TPoint>
    {
        public TerrainGuiderGroup()
        {
        }

        public TerrainGuiderGroup(TerrainGuiderGroup<TPoint> guiderGroup)
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

        IEnumerable<TPoint> IDisplayGuider<TPoint>.GetPointsToDisplay()
        {
            return GetPointsToDisplay();
        }

        public class GuiderGroup_HashSet : TerrainGuiderGroup<TPoint>
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

        public class GuiderGroup_List : TerrainGuiderGroup<TPoint>
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
