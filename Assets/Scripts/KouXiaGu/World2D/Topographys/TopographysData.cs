using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 定义地貌预制ID和其信息转换;
    /// 地貌预制是预先定义的,但是具体信息是从XML文件读取到的;
    /// </summary>
    [DisallowMultipleComponent]
    public class TopographysData : MonoBehaviour
    {
        TopographysData() { }

        /// <summary>
        /// 定义地貌信息的XML文件;
        /// </summary>
        [SerializeField]
        string TopographyInfosXMLFilePath;




    }

}
