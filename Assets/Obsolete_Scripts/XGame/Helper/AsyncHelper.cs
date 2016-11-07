using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace XGame
{

    /// <summary>
    /// 异步操作;
    /// </summary>
    public static class AsyncHelper
    {

        /// <summary>
        /// Unity协程;
        /// </summary>
        public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();


        /// <summary>
        /// 异步调用;
        /// </summary>
        /// <param name="coroutines"></param>
        /// <param name="schedule"></param>
        /// <param name="callBreak">当所有调用完毕时调用;</param>
        /// <returns></returns>
        public static IEnumerator LoadAsync(
            IEnumerable<IEnumerator> coroutines,
            ISchedule schedule,
            Action callBreak = null)
        {
            foreach (var coroutine in coroutines)
            {
                while (coroutine.MoveNext())
                {
                    yield return coroutine.Current;
                }
                if (schedule != null)
                    schedule.Complete++;
            }
            if (callBreak != null)
                callBreak();
        }


        /// <summary>
        /// 对coroutines进行迭代;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contents">要传入每个coroutines内的内容;</param>
        /// <param name="coroutines">获取到协程类;</param>
        /// <param name="schedule">进度;</param>
        /// <param name="callBreak">在整个流程结束后调用;</param>
        /// <returns></returns>
        public static IEnumerator LoadAsync<T>(
            T content,
            IEnumerable<Func<T, IEnumerator>> coroutines,
            ISchedule schedule,
            Action callBreak = null)
        {
            foreach (var coroutine in coroutines)
            {
                IEnumerator enumerator = coroutine(content);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
                if (schedule != null)
                    schedule.Complete++;
            }
            if(callBreak != null)
                callBreak();
        }

        /// <summary>
        /// 按contents的顺序对coroutines进行迭代;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contents">要传入每个coroutines内的内容;</param>
        /// <param name="coroutines">获取到协程类;</param>
        /// <param name="schedule">进度;</param>
        /// <param name="callBreak">在整个流程结束后调用;</param>
        /// <returns></returns>
        public static IEnumerator LoadAsync<T>(
            IEnumerable<T> contents,
            IEnumerable<Func<T, IEnumerator>> coroutines,
            ISchedule schedule,
            Action callBreak = null)
        {
            foreach (var content in contents)
            {
                IEnumerator enumerator = LoadAsync(content, coroutines, schedule);
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Current;
                }
            }
            if (callBreak != null)
                callBreak();
        }

        /// <summary>
        /// 异步调用;
        /// </summary>
        /// <param name="coroutine">协程;</param>
        /// <param name="schedule">进度接口;</param>
        /// <param name="callBreak">全部完成后调用;</param>
        /// <returns></returns>
        public static IEnumerator LoadAsync(
            IEnumerator coroutine,
            ISchedule schedule,
            Action callBreak = null)
        {
            while (coroutine.MoveNext())
            {
                yield return coroutine.Current;
            }

            if (schedule != null)
                schedule.Complete++;

            if (callBreak != null)
                callBreak();
        }


        /// <summary>
        /// 异步读取Module资源;
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="contents">不同的内容,每个Module都需要读取的内容;</param>
        /// <param name="modules">多个不同的组件</param>
        /// <param name="coroutine">获取到协程的方法;</param>
        /// <param name="schedule">进度接口;</param>
        /// <param name="callBreak">全部完成后调用;</param>
        /// <returns></returns>
        public static IEnumerator LoadAsync<T1, T2>(
            IEnumerable<T1> contents,
            IEnumerable<T2> modules,
            Func<T1, T2, IEnumerator> coroutine,
            ISchedule schedule,
            Action callBreak = null)
        {
            IEnumerator enumeratorLoad;

            foreach (var module in modules)
            {
                foreach (var content in contents)
                {
                    enumeratorLoad = coroutine(content, module);
                    enumeratorLoad = LoadAsync(enumeratorLoad, schedule, null);

                    while (enumeratorLoad.MoveNext())
                        yield return enumeratorLoad.Current;
                }
            }
        }

        /// <summary>
        /// 异步按顺序读取Module资源;
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="contents">不同的内容,每个Module都需要读取的内容;</param>
        /// <param name="modules">多个不同的组件</param>
        /// <param name="coroutine">获取到协程的方法;</param>
        /// <param name="schedule">进度接口;</param>
        /// <param name="callBreak">全部完成后调用;</param>
        /// <returns></returns>
        public static IEnumerator OrderLoadAsync<T1, T2>(
            IEnumerable<T1> contents,
            IEnumerable<T2> modules,
            Func<T1, T2, IEnumerator> coroutine,
            ISchedule schedule,
            Action callBreak = null)
            where T2 : ICallOrder
        {
            IEnumerable<T2> filterModules;
            IEnumerator enumeratorLoad;
            var enumArray = Enum.GetValues(typeof(CallOrder));

            foreach (CallOrder enumValue in enumArray)
            {
                filterModules = modules.Where(controller => controller.CallOrder == enumValue);
                enumeratorLoad = LoadAsync(contents, filterModules, coroutine, schedule, null);

                while (enumeratorLoad.MoveNext())
                    yield return enumeratorLoad.Current;
            }

            if (callBreak != null)
                callBreak();
        }


        public static IEnumerator OrderLoadAsync<T1>(
            IEnumerable<T1> modules,
            Func<T1, IEnumerator> func,
            ISchedule schedule,
            Action callBreak = null)
            where T1 : ICallOrder
        {
            IEnumerable<T1> filterModules;
            IEnumerator enumeratorLoad;
            var enumArray = Enum.GetValues(typeof(CallOrder));

            foreach (CallOrder enumValue in enumArray)
            {
                filterModules = modules.Where(controller => controller.CallOrder == enumValue);
                foreach (var module in filterModules)
                {
                    enumeratorLoad = func(module);
                    while (enumeratorLoad.MoveNext())
                    {
                        yield return enumeratorLoad.Current;
                    }
                    if (schedule != null)
                        schedule.Complete++;
                }
            }

            if (callBreak != null)
                callBreak();
        }




    }

}
