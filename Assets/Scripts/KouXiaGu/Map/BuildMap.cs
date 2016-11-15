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
    public class BuildMap : MonoBehaviour
    {
        [SerializeField, Tooltip("是否为编辑地图模式?")]
        private bool isEdit;

        [SerializeField, Tooltip("地图六边形的外径")]
        private float hexOuterDiameter = 2;

        [SerializeField]
        private MapBlockIOInfo mapBlockIOInfo;

        [SerializeField]
        private BlocksMapInfo blocksMapInfo;

        private static BuildMap instance;
        private IDynaMap<IntVector2, HexMapNode> mapCollection;
        private Hexagon mapHexagon;
        private IDisposable targetDisposable;
        private IObservable<Vector2> targetObservable;

        public static BuildMap GetInstance
        {
            get { return instance ?? (instance = FindInstance()); }
        }
        public IMap<IntVector2, HexMapNode> MapCollection
        {
            get { return mapCollection; }
        }
        public Hexagon MapHexagon
        {
            get { return mapHexagon; }
        }
        public IObservable<Vector2> TargetObservable
        {
            get
            {
                return targetObservable;
            }
            set {
                if (targetDisposable != null)
                    targetDisposable.Dispose();
                targetObservable = value;
                targetDisposable = targetObservable.Subscribe(UpdateMapRes);
            }
        }

        private void Awake()
        {
            instance = FindInstance();

            mapHexagon = new Hexagon() { OuterDiameter = hexOuterDiameter };
            InitMap();
        }

        private void InitMap()
        {
            if (isEdit)
            {
                BlockEditMap<HexMapNode> editMap = new BlockEditMap<HexMapNode>(
                    mapBlockIOInfo.addressPrefix, blocksMapInfo);
                AppendTo(editMap as IArchiveInThread);
                AppendTo(editMap as IBuildGameInThread);
                mapCollection = editMap;
            }
            else
            {
                BlockArchiverMap<HexMapNode> archiverMap = new BlockArchiverMap<HexMapNode>(
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

        private void UpdateMapRes(Vector2 targetPosition)
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

        private static BuildMap FindInstance()
        {
            return GameObject.FindWithTag("GameController").GetComponent<BuildMap>();
        }


    }

}
