using KouXiaGu.OperationRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.World.Map.MapEdit
{

    /// <summary>
    /// 地图编辑操作;
    /// </summary>
    public interface IMapEditHandler
    {
        /// <summary>
        /// 预留信息;
        /// </summary>
        string GetMessage();

        /// <summary>
        /// 对比;
        /// </summary>
        bool Contrast(IMapEditHandler handler);

        /// <summary>
        /// 对所有节点执行操作,若不存在独立的撤销操作则返回Null;
        /// </summary>
        IVoidable Execute(IEnumerable<EditMapNode> nodes);
    }
}
