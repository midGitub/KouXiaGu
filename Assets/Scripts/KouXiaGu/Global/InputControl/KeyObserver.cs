using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.InputControl
{

    public class KeyDownObserver : UnityThreadBehaviour
    {
        public KeyDownObserver(object sender, KeyFunction key, Action onKeyDown)
            : base(sender)
        {
            Key = key;
            this.onKeyDown = onKeyDown;
        }

        Action onKeyDown;
        public KeyFunction Key { get; private set; }

        protected override void OnNext()
        {
            if (KeyInput.GetKeyDown(Key))
            {
                onKeyDown();
            }
        }
    }
}
