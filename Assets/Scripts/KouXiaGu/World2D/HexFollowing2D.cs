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
        private Transform target;

        private void Update()
        {
            transform.position = WorldConvert.PlaneToHex(target.position);
        }

    }

}
