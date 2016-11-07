using KouXiaGu.Map;
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
            str += GetWorldPoint(mousePosition);
            str += GetMapPoint(mousePosition);

            return str;
        }

        private string GetScreenPoint(Vector3 mousePosition)
        {
            string str = "";

            str += "  视窗坐标 :" + mousePosition;

            return str;
        }

        private string GetWorldPoint(Vector3 mousePosition)
        {
            string str = "";
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                str += "  世界坐标 :" + raycastHit.point.ToString();
            }
            else
            {
                str += "  世界坐标 :" + "无法确定!";
            }
            return str;
        }

        private string GetMapPoint(Vector3 mousePosition)
        {
            string str = "";

            Hexagon hexagon = new Hexagon();
            hexagon.OuterDiameter = 2f;
            Vector2 worldPoint = GetWorldPoint(Camera.main, mousePosition);
            var item = hexagon.GetClosePoint(worldPoint);
            str += "地图坐标 :" + item;

            return str;
        }

        private static Vector2 GetWorldPoint(Camera camera, Vector3 screenPoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                return raycastHit.point;
            }
            else
            {
                Debug.Log("坐标无法确定!");
                return raycastHit.point;
            }
        }

        //[ContextMenu("六边形测试;")]
        //private void S_Test()
        //{
        //    Hexagon hexagon = new Hexagon();
        //    hexagon.OuterDiameter = 2;

        //    Debug.Log(hexagon);
        //}

    }

}
