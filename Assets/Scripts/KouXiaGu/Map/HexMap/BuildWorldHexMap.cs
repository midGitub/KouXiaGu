using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using System.Collections;

namespace KouXiaGu.Map
{

    [DisallowMultipleComponent]
    public class BuildWorldHexMap : MonoBehaviour
    {

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField]
        private MapBlockIOInfo mapBlockIOInfo;

        [SerializeField]
        private BlocksMapInfo blocksMapInfo;

        private static BuildWorldHexMap instance;
        private DynaBlocksMap<MapBlock<HexMapNode>, HexMapNode> mapCollection;
        private Hexagon mapHexagon;

        public IMap<IntVector2, HexMapNode> MapCollection
        {
            get { return mapCollection; }
        }

        public static BuildWorldHexMap GetInstance
        {
            get { return instance ?? (instance = FindInstance()); }
        }

        /// <summary>
        /// 当前地图所用的六边形尺寸;
        /// </summary>
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }

        private void Awake()
        {
            instance = FindInstance();

            mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
            InitMap();
        }

        private void InitMap()
        {
            BlockArchiverMap<HexMapNode> dynaBlocksArchiver = new BlockArchiverMap<HexMapNode>(
                mapBlockIOInfo, blocksMapInfo);

            AddToInit(dynaBlocksArchiver);

            mapCollection = dynaBlocksArchiver;
        }

        private void AddToInit(BlockArchiverMap<HexMapNode> dynaBlocksArchiver)
        {
            IBuildGameData buildGame = Initializers.BuildGameData;

            buildGame.AppendBuildGame.Add(dynaBlocksArchiver);
            buildGame.AppendArchiveGame.Add(dynaBlocksArchiver);
        }

        public void UpdateMapRes(Vector2 targetPosition)
        {
            IntVector2 mapPoint = mapHexagon.TransfromPoint(targetPosition);
            mapCollection.UpdateMapData(mapPoint);
        }

        /// <summary>
        /// 获取到地图坐标;
        /// </summary>
        public IntVector2 GetMapPosition(Vector2 targetPosition)
        {
            IntVector2 mapPosition = mapHexagon.TransfromPoint(targetPosition);
            return mapPosition;
        }

        /// <summary>
        /// 获取到鼠标在地图上表示的坐标;
        /// </summary>
        /// <returns></returns>
        public IntVector2 GetMouseMapPosition()
        {
            var mousePosition = GetMousePosition();
            IntVector2 mapPosition = GetMapPosition(mousePosition);
            return mapPosition;
        }

        /// <summary>
        /// 获取鼠标所在水平面上的坐标;
        /// </summary>
        public static Vector2 GetMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                return raycastHit.point;
            }
            else
            {
                throw new Exception("坐标无法确定!检查摄像机之前地面是否存在3D碰撞模块!");
            }
        }

        private static BuildWorldHexMap FindInstance()
        {
            return GameObject.FindWithTag("GameController").GetComponent<BuildWorldHexMap>();
        }

    }

}
