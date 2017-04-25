using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu
{

    public class Unsubscriber : IDisposable
    {
        public Unsubscriber(Action disposer)
        {
            if (disposer == null)
                throw new ArgumentNullException();

            isUnsubscribe = false;
            this.disposer = disposer;
        }

        bool isUnsubscribe;
        Action disposer;

        public void Dispose()
        {
            if (!isUnsubscribe)
            {
                isUnsubscribe = true;
                disposer();
            }
        }
    }


}
