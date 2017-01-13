using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 初始化基类;
    /// </summary>
    public abstract class Initializer : MonoBehaviour, IStartOperate
    {
        protected Initializer() { }

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Ex { get; private set; }

        /// <summary>
        /// 完成初始化,且没有发生异常;
        /// </summary>
        protected void OnComplete()
        {
            IsCompleted = true;
            IsFaulted = false;
            Ex = null;
        }

        /// <summary>
        /// 初始化中发生异常;
        /// </summary>
        protected void OnFail(Exception ex)
        {
            IsCompleted = true;
            IsFaulted = true;
            this.Ex = ex;
        }

        public abstract void Initialize();

    }

}
