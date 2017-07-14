using System;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;
using KouXiaGu.World;
using UnityEngine.EventSystems;

namespace KouXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 地图编辑管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class MapEditPanle : MonoBehaviour
    {
        MapEditPanle()
        {
        }


        public IEditOperation Current { get; private set; }

        IRecorder Recorder
        {
            get { return RecordManager.DefaultRecorder; }
        }

        IWorldComplete world
        {
            get { return WorldSceneManager.World; }
        }

        void Update()
        {
            if (world != null && Current != null)
            {
                Vector3 mousePoint;
                if (Input.GetMouseButtonDown(0) && LandformRay.Instance.TryGetMouseRayPoint(out mousePoint) && !EventSystem.current.IsPointerOverGameObject())
                {
                    CubicHexCoord position = mousePoint.GetTerrainCubic();
                    Perform(position);
                }
            }
        }

        public void CreateRoad()
        {
            SetCurrentOperation(new ChangeRoadOnly(world.WorldData.MapData, 1));
        }

        /// <summary>
        /// 设置当前的编辑项目(可撤销);
        /// </summary>
        public void SetCurrentOperation(IEditOperation newValue)
        {
            var operation = new SetEditOperation(this, Current, newValue);
            Recorder.Register(operation);
        }

        /// <summary>
        /// 执行这个编辑项目(可撤销);
        /// </summary>
        void Perform(CubicHexCoord position)
        {
            var operation = Current.Perform(position);
            Recorder.Register(operation);
        }

        sealed class SetEditOperation : SafeVoidable
        {
            public SetEditOperation(MapEditPanle parent, IEditOperation original, IEditOperation newValue)
            {
                this.parent = parent;
                this.original = original;
                this.newValue = newValue;
                parent.Current = newValue;
            }

            readonly MapEditPanle parent;
            readonly IEditOperation original;
            readonly IEditOperation newValue;

            public override void PerformRedo()
            {
                parent.Current = newValue;
            }

            public override void PerformUndo()
            {
                parent.Current = original;
            }
        }
    }
}
