using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;

namespace KouXiaGu
{

    /// <summary>
    /// 游戏状态构建控制;
    /// </summary>
    public class Constructer
    {

        public static IEnumerator Start<T>(T item)
        {
            UnityEngine.Object[] array = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            IConstruct<T>[] constructs = array.OfType<IConstruct<T>>().ToArray();
            return Start(constructs, item);
        }

        public static IEnumerator Start<T>(IEnumerable<IConstruct<T>> constructs, T item)
        {
            HashSet<IConstruct<T>> waitSet = new HashSet<IConstruct<T>>();
            ConstructerGroup<T>[] constructerGroups = ToConstructerGroup(constructs, waitSet).ToArray();

            foreach (var constructerGroup in constructerGroups)
            {
                constructerGroup.Prepare(item);
            }

            while (waitSet.Count != 0)
            {
                yield return null;
            }

            foreach (var constructerGroup in constructerGroups)
            {
                constructerGroup.Construction(item);
            }

            while (waitSet.Count != 0)
            {
                yield return null;
            }

        }

        static IEnumerable<ConstructerGroup<T>> ToConstructerGroup<T>(IEnumerable<IConstruct<T>> constructs, HashSet<IConstruct<T>> waitSet)
        {
            foreach (var construct in constructs)
            {
                yield return new ConstructerGroup<T>(waitSet, construct);
            }
        }

        public class ConstructerGroup<T>
        {
            public ConstructerGroup(HashSet<IConstruct<T>> waitSet, IConstruct<T> construct)
            {
                this.waitSet = waitSet;
                this.construct = construct;
            }

            HashSet<IConstruct<T>> waitSet;
            IConstruct<T> construct;

            public void Prepare(T item)
            {
                waitSet.Add(construct);
                Observable.FromMicroCoroutine(_ => construct.Prepare(item)).Subscribe(null, OnComplete);
            }

            public void Construction(T item)
            {
                waitSet.Add(construct);
                Observable.FromMicroCoroutine(_ => construct.Construction(item)).Subscribe(null, OnComplete);
            }

            void OnComplete()
            {
               waitSet.Remove(construct);
            }

        }

    }

}
