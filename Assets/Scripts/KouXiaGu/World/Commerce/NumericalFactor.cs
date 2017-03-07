using System;
using System.Collections;
using System.Collections.Generic;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 影响方式;
    /// </summary>
    public enum NumericalFactorMode
    {
        /// <summary>
        /// 仅为标记,不影响其它;
        /// </summary>
        MarkOnly,

        /// <summary>
        /// 按百分百影响;
        /// </summary>
        Percentage,

        /// <summary>
        /// 增加减少数量;
        /// </summary>
        Number,

    }

    /// <summary>
    /// 影响因素;
    /// </summary>
    public interface INumericalFactor
    {

        /// <summary>
        /// 唯一标识;
        /// </summary>
        int ID { get; }

        /// <summary>
        /// 影响方式;
        /// </summary>
        NumericalFactorMode Mode { get; }

        /// <summary>
        /// 若为百分百,则 百分比 * 100,即 70% = 70, -58% = -58;数量则直接为数量;
        /// </summary>
        int Number { get; }

        /// <summary>
        /// 是否允许在其生效时移除?
        /// </summary>
        bool IsRemovable { get; }

        /// <summary>
        /// 主动使其失效时调用;
        /// </summary>
        void OnInvalid(NumericalFactor sender);

    }


    /// <summary>
    /// 影响某个数值的因素;
    /// </summary>
    public class NumericalFactor : IComparer<INumericalFactor>, IEnumerable<INumericalFactor>
    {

        const float DefaultPercentage = 1;
        const int DefaultNumber = 0;

        public NumericalFactor()
        {
            ResetVariable();

            factorsList = new List<INumericalFactor>();
        }

        public NumericalFactor(IEnumerable<INumericalFactor> collections)
        {
            factorsList = new List<INumericalFactor>(collections);
            Recalculate();
        }


        List<INumericalFactor> factorsList;

        /// <summary>
        /// 百分比变化;
        /// </summary>
        public float Percentage { get; private set; }

        /// <summary>
        /// 数值变化;
        /// </summary>
        public int Increment { get; private set; }

        public int Count
        {
            get { return factorsList.Count; }
        }


        /// <summary>
        /// 重置\初始化变量;
        /// </summary>
        void ResetVariable()
        {
            Percentage = DefaultPercentage;
            Increment = DefaultNumber;
        }

        /// <summary>
        /// 重新计算影响效果;
        /// </summary>
        public void Recalculate()
        {
            ResetVariable();

            foreach (var factor in factorsList)
            {
                Valid(factor);
            }
        }


        /// <summary>
        /// 添加一个新的效果;
        /// </summary>
        public IDisposable AddFactor(INumericalFactor factor)
        {
            if (factor == null)
                throw new ArgumentNullException();
            if (factorsList.Contains(factor))
                throw new ArgumentException();

            factorsList.Add(factor);
            Valid(factor);
            return new RemoveFactor(this, factor);
        }

        /// <summary>
        /// 使这个效果生效;
        /// </summary>
        void Valid(INumericalFactor factor)
        {
            switch (factor.Mode)
            {
                case NumericalFactorMode.Percentage:
                    Percentage += factor.Number / 100f;
                    break;

                case NumericalFactorMode.Number:
                    Increment += factor.Number;
                    break;
            }
        }

        /// <summary>
        /// 使这个效果失效;
        /// </summary>
        void Invalid(INumericalFactor factor)
        {
            switch (factor.Mode)
            {
                case NumericalFactorMode.Percentage:
                    Percentage -= factor.Number / 100f;
                    break;

                case NumericalFactorMode.Number:
                    Increment -= factor.Number;
                    break;
            }
        }

        /// <summary>
        /// 尝试清除所有效果,清除成功返回true,否则返回false;
        /// </summary>
        public bool TryClear()
        {
            if (!IsRemovable())
                return false;

            ResetVariable();
            foreach (var factor in factorsList)
            {
                factor.OnInvalid(this);
            }

            return true;
        }

        /// <summary>
        /// 是否已经不存在特殊的因素;
        /// </summary>
        public bool IsRemovable()
        {
            return factorsList.Find(factor => !factor.IsRemovable) == null;
        }


        /// <summary>
        /// 对影响因素进行排序;
        /// </summary>
        public void Sort()
        {
            factorsList.Sort(this);
        }

        /// <summary>
        /// 对影响因素进行排序;
        /// </summary>
        public void Sort(IComparer<INumericalFactor> comparer)
        {
            factorsList.Sort(comparer);
        }

        /// <summary>
        /// 正面效果排在前面,负面排在后面;
        /// </summary>
        int IComparer<INumericalFactor>.Compare(INumericalFactor x, INumericalFactor y)
        {
            return x.Number - y.Number;
        }


        public IEnumerator<INumericalFactor> GetEnumerator()
        {
            return ((IEnumerable<INumericalFactor>)this.factorsList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<INumericalFactor>)this.factorsList).GetEnumerator();
        }


        class RemoveFactor : IDisposable
        {
            public RemoveFactor(NumericalFactor numericalFactor, INumericalFactor factor)
            {
                this.numericalFactor = numericalFactor;
                this.factor = factor;
            }

            NumericalFactor numericalFactor;
            INumericalFactor factor;

            List<INumericalFactor> factorsList
            {
                get { return numericalFactor.factorsList; }
            }

            public void Dispose()
            {
                if (factor != null)
                {
                    factorsList.Remove(factor);
                    numericalFactor.Invalid(factor);

                    numericalFactor = null;
                    factor = null;
                }
            }

        }

    }

}
