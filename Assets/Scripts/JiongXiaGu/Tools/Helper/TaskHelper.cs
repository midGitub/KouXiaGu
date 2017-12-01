using System;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu
{

    public static class TaskHelper
    {
        /// <summary>
        /// 启动 Task,并将它安排到指定的 TaskScheduler 中执行;
        /// </summary>
        public static Task Run(Action action, TaskScheduler scheduler)
        {
            Task task = new Task(action);
            task.Start(scheduler);
            return task;
        }

        /// <summary>
        /// 启动 Task,并将它安排到指定的 TaskScheduler 中执行;
        /// </summary>
        public static Task Run(Action action, CancellationToken cancellation, TaskScheduler scheduler)
        {
            Task task = new Task(action, cancellation);
            task.Start(scheduler);
            return task;
        }

        /// <summary>
        /// 启动 Task,并将它安排到指定的 TaskScheduler 中执行;
        /// </summary>
        public static Task<TResult> Run<TResult>(Func<TResult> function, TaskScheduler scheduler)
        {
            Task<TResult> task = new Task<TResult>(function);
            task.Start(scheduler);
            return task;
        }

        /// <summary>
        /// 启动 Task,并将它安排到指定的 TaskScheduler 中执行;
        /// </summary>
        public static Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellation, TaskScheduler scheduler)
        {
            Task<TResult> task = new Task<TResult>(function, cancellation);
            task.Start(scheduler);
            return task;
        }
    }
}
