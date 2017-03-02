using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Rx;
using UnityEngine;

namespace KouXiaGu.SimplifiedTime
{

    /// <summary>
    /// 在控制台输出时间信息;
    /// </summary>
    public class OutputTimeConsole : Observer<SimplifiedDateTime>
    {

        public OutputTimeConsole()
        {
        }

        public OutputTimeConsole(string designation)
        {
            this.Designation = designation;
        }


        public string Designation { get; private set; }


        public override void OnNext(SimplifiedDateTime item)
        {
            Debug.Log(Designation + item.ToString());
        }

    }

}
