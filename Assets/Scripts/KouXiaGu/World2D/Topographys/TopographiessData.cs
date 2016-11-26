using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using KouXiaGu.World2D.Map;

namespace KouXiaGu.World2D
{


    /// <summary>
    /// 定义地貌预制ID和其信息转换;
    /// </summary>
    [Serializable]
    public class TopographiessData : UnitySingleton<TopographiessData>
    {
        TopographiessData() { }

        /// <summary>
        /// 定义地貌信息的XML文件;
        /// </summary>
        [SerializeField]
        string TopographyInfosXMLFileName = "Topographys.xml";
        [SerializeField]
        TopographyInfo[] topographyInfos;
        [SerializeField]
        Topography[] topographiess;

        Dictionary<int, TopographyInfo> topographyInfosDictionary;
        Dictionary<int, Topography> topographiessDictionary;

        public string TopographyInfosXMLFilePath
        {
            get { return Path.Combine(ResCoreData.CoreDataDirectoryPath, TopographyInfosXMLFileName); }
        }


        public void Start()
        {
            topographyInfosDictionary = topographyInfos.ToDictionary(item => item.id);
            topographiessDictionary = topographiess.ToDictionary(item => item.ID);
        }


        /// <summary>
        /// 根据ID获取到地貌详细信息;
        /// </summary>
        public TopographyInfo GetInfoWithID(int id)
        {
            return topographyInfosDictionary[id];
        }

        /// <summary>
        /// 根据ID获取到预制物体;
        /// </summary>
        public Topography GetPrefabWithID(int id)
        {
            return topographiessDictionary[id];
        }


        //[ContextMenu("输出到XML")]
        //void Input_TopographyPrefab()
        //{
        //    TopographyInfo[] topographys = topographyPrefabs.Select(item => item.Info).ToArray();
        //    SerializeHelper.Serialize_Xml(TopographyInfosXMLFilePath, topographys);
        //}

        //[ContextMenu("输入到XML")]
        //void OutPut_TopographyPrefab()
        //{
        //    TopographyInfo[] topographys = SerializeHelper.Deserialize_Xml<TopographyInfo[]>(TopographyInfosXMLFilePath);
        //    foreach (var item in topographys)
        //    {
        //        topographyPrefabs.First(topographyPrefab => topographyPrefab.id == item.id).Info = item;
        //    }
        //}

    }

}
