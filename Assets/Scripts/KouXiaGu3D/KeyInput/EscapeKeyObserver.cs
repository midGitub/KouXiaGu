using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.KeyInput
{

    public class EscapeKeyObserver : IKeyResponse
    {
        public EscapeKeyObserver(Action onKeyDown)
        {
            this.onKeyDown = onKeyDown;
        }

        Action onKeyDown;
        IDisposable unsubscriber;

        public void Subscribe()
        {
            unsubscriber = SpecialKey.Escape.Subscribe(this);
        }

        public void Unsubscribe()
        {
            unsubscriber.Dispose();
            unsubscriber = null;
        }

        void IKeyResponse.OnKeyDown()
        {
            onKeyDown();
        }
    }

}
