//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ProtoBuf;

//namespace KouXiaGu.WorldEvents
//{

//    /// <summary>
//    /// 国家资源;
//    /// </summary>
//    public class CountryResources
//    {

//        public CountryResources()
//        {
//            resources = new Dictionary<int, Resource>();
//        }


//        /// <summary>
//        /// 资源信息;
//        /// </summary>
//        Dictionary<int, Resource> resources;


//        /// <summary>
//        /// 添加资源供应源;
//        /// </summary>
//        public void AddSupplier(int resourceType)
//        {
//            throw new NotImplementedException();
//        }

//        /// <summary>
//        /// 改变该资源的存储量;
//        /// </summary>
//        internal void ChangeStock(int resourceType, int increment)
//        {

//        }

//        /// <summary>
//        /// 改变该资源的最大存储容量;
//        /// </summary>
//        internal void ChangeCapacity(int resourceType, int increment)
//        {

//        }

//        /// <summary>
//        /// 资源;
//        /// </summary>
//        class Resource : IReadOnlyResource
//        {

//            public Resource(int resourceType)
//            {
//                this.ResourceType = resourceType;
//            }

//            /// <summary>
//            /// 资源类型;
//            /// </summary>
//            public int ResourceType { get; private set; }

//            /// <summary>
//            /// 存量;
//            /// </summary>
//            public int Stock { get; set; }

//            /// <summary>
//            /// 最大容量;
//            /// </summary>
//            public int Capacity { get; set; }

//        }

//    }

//}
