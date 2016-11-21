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
            UnityEngine.Object[] array = GameObject.FindObjectsOfType(typeof(IConstruct<T>));
            IEnumerable<IConstruct<T>> constructs = array.Cast<IConstruct<T>>();
            return Start(constructs, item);
        }

        public static IEnumerator Start<T>(IEnumerable<IConstruct<T>> constructs, T item)
        {
            HashSet<IConstruct<T>> wait = new HashSet<IConstruct<T>>();

            foreach (var construct in constructs)
            {
                Action onComplete = () => wait.Remove(construct);
                Observable.FromMicroCoroutine(_ => construct.Construction(item)).Subscribe(null, onComplete);
            }

            while (wait.Count != 0)
            {
                yield return null;
            }

        }

    }

}
