using JiongXiaGu.Unity.Archives;
using JiongXiaGu.Unity.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Initializers
{

    public interface IGameResInitializeHandle
    {
        void Initialize(Archive archive, CancellationToken token);
    }

    [DisallowMultipleComponent]
    public sealed class GameResInitializer : InitializerBase
    {
        private GameResInitializer()
        {
        }

        private IGameResInitializeHandle[] integrateHandlers;

        private void Awake()
        {
            integrateHandlers = GetComponentsInChildren<IGameResInitializeHandle>();
        }

        public Task Initialize(Archive archive, IProgress<ProgressInfo> progress, CancellationToken token)
        {
            return base.Initialize(archive, progress, token);
        }

        protected override List<Func<CancellationToken, string>> EnumerateHandler(object state)
        {
            var handlers = new List<Func<CancellationToken, string>>();
            var archive = state as Archive;

            foreach (var integrateHandler in integrateHandlers)
            {
                handlers.Add(delegate (CancellationToken token)
                {
                    token.ThrowIfCancellationRequested();

                    integrateHandler.Initialize(archive, token);
                    return null;
                });
            }

            return handlers;
        }
    }
}
