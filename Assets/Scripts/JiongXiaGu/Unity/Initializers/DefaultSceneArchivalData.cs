using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Initializers
{

    public class DefaultSceneArchivalData
    {
        public SceneArchivalData Value { get; private set; }

        public DefaultSceneArchivalData()
        {
            Value = new SceneArchivalData();
        }
    }
}
