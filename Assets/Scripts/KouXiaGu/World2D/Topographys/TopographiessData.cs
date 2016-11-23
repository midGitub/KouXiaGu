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
    [DisallowMultipleComponent]
    public class TopographiessData : UnitySingleton<TopographiessData>
    {
        TopographiessData() { }

        /// <summary>
        /// 定义地貌信息的XML文件;
        /// </summary>
        [SerializeField]
        string TopographyInfosXMLFileName;

        /// <summary>
        /// 预制定义;
        /// </summary>
        [SerializeField]
        TopographyPrefab[] topographyPrefabs;

        Dictionary<int, TopographyPrefab> topographyPrefabDictionary;

        public string TopographyInfosXMLFilePath
        {
            get { return Path.Combine(ResCoreData.CoreDataDirectoryPath, TopographyInfosXMLFileName); }
        }

        public TopographyPrefab GetWithID(int id)
        {
            return topographyPrefabDictionary[id];
        }

        void Start()
        {
            topographyPrefabDictionary = GetDictionary();
        }

        Dictionary<int, TopographyPrefab> GetDictionary()
        {
            Dictionary<int, TopographyPrefab> topographyPrefabDictionary = new Dictionary<int, TopographyPrefab>();

            foreach (var topographyPrefab in topographyPrefabs)
            {
                try
                {
                    topographyPrefabDictionary.Add(topographyPrefab.id, topographyPrefab);
                }
                catch (ArgumentException)
                {
                    Debug.LogWarning("地貌存在相同定义的ID:" + topographyPrefab.id + ";已进行忽略");
                }
            }

            return topographyPrefabDictionary;
        }

        [ContextMenu("输出到XML")]
        void Input_TopographyPrefab()
        {
            Topography[] topographys = topographyPrefabs.Select(item => item.topography).ToArray();
            SerializeHelper.Serialize_Xml(TopographyInfosXMLFilePath, topographys);
        }

        [ContextMenu("输入到XML")]
        void OutPut_TopographyPrefab()
        {
            Topography[] topographys = SerializeHelper.Deserialize_Xml<Topography[]>(TopographyInfosXMLFilePath);
            foreach (var item in topographys)
            {
                topographyPrefabs.First(topographyPrefab => topographyPrefab.id == item.id).topography = item;
            }
        }

    }

}
