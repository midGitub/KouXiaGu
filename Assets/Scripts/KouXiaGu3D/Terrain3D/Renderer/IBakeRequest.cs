using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    public interface IBakeRequest<T>
    {
        Vector3 CameraPosition { get; }
        IEnumerable<T> BakingNodes { get; }
        void TextureComplete(Texture2D diffuse, Texture2D height);
    }

}
