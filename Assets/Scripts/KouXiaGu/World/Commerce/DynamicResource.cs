using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Commerce
{


    public class DynamicResource
    {

        const int DefaultTotalNumber = 0;
        const int DefaultDemandNumber = 0;


        public DynamicResource()
        {
            this.Total = DefaultTotalNumber;
            this.Demand = DefaultDemandNumber;
            branchList = new SortedList<Branch>();
        }


        /// <summary>
        /// 按优先级排序的
        /// </summary>
        SortedList<Branch> branchList;

        /// <summary>
        /// 总数;
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// 总需求;
        /// </summary>
        public int Demand { get; private set; }

        /// <summary>
        /// 剩余数;
        /// </summary>
        public int Remaining
        {
            get { return Total - Demand; }
        }


        /// <summary>
        ///  获取到对应资源;
        /// </summary>
        /// <param name="priority">越高越优先;</param>
        /// <param name="demand">需求;</param>
        public IDynamicResource<int> GetResource(int priority, int demand)
        {
            var resource = new Branch(this, priority, demand);
            branchList.Add(resource);
            return resource;
        }

        /// <summary>
        /// 重新分配资源;
        /// </summary>
        void Redistribute()
        {
            int total = Total;

            foreach (var branch in branchList)
            {
                branch.Practice = Math.Min(branch.Demand, total);
                total -= branch.Demand;

                if (total <= 0)
                    break;
            }
        }


        /// <summary>
        /// 分配单元;
        /// </summary>
        class Branch : IDynamicResource<int>, IComparable<Branch>
        {

            public Branch(DynamicResource manager, int priority, int demand)
            {
                this.Manager = manager;
                this.Priority = priority;
                this.Demand = 0;
                this.Practice = 0;

                SetDemand(demand);
            }

            /// <summary>
            /// 优先级;越高越优先;
            /// </summary>
            public int Priority { get; private set; }

            /// <summary>
            /// 需求;
            /// </summary>
            public int Demand { get; private set; }

            /// <summary>
            /// 实际分配;
            /// </summary>
            public int Practice { get; set; }

            public DynamicResource Manager { get; private set; }


            /// <summary>
            /// 设置新的需求;
            /// </summary>
            public void SetDemand(int newDemand)
            {
                Manager.Demand += newDemand - Demand;
                Demand = newDemand;
                Manager.Redistribute();
            }

            public int CompareTo(Branch other)
            {
                return other.Priority - Priority;
            }

        }

    }

}
