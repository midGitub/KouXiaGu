using JiongXiaGu.Unity.Archives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiongXiaGu.Unity.RectMaps;

namespace JiongXiaGu.Unity.Initializers
{

    public class DefaultSceneArchivalData
    {
        public SceneArchivalData Value { get; private set; }

        public DefaultSceneArchivalData()
        {
            Value = new SceneArchivalData();
            AddMapData();
        }

        void AddMapData()
        {
            MapSceneArchivalData archivalData = new MapSceneArchivalData("Wolrd_111");
            Value.Add(archivalData);
        }
    }
}
