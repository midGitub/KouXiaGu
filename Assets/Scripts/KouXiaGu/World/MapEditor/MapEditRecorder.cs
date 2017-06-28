using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.OperationRecord;
using KouXiaGu.Grids;
using KouXiaGu.World.Map;
using UnityEngine;

namespace KouXiaGu.World.MapEditor
{

    /// <summary>
    /// 地图编辑记录;
    /// </summary>
    public class MapEditRecorder
    {
        const int MaxRecordCount = 50;

        public MapEditRecorder() : this(MaxRecordCount)
        {
        }

        public MapEditRecorder(int maxRecordCount)
        {
            record = new Recorder<IVoidable>(maxRecordCount);
            Current = null;
        }

        readonly Recorder<IVoidable> record;
        public IEditOperation Current { get; private set; }

        public bool IsActivated
        {
            get { return Current != null; }
        }

        public IRecorder Recorder
        {
            get { return record; }
        }

        /// <summary>
        /// 设置当前的编辑项目;
        /// </summary>
        public void SetOperationItem(IEditOperation newValue)
        {
            var operation = new SetEditOperation(this, Current, newValue);
            Recorder.Register(operation);
        }

        /// <summary>
        /// 执行这个编辑项目;
        /// </summary>
        public void Perform(IEditOperation current, CubicHexCoord position)
        {
            if (IsActivated)
            {
                var operation = current.Perform(position);
                Recorder.Register(operation);
            }
        }

        sealed class SetEditOperation : SafeVoidable
        {
            public SetEditOperation(MapEditRecorder parent, IEditOperation original, IEditOperation newValue)
            {
                this.parent = parent;
                this.original = original;
                this.newValue = newValue;
                parent.Current = newValue;
            }

            readonly MapEditRecorder parent;
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
