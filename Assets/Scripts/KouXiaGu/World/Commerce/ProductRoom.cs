using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Commerce
{

    /// <summary>
    /// 存放\记录 产品的数目;
    /// </summary>
    public class ProductRoom
    {

        public ProductRoom(Product productType, ProductHouse warehouse)
        {
            this.ProductType = productType;
            this.House = warehouse;
            this.Total = 0;
        }


        /// <summary>
        /// 归属;
        /// </summary>
        public ProductHouse House { get; private set; }

        /// <summary>
        /// 产品类型;
        /// </summary>
        public Product ProductType { get; private set; }

        /// <summary>
        /// 资源总数;
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// 影响因素;
        /// </summary>
        public NumericalFactor InfluenceFactor { get; private set; }

        /// <summary>
        /// 房间是否为空?
        /// </summary>
        public bool IsEmpty
        {
            get { return Total == 0; }
        }

        /// <summary>
        /// 是否允许移除;
        /// </summary>
        public bool IsRemovable()
        {
            return InfluenceFactor.IsRemovable();
        }


        /// <summary>
        /// 增加产品数目; number 大于或等于 0;
        /// </summary>
        public void AddProduct(int number)
        {
            Total += number;
        }

        /// <summary>
        /// 移除产品数目,并返回未能移除的数目; number 大于或等于 0;
        /// </summary>
        public int RemoveProduct(int number)
        {
            if (number > Total)
            {
                int result = Total - number;
                Total = 0;
                return Math.Abs(result);
            }
            else
            {
                Total -= number;
                return 0;
            }
        }

        /// <summary>
        /// 尝试移除这类产品数目,若无法移除则返回false,移除成功返回true;
        /// </summary>
        public bool TryRemoveProduct(int number)
        {
            if (Total < number)
                return false;

            Total -= number;
            return true;
        }


        public void DayUpdate()
        {
            Total = (int)(Total * InfluenceFactor.Percentage);
            Total += InfluenceFactor.Increment;
        }

    }

}
