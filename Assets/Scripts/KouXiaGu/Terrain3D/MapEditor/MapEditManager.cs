using KouXiaGu.Terrain3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Grids;
using KouXiaGu.OperationRecord;

namespace KouXiaGu.Terrain3D.MapEditor
{

    /// <summary>
    /// 地图编辑管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class MapEditManager : MonoBehaviour
    {
        MapEditManager()
        {
        }

        [SerializeField]
        int maxRecordCount = 30;
        public Recorder<IVoidable> Recorder { get; private set; }
        IDisposable recordRegister;
        public IEditOperation Current { get; private set; }


        IWorldComplete world
        {
            get { return WorldSceneManager.World; }
        }

        void Awake()
        {
            Recorder = new Recorder<IVoidable>(maxRecordCount);
        }

        void OnEnable()
        {
            recordRegister = RecordManager.AddLast(Recorder);
        }

        void OnDisable()
        {
            recordRegister.Dispose();
            recordRegister = null;
        }

        void Update()
        {
            if (world != null && Current == null)
            {
                SetCurrentOperation(new ChangeRoadOnly(world.WorldData.MapData, 1));
            }

            if (world != null && Current != null)
            {
                Vector3 mousePoint;
                if (LandformRay.Instance.TryGetMouseRayPoint(out mousePoint))
                {
                    CubicHexCoord position = mousePoint.GetTerrainCubic();
                    if (Input.GetMouseButtonDown(0))
                    {
                        Current.Perform(position);
                    }
                }
            }
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
            public SetEditOperation(MapEditManager parent, IEditOperation original, IEditOperation newValue)
            {
                this.parent = parent;
                this.original = original;
                this.newValue = newValue;
                parent.Current = newValue;
            }

            readonly MapEditManager parent;
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
