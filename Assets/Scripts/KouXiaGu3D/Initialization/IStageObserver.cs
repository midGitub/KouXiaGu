using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu.Initialization
{


    public interface IStageObserver<T>
    {

        /// <summary>
        /// 开始进入这个阶段;
        /// </summary>
        IEnumerator OnEnter(T item);

        /// <summary>
        /// 离开这个阶段;
        /// </summary>
        IEnumerator OnLeave(T item);

        /// <summary>
        /// 当进入这个阶段失败时回滚操作;
        /// </summary>
        IEnumerator OnEnterRollBack(T item);

        /// <summary>
        /// 当离开这个阶段失败时回滚操作;
        /// </summary>
        IEnumerator OnLeaveRollBack(T item);

        /// <summary>
        /// 成功进入到这个阶段调用;
        /// </summary>
        void OnEnterCompleted();

        /// <summary>
        /// 成功离开这个阶段调用;
        /// </summary>
        void OnLeaveCompleted();
    }

}
