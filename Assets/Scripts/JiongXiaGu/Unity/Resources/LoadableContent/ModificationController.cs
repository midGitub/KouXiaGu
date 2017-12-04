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
        /// 
        /// </summary>
        Task Add(LoadableContent content);


        void Remove(LoadableContent content);
    }

    /// <summary>
    /// 模组状态和信息;
    /// </summary>
    public class ModificationController : MonoBehaviour
    {


    }
}
