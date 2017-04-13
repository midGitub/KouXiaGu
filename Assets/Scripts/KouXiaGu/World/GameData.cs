using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World
{

    public class GameData
    {


        /// <summary>
        /// 基础信息;
        /// </summary>
        public WorldElementManager ElementInfo { get; private set; }

        void Initialize()
        {
            ElementInfo = WorldElementManager.Read();
        }

    }

}
