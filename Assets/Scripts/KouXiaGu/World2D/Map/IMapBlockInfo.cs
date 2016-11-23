﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World2D.Map
{

    public interface IMapBlockInfo
    {
        string AddressPrefix { get; }
        string FullArchiveTempDirectoryPath { get; }
        string FullPrefabMapDirectoryPath { get; }
    }

}