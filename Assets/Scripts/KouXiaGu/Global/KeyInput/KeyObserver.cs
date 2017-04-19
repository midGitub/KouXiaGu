using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Rx;

namespace KouXiaGu.KeyInput
{

    public class KeyDownObserver : UnityThreadEvent
    {
        public KeyDownObserver(KeyFunction key, Action onKeyDown)
        {
            Key = key;
            this.onKeyDown = onKeyDown;
        }

        Action onKeyDown;
        public KeyFunction Key { get; private set; }

        public override void OnNext()
        {
            if (CustomInput.GetKeyDown(Key))
            {
                onKeyDown();
            }
        }
    }



}
