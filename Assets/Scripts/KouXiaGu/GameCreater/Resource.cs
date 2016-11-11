using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏创建接口;
    /// </summary>
    public interface ICreateGameResource
    {

        /// <summary>
        /// 是否是通过已保存的存档创建的;
        /// </summary>
        bool IsFromArchived { get; }

        /// <summary>
        /// 存档文件;
        /// </summary>
        ArchivedExpand Archive { get; }

        /// <summary>
        /// 存档目录;
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 在初始化时,游戏需要加载的资源目录;
        /// </summary>
        IEnumerable<string> ResPath { get; }

    }


    [DisallowMultipleComponent]
    public class Resource : MonoBehaviour
    {


    }

}
