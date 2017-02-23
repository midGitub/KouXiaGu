//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.WorldEvents
//{


//    /// <summary>
//    /// 城镇资源;
//    /// </summary>
//    public class TownResources
//    {

//        public TownResources()
//        {
//            resources = new List<Resource>();
//        }

//        public TownResources(CountryResources belongCountry) : this()
//        {
//            SetBelongCountry(belongCountry);
//        }


//        List<Resource> resources;

//        /// <summary>
//        /// 城镇所属国家;
//        /// </summary>
//        public CountryResources BelongCountry { get; private set; }


//        /// <summary>
//        /// 设置所属国家;
//        /// </summary>
//        public void SetBelongCountry(CountryResources belongCountry)
//        {
//            this.BelongCountry = belongCountry;
//            AttachBelongCountry();
//        }

//        /// <summary>
//        /// 将现有资源附加到国家资源内;
//        /// </summary>
//        void AttachBelongCountry()
//        {

//        }


//        /// <summary>
//        /// 资源;
//        /// </summary>
//        internal class Resource : IReadOnlyResource
//        {

//            const int minStock = 0;
//            const int minCapacity = 0;


//            public Resource()
//            {
//                this.Stock = minStock;
//                this.Capacity = minCapacity;
//            }

//            public Resource(int resourceType, TownResources town) : this()
//            {
//                this.ResourceType = resourceType;
//                this.Town = town;
//            }


//            /// <summary>
//            /// 资源类型;
//            /// </summary>
//            public int ResourceType { get; private set; }

//            /// <summary>
//            /// 城镇信息;
//            /// </summary>
//            public TownResources Town { get; set; }

//            /// <summary>
//            /// 存量;
//            /// </summary>
//            public int Stock { get; private set; }

//            /// <summary>
//            /// 最大容量;
//            /// </summary>
//            public int Capacity { get; private set; }

//            /// <summary>
//            /// 是否为空?
//            /// </summary>
//            public bool IsEmpty
//            {
//                get { return Stock <= 0 && Capacity <= 0; }
//            }

//            /// <summary>
//            /// 所属国家;
//            /// </summary>
//            public CountryResources Country
//            {
//                get { return Town.BelongCountry; }
//            }


//            /// <summary>
//            /// 改变存量,若扣除量超出现有存储量,则返回异常;
//            /// </summary>
//            public void ChangeStock(int increment)
//            {
//                var result = Stock + increment;

//                if (result < 0)
//                    throw new ArgumentOutOfRangeException();

//                this.Stock = result;
//                Country.ChangeStock(ResourceType, increment);
//            }

//            /// <summary>
//            /// 改变容量,若结果小余0则返回异常;
//            /// </summary>
//            public void ChangeCapacity(int increment)
//            {
//                var result = this.Capacity + increment;

//                if (result < 0)
//                    throw new ArgumentOutOfRangeException();

//                this.Capacity = result;
//                Country.ChangeCapacity(ResourceType, increment);
//            }

//        }

//    }

//}
