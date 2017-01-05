using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.KeyInput
{


    public class EnterKeyObserver : KeyObserver, IKeyResponse
    {

        public EnterKeyObserver(Action onKeyDown) : base(onKeyDown) { }

        public override void Subscribe()
        {
            Unsubscriber = SpecialKey.Enter.Subscribe(this);
        }

    }

}
