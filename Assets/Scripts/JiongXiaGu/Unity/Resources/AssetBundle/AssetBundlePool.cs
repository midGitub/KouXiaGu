using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    /// <summary>
    /// AssetBundle 实例池;
    /// </summary>
    public static class AssetBundlePool
    {


    }


    public sealed class AssetBundleAppoint : IDisposable
    {
        private bool isDisposed = false;

        ~AssetBundleAppoint()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }
                isDisposed = true;
            }
        }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
