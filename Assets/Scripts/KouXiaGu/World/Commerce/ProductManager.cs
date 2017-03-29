using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using KouXiaGu.Collections;

namespace KouXiaGu.World.Commerce
{


    /// <summary>
    /// 原生的产品信息;
    /// </summary>
    public class ProductManager
    {

        public ProductManager()
        {
            productTypes = new CustomDictionary<int, Product>();
            categorieTypes = new CustomDictionary<int, ProductCategorie>();
        }


        /// <summary>
        /// 产品合集;
        /// </summary>
        CustomDictionary<int, Product> productTypes;

        /// <summary>
        /// 产品种类合集;
        /// </summary>
        CustomDictionary<int, ProductCategorie> categorieTypes;


        /// <summary>
        /// 产品合集;
        /// </summary>
        public IReadOnlyDictionary<int, Product> ProductDictionary
        {
            get { return productTypes; }
        }

        /// <summary>
        /// 产品种类合集;
        /// </summary>
        public IReadOnlyDictionary<int, ProductCategorie> CategorieDictionary
        {
            get { return categorieTypes; }
        }

        /// <summary>
        /// 获取到这个种类的所以产品;
        /// </summary>
        public IEnumerable<Product> GetProduct(ProductCategorie categorie)
        {
            return productTypes.Values.Where(item => item.Categorie == categorie);
        }

        /// <summary>
        /// 读取到产品信息;
        /// </summary>
        public void Read()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 清除所有产品信息;
        /// </summary>
        internal void Clear()
        {
            productTypes.Clear();
            categorieTypes.Clear();
        }

    }

}
