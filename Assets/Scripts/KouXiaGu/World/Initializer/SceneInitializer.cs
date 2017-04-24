using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu.World
{

    /// <summary>
    /// 场景初始化,如根据角色位置进行 地形烘培 人物创建 等;
    /// </summary>
    public class SceneInitializer : AsyncInitializer<IWorldScene>, IWorldScene
    {
        public override string Prefix
        {
            get { return "游戏世界场景"; }
        }

        public IAsyncOperation<IWorldScene> Start(IWorldData data, IWorldComponent component, IObservable<IWorld> starter)
        {
            StartInitialize();
            OnCompleted(this);
            return this;
        }

    }

}
