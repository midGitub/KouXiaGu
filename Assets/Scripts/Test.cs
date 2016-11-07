using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Assets.Scripts
{


    public class Test : MonoBehaviour
    {

        public ReactiveProperty<int> i = new ReactiveProperty<int>(12);

        private void Start()
        {
            i.Subscribe(c => Debug.Log(c));
        }

        [ContextMenu("AAA")]
        private void TTTT()
        {
            i.Value = 1;
        }

    }

}
