using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{


    public interface IContentLoadHandler
    {
        /// <summary>
        /// 读取到对应资源,同步方法;
        /// </summary>
        void Add(LoadableContent content);

        /// <summary>
        /// 清除所有已经读取的资源;
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// 模组状态和信息;
    /// </summary>
    public class ModificationController : MonoBehaviour
    {


    }
}
