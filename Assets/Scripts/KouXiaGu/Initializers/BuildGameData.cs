using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    public struct BuildGameData
    {
        public BuildGameData(ICoreDataResource coreDataResource, ModGroup modRes)
        {
            this.coreDataResource = coreDataResource;
            this.modRes = modRes;
        }

        public ModGroup modRes { get; private set; }

        public ICoreDataResource coreDataResource { get; private set; }

    }

}
