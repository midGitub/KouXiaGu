using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.KeyInput
{


    public abstract class KeyObserver : IKeyResponse
    {

        public KeyObserver(Action onKeyDown)
        {
            this.onKeyDown = onKeyDown;
        }

        Action onKeyDown;
        protected IDisposable Unsubscriber { get; set; }

        public abstract void Subscribe();

        public bool Unsubscribe()
        {
            if (Unsubscriber != null)
            {
                Unsubscriber.Dispose();
                Unsubscriber = null;
                return true;
            }
            return false;
        }

        void IKeyResponse.OnKeyDown()
        {
            onKeyDown();
        }

    }

}
