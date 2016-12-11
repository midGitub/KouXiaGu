using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 按地图六边形中心点的位置跟踪目标;
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class HexFollowing2D : MonoBehaviour
    {
        private HexFollowing2D() { }

        [SerializeField]
        Transform target;

        [SerializeField]
        ShortVector2 offset;

        private void Update()
        {
            Vector2 point = WorldConvert.PlaneToHex(target.position);
            point.x = offset.x * WorldConvert.MapHexagon.DistanceX * 2;
            point.y = offset.y * WorldConvert.MapHexagon.DistanceY * 2;
            transform.position = point;
        }

    }

}
