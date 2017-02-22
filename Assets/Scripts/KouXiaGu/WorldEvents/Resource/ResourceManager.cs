using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;

namespace KouXiaGu.WorldEvents
{


    public class ResourceManager
    {

        /// <summary>
        /// 所有资源类别;
        /// </summary>
        public IReadOnlyDictionary<int, Resource> Types { get; private set; }


    }

}
