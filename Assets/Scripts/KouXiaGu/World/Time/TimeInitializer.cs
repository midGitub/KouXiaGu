using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Initialization;
using UnityEngine;

namespace KouXiaGu
{

    public class TimeInitializer : MonoBehaviour, IStartOperate, IRecoveryOperate, IArchiveOperate
    {

        public bool IsCompleted { get; private set; }
        public bool IsFaulted { get; private set; }
        public Exception Ex { get; private set; }

        void ResetState()
        {
            IsCompleted = false;
            IsFaulted = false;
            Ex = null;
        }

        void IStartOperate.Initialize()
        {
            ResetState();
            throw new NotImplementedException();
        }

        void IRecoveryOperate.Initialize(ArchiveFile archive)
        {
            ResetState();
            throw new NotImplementedException();
        }

        void IArchiveOperate.SaveState(ArchiveFile archive)
        {
            ResetState();
            throw new NotImplementedException();
        }

    }

}
