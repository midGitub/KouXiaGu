//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace KouXiaGu.WorldEvents
//{

//    /// <summary>
//    /// 只读资源信息;
//    /// </summary>
//    public interface IReadOnlyResource
//    {

//        /// <summary>
//        /// 资源类型;
//        /// </summary>
//        int ResourceType { get; }

//        /// <summary>
//        /// 存量;
//        /// </summary>
//        int Stock { get; }

//        /// <summary>
//        /// 最大容量;
//        /// </summary>
//        int Capacity { get; }
//    }

//    /// <summary>
//    /// 可购买的,可采购的;Supplier 供应商;
//    /// </summary>
//    public interface IPurchasable : IReadOnlyResource
//    {

//    }

//    /// <summary>
//    /// 统计表单;
//    /// </summary>
//    public class StatisticalForm : IReadOnlyResource
//    {

//        public StatisticalForm(int resourceType)
//        {
//            this.ResourceType = resourceType;
//        }

//        /// <summary>
//        /// 资源类型;
//        /// </summary>
//        public int ResourceType { get; private set; }

//        /// <summary>
//        /// 存量;
//        /// </summary>
//        public int Stock { get; set; }

//        /// <summary>
//        /// 最大容量;
//        /// </summary>
//        public int Capacity { get; set; }
//    }

//    /// <summary>
//    /// 资源统计;
//    /// </summary>
//    public interface IStatistician
//    {

//        /// <summary>
//        /// 资源类型;
//        /// </summary>
//        int ResourceType { get; }

//        /// <summary>
//        /// 添加统计对象;
//        /// </summary>
//        StatisticalForm AddSupplier(IPurchasable supplier);
//    }

//}
