using UnityEngine;

namespace KouXiaGu.World2D
{

    /// <summary>
    /// 根据挂在物体位置跟随目标(非平滑);
    /// </summary>
    [DisallowMultipleComponent, ExecuteInEditMode]
    public class HexFollowing2D : MonoBehaviour
    {

        [SerializeField]
        private Transform target;

        private void Update()
        {
            transform.position = WorldConvert.PlaneToHex(target.position);
        }

    }

}
