using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Archives
{

    /// <summary>
    /// 对状态进行存档的处置接口;
    /// </summary>
    public interface IDataArchival
    {
        /// <summary>
        /// 输出存档内容;
        /// </summary>
        /// <param name="archive">存档路径;</param>
        /// <param name="cancellationToken">取消存档标记;</param>
        Task Write(IArchiveFileInfo archive, CancellationToken cancellationToken);
    }
}
