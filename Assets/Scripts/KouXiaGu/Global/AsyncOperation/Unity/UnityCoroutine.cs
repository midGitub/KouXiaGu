using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    /// <summary>
    /// Unity协程;
    /// </summary>
    public class UnityCoroutine : UnityThreadBehaviour
    {
        public UnityCoroutine(object sender, IEnumerator coroutine) : base(sender)
        {
            this.coroutine = new Coroutine(coroutine);
        }

        readonly Coroutine coroutine;

        protected override void OnNext()
        {
            coroutine.MoveNext();
        }
    }

}
