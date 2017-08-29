using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 显示合集\组;
    /// </summary>
    public abstract class GuiderGroup<TPoint> : IGuider<TPoint>
    {
        public GuiderGroup()
        {
        }

        public GuiderGroup(GuiderGroup<TPoint> guiderGroup)
        {

        }

        protected abstract ICollection<IGuider<TPoint>> guiderCollection { get; }
        protected abstract ICollection<TPoint> pisplayPointCollection { get; }

        public virtual void Add(IGuider<TPoint> guider)
        {
            if (!guiderCollection.Contains(guider))
            {
                guiderCollection.Add(guider);
            }
        }

        public virtual bool Remove(IGuider<TPoint> guider)
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

        IEnumerable<TPoint> IGuider<TPoint>.GetPointsToDisplay()
        {
            return GetPointsToDisplay();
        }

        public class GuiderGroup_HashSet : GuiderGroup<TPoint>
        {
            public GuiderGroup_HashSet()
            {
                guiderList = new List<IGuider<TPoint>>();
                pisplayPoints = new HashSet<TPoint>();
            }

            List<IGuider<TPoint>> guiderList;
            HashSet<TPoint> pisplayPoints;

            protected override ICollection<IGuider<TPoint>> guiderCollection
            {
                get { return guiderList; }
            }

            protected override ICollection<TPoint> pisplayPointCollection
            {
                get { return pisplayPoints; }
            }
        }

        public class GuiderGroup_List : GuiderGroup<TPoint>
        {
            public GuiderGroup_List()
            {
                guiderList = new List<IGuider<TPoint>>();
                pisplayPoints = new List<TPoint>();
            }

            List<IGuider<TPoint>> guiderList;
            List<TPoint> pisplayPoints;

            protected override ICollection<IGuider<TPoint>> guiderCollection
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
