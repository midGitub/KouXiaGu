using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    public interface IGameSceneBuilder
    {
        Task Build();
    }

    public class GameSceneBuilder : InitializerBase
    {
        private GameSceneBuilder()
        {
        }

        protected override List<Func<CancellationToken, string>> EnumerateHandler(object state)
        {
            throw new NotImplementedException();
        }
    }
}
