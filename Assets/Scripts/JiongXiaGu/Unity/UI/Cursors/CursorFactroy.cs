using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiongXiaGu.Unity.UI.Cursors
{


    public class CursorFactroy
    {

        /// <summary>
        /// 从目录读取到自定义光标;
        /// </summary>
        public ICustomCursor Read(string directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录读取到动画光标;
        /// </summary>
        public AnimatedCursor ReadAnimatedCursor(string directory)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从目录读取到静态光标;
        /// </summary>
        public StaticCursor ReadStaticCursor(string directory)
        {
            throw new NotImplementedException();
        }
    }
}
