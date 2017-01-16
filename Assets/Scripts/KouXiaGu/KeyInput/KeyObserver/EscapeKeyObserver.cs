using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.KeyInput
{

    public class EscapeKeyObserver : KeyObserver, IKeyResponse
    {
        public EscapeKeyObserver(Action onKeyDown) : base(onKeyDown) { }

        public override void Subscribe()
        {
            Unsubscriber = SpecialKey.Escape.Subscribe(this);
        }

    }

}
