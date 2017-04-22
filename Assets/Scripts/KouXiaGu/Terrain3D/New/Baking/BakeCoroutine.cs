using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    /// <summary>
    /// 实现具体烘培方法;
    /// </summary>
    class BakeCoroutine : IEnumerator
    {
        public object Current
        {
            get { return null; }
        }



        public bool MoveNext()
        {
            return true;
        }

        public void Reset()
        {
            return;
        }
    }

}
