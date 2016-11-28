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


        public static IEnumerator Start1<T>(T item)
        {
            UnityEngine.Object[] array = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            IConstruct1<T>[] constructs = array.OfType<IConstruct1<T>>().ToArray();
            return Start1(constructs, item);
        }

        public static IEnumerator Start1<T>(IEnumerable<IConstruct1<T>> constructs, T item)
        {
            HashSet<IConstruct1<T>> waitSet = new HashSet<IConstruct1<T>>();
            ConstructerGroup<T>[] constructerGroups = ToConstructerGroup(constructs, waitSet).ToArray();

            foreach (var constructerGroup in constructerGroups)
            {
                constructerGroup.Construction(item);
            }

            while (waitSet.Count != 0)
            {
                yield return null;
            }
        }

        static IEnumerable<ConstructerGroup<T>> ToConstructerGroup<T>(IEnumerable<IConstruct1<T>> constructs, HashSet<IConstruct1<T>> waitSet)
        {
            foreach (var construct in constructs)
            {
                yield return new ConstructerGroup<T>(waitSet, construct);
            }
        }


        public class ConstructerGroup<T>
        {
            public ConstructerGroup(HashSet<IConstruct1<T>> waitSet, IConstruct1<T> construct)
            {
                this.waitSet = waitSet;
                this.construct = construct;
            }

            HashSet<IConstruct1<T>> waitSet;
            IConstruct1<T> construct;

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



        public static IEnumerator Start2<T>(T item)
        {
            UnityEngine.Object[] array = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
            IConstruct2<T>[] constructs = array.OfType<IConstruct2<T>>().ToArray();
            return Start2(constructs, item);
        }

        public static IEnumerator Start2<T>(IEnumerable<IConstruct2<T>> constructs, T item)
        {
            HashSet<IConstruct2<T>> waitSet = new HashSet<IConstruct2<T>>();
            Constructer2Group<T>[] constructerGroups = ToConstructer2Group(constructs, waitSet).ToArray();

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

        static IEnumerable<Constructer2Group<T>> ToConstructer2Group<T>(IEnumerable<IConstruct2<T>> constructs, HashSet<IConstruct2<T>> waitSet)
        {
            foreach (var construct in constructs)
            {
                yield return new Constructer2Group<T>(waitSet, construct);
            }
        }

        public class Constructer2Group<T>
        {
            public Constructer2Group(HashSet<IConstruct2<T>> waitSet, IConstruct2<T> construct)
            {
                this.waitSet = waitSet;
                this.construct = construct;
            }

            HashSet<IConstruct2<T>> waitSet;
            IConstruct2<T> construct;

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
