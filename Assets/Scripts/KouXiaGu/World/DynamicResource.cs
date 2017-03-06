using System;
using System.Collections.Generic;
using KouXiaGu.Collections;

namespace KouXiaGu.World
{

    /// <summary>
    /// 动态分配的资源;
    /// </summary>
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
        /// 构造函数;
        /// </summary>
        /// <param name="number">资源总数;</param>
        public DynamicResource(int number)
        {
            this.Total = number;
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
            Redistribute();
            return resource;
        }

        /// <summary>
        /// 设置新的资源总数;
        /// </summary>
        public void SetTotal(int number)
        {
            if (number == Total)
            {
                return;
            }

            if (number < Demand || (Total < Demand && number >= Demand))
            {
                this.Total = number;
                Redistribute();
            }
            else
            {
                this.Total = number;
            }
        }

        /// <summary>
        /// 重新分配资源;
        /// </summary>
        void Redistribute()
        {
            if (branchList.Count == 0)
            {
                return;
            }

            ClearPractice();

            var firstBranch = branchList[0];
            int total = Total;
            int priority = firstBranch.Priority;
            int demand = firstBranch.Demand;

            List<Branch> branchs = new List<Branch>();
            branchs.Add(firstBranch);

            for (int i = 1; i < branchList.Count; i++)
            {
                var branch = branchList[i];

                if (branch.Priority != priority)
                {
                    if (demand <= total)
                    {
                        total = Fill(branchs, total);

                        if (total <= 0)
                        {
                            return;
                        }
                    }
                    else
                    {
                        Average(branchs, total);
                        return;
                    }

                    branchs.Clear();
                    demand = 0;
                    priority = branch.Priority;
                }

                branchs.Add(branch);
                demand += branch.Demand;
            }

            if (demand <= total)
            {
                Fill(branchs, total);
            }
            else
            {
                Average(branchs, total);
            }

        }

        /// <summary>
        /// 清空所有分配内容;
        /// </summary>
        void ClearPractice()
        {
            foreach (var branch in branchList)
            {
                branch.Practice = 0;
            }
        }

        /// <summary>
        /// 先满足前面的需求,若不足够则无视后面的需求;
        /// </summary>
        /// <returns>剩余数;</returns>
        int Fill(IEnumerable<Branch> branchs, int number)
        {
            foreach (var branch in branchs)
            {
                branch.Practice = Math.Min(branch.Demand, number);
                number -= branch.Demand;

                if (number <= 0)
                {
                    return 0;
                }
            }
            return number;
        }

        /// <summary>
        /// 平均分; number 需要 小余或等于 总需求;
        /// </summary>
        void Average(IEnumerable<Branch> branchs, int number)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException();

            while (true)
            {
                foreach (var branch in branchs)
                {
                    if (branch.Practice < branch.Demand)
                    {
                        branch.Practice++;
                        number--;

                        if (number <= 0)
                        {
                            goto End;
                        }
                    }
                }
            }
            End:
            return;
        }



        /// <summary>
        /// 分配单元;
        /// </summary>
        class Branch : IDynamicResource<int>, IComparable<Branch>, IDisposable
        {

            public Branch(DynamicResource manager, int priority, int demand)
            {
                this.Manager = manager;
                this.Priority = priority;
                this.Demand = demand;
                this.Practice = 0;

                manager.Demand += demand;
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
                if (newDemand == Demand)
                {
                    return;
                }

                Manager.Demand += newDemand - Demand;
                Demand = newDemand;

                if (Manager.Demand > Manager.Total)
                {
                    Manager.Redistribute();
                }
                else
                {
                    Practice = Demand;
                }
            }

            public int CompareTo(Branch other)
            {
                return other.Priority - Priority;
            }

            public void Dispose()
            {
                Manager.branchList.Remove(this);
                Manager.Demand -= Demand;
                Practice = 0;
                Manager.Redistribute();
            }

            public override string ToString()
            {
                return "[Priority:" + Priority + ",Demand:" + Demand + ",Practice:" + Practice + "]";
            }

        }

    }

}
