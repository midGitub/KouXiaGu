using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [Serializable]
    public class GameArchiver
    {
        public GameArchiver()
        {

        }

        [SerializeField]
        private GameObject BaseComponents;

        private IEnumerable<ISaveInCoroutine> SaveInCoroutineComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ISaveInCoroutine>(); }
        }
        private IEnumerable<ISaveInThread> SaveInThreadComponents
        {
            get { return BaseComponents.GetComponentsInChildren<ISaveInThread>(); }
        }

        public IEnumerator Save(ISaveRes saveRes)
        {
            throw new NotImplementedException();
        } 

    }

}
