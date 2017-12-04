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
    public class RectMapController : MonoBehaviour, IComponentInitializeHandle
    {
        private const string InitializerName = "地图数据初始化";

        private MapSearcher mapSearcher;

        private List<MapFileInfo> availableMaps
        {
            get { return RectMap.AvailableMaps; }
            set { RectMap.AvailableMaps = value; }
        }

        Task IComponentInitializeHandle.Initialize(CancellationToken token)
        {
            mapSearcher = new MapSearcher();

            availableMaps = mapSearcher.Find(LoadableResource.All);
            if (availableMaps.Count == 0)
            {
                throw new FileNotFoundException("未找到可用的文件");
            }

            UnityDebugHelper.SuccessfulReport(InitializerName, () => GetInfoLog());
            return Task.CompletedTask;
        }

        [ContextMenu("报告详细信息")]
        private void LogInfo()
        {
            Debug.Log(GetInfoLog());
        }

        private string GetInfoLog()
        {
            string log = "可使用地图总数 : " + RectMap.AvailableMaps.Count
                + ", 可使用地图 : " + string.Join(", ", RectMap.AvailableMaps.Select(map => string.Format("[Name : {0}]", map.Description.Name)))
                ;
            return log;
        }
    }
}
