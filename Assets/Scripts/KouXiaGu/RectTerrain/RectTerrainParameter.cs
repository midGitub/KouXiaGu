using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KouXiaGu.RectTerrain
{

    [Serializable]
    public class RectTerrainParameter
    {

        [SerializeField]
        LandformChunk chunkPrefab;

        public LandformChunk ChunkPrefab
        {
            get { return chunkPrefab; }
        }
    }
}
