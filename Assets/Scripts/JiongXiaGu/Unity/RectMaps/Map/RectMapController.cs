using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
using JiongXiaGu.Unity.Initializers;
using System.Threading;
using JiongXiaGu.Unity.Resources;
using System.IO;

namespace JiongXiaGu.Unity.RectMaps
{

    [DisallowMultipleComponent]
    public class RectMapController : MonoBehaviour, IModificationInitializeHandle
    {
        private const string InitializerName = "地图数据初始化";

        private MapSearcher mapSearcher;

        /// <summary>
        /// 所有可用的地图(在进行初始化之后,仅提供Unity线程对此内容进行变更);
        /// </summary>
        internal static List<MapFileInfo> AvailableMaps { get; set; }

        void IModificationInitializeHandle.Initialize(IReadOnlyList<ModificationContent> mods, CancellationToken token)
        {
            mapSearcher = new MapSearcher();

            AvailableMaps = mapSearcher.Find(mods.Select(item => item.BaseContent));
            if (AvailableMaps.Count == 0)
            {
                throw new FileNotFoundException("未找到可用的文件");
            }

            UnityDebugHelper.SuccessfulReport(InitializerName, () => GetInfoLog());
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        private string GetInfoLog()
        {
            string log = "可使用地图总数 : " + AvailableMaps.Count
                + ", 可使用地图 : " + string.Join(", ", AvailableMaps.Select(map => string.Format("[Name : {0}]", map.Description.Name)))
                ;
            return log;
        }
    }
}
