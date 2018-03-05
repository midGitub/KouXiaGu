using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Schedulers
{

    public struct DataLockToken
    {
        private DataLockTokenSource source;

        /// <summary>
        /// 是否只读?
        /// </summary>
        public bool IsReadOnly => source != null && source.IsReadOnly;

        public DataLockToken(DataLockTokenSource source)
        {
            this.source = source;
        }

        /// <summary>
        /// 若为只读状态则返回异常;
        /// </summary>
        public void ThrowIfIsReadOnly()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException("Read Only!");
            }
        }
    }
}
