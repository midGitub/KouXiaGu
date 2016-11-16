using KouXiaGu.Map;
using KouXiaGu.World2D;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.Test
{

    /// <summary>
    /// 游戏视窗位置测试;
    /// </summary>
    [SerializeField]
    public class PositionTest : MonoBehaviour
    {

        private Text textObject;

        private void Awake()
        {
            textObject = GetComponent<Text>();
        }

        private void Start()
        {
            this.ObserveEveryValueChanged(_ => UnityEngine.Input.mousePosition).
                SubscribeToText(textObject, TextUpdate);
        }

        private string TextUpdate(Vector3 mousePosition)
        {
            string str = "";

            str += GetScreenPoint(mousePosition);
            str += GetPlanePoint(mousePosition);

            return str;
        }

        private string GetScreenPoint(Vector3 mousePosition)
        {
            string str = "";

            str += "视窗坐标 :" + mousePosition;

            return str;
        }

        private string GetPlanePoint(Vector3 mousePosition)
        {
            string str = "";

            str += "平面坐标 :" + WorldConvert.MouseToHexPair().HexPoint +
                "地图坐标 :" + WorldConvert.MouseToHexPair().MapPoint + 
                "平面坐标 :" + WorldConvert.MouseToPlane();
            return str;
        }

    }

}
