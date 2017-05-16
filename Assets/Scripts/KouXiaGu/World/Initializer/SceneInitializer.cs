using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{


    public class SceneInitializer : AsyncInitializer
    {
        public override string Prefix
        {
            get { return "等待世界场景构建完成;"; }
        }
    }

}
