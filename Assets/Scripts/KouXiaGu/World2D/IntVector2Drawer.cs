
namespace KouXiaGu.World2D
{
    using Grids;

#if UNITY_EDITOR

    using UnityEditor;
    using UnityEngine;


    [CustomPropertyDrawer(typeof(RectCoord))]
    public class IntVector2Drawer : ShortVector2Drawer
    {


    }

#endif

}
