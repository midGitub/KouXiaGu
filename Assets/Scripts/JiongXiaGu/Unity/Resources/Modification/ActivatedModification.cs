using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Collections;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// 表示激活的模组;
    /// </summary>
    public class ActivatedModification
    {
        public ModificationFactory Factory { get; private set; }
        private List<Modification> modifications;
        public IReadOnlyList<Modification> Modifications => modifications;

        public ActivatedModification()
        {
            Factory = new ModificationFactory();
            modifications = new List<Modification>();
        }

        public ActivatedModification(IEnumerable<ModificationInfo> infos) : this()
        {
            Activate(infos);
        }

        /// <summary>
        /// 激活指定的模组,若未变化则返回false,模组发生变化则返回true;
        /// </summary>
        public bool Activate(IEnumerable<ModificationInfo> infos)
        {
            if (infos == null)
                throw new ArgumentNullException(nameof(infos));

            bool isChanged = false;
            List<Modification> newModifications = new List<Modification>();
            int startIndex = 0;

            foreach (var info in infos)
            {
                var modification = modifications[startIndex];
                if (Equals(modification, info))
                {
                    newModifications.Add(modification);
                }
                else
                {
                    isChanged = true;

                    var targetIndex = modifications.FindIndex(startIndex, Equals);
                    if (targetIndex >= 0)
                    {
                        modification = modifications[targetIndex];
                        modifications[targetIndex] = null;
                        newModifications.Add(modification);
                    }
                    else
                    {
                        modification = Load(info);
                        newModifications.Add(modification);
                    }
                }
            }

            foreach (var old in modifications)
            {
                if (old != null)
                {
                    old.Dispose();
                }
            }

            modifications.Clear();
            modifications.AddRange(newModifications);

            return isChanged;
        }

        private Modification Load(ModificationInfo info)
        {
            return Factory.Read(info.ModificationDirectory);
        }

        private bool Equals(Modification modification, ModificationInfo info)
        {
            if (modification == null)
                return false;

            return modification.Description.ID == info.Description.ID;
        }
    }
}
