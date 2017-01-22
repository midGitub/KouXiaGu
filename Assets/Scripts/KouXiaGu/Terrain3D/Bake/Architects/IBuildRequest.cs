using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D
{

    public interface IBuildRequest : IRequest
    {

        /// <summary>
        /// 出现错误时调用;
        /// </summary>
        void OnError(Exception ex);

    }

}
