using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using System.Collections;

namespace KouXiaGu.Map
{

    /// <summary>
    /// 游戏地图
    /// </summary>
    [DisallowMultipleComponent]
    public class GameMap : MonoBehaviour
    {

        [SerializeField, Tooltip("是否为编辑地图模式?")]
        private bool isEdit;

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField]
        private MapBlockIOInfo mapBlockIOInfo;

        [SerializeField]
        private BlocksMapInfo blocksMapInfo;

        private static GameMap instance;
        private IDynaMap<IntVector2, MapNode> mapCollection;
        private Hexagon mapHexagon;

        public static GameMap GetInstance
        {
            get { return instance ?? (instance = GameController.FindInstance<GameMap>()); }
        }
        public bool IsEdit
        {
            get { return isEdit; }
            set { isEdit = value; }
        }
        public IMap<IntVector2, MapNode> MapCollection
        {
            get { return mapCollection; }
        }
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }

        private void Awake()
        {
            mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
        }

        private void Start()
        {
            InitMap();
        }

        /// <summary>
        /// 若为编辑状态则初始化编辑地图,否则初始化存档地图;
        /// </summary>
        private void InitMap()
        {
            if (isEdit)
            {
                BlockEditMap<MapNode> editMap = new BlockEditMap<MapNode>(
                    mapBlockIOInfo.addressPrefix, blocksMapInfo);
                AppendTo(editMap as IArchiveInThread);
                AppendTo(editMap as IBuildGameInThread);
                mapCollection = editMap;
            }
            else
            {
                BlockArchiverMap<MapNode> archiverMap = new BlockArchiverMap<MapNode>(
                    mapBlockIOInfo, blocksMapInfo);
                AppendTo(archiverMap as IArchiveInThread);
                AppendTo(archiverMap as IBuildGameInThread);
                mapCollection = archiverMap;
            }
        }

        private void AppendTo(IArchiveInThread item)
        {
            IBuildGameData buildGame = Initializers.BuildGameData;
            buildGame.AppendArchiveGame.Add(item);
        }

        private void AppendTo(IBuildGameInThread item)
        {
            IBuildGameData buildGame = Initializers.BuildGameData;
            buildGame.AppendBuildGame.Add(item);
        }

        internal void UpdateMap(Vector2 planePoint)
        {
            IntVector2 mapPoint = mapHexagon.TransfromPoint(planePoint);
            mapCollection.UpdateMapData(mapPoint);
        }

        #region 坐标转换;

        /// <summary>
        /// 获取到地图坐标;
        /// </summary>
        public IntVector2 PlaneToMapPoint(Vector2 planePoint)
        {
            IntVector2 mapPosition = mapHexagon.TransfromPoint(planePoint);
            return mapPosition;
        }

        #endregion

    }

}
