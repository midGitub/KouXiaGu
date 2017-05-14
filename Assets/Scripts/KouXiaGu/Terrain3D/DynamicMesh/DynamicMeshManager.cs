using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.Wall
{

    [DisallowMultipleComponent]
    public class DynamicMeshManager : UnitySington<DynamicMeshManager>
    {
        DynamicMeshManager()
        {
        }

        /// <summary>
        /// 用于存储网格数据;
        /// </summary>
        [SerializeField]
        List<SerializableDynamicMeshData> serializableDataList;
        Dictionary<string, DynamicMeshData> meshDataDictionary;

        /// <summary>
        /// 运行时获取网格数据的字典;
        /// </summary>
        public IReadOnlyDictionary<string, DynamicMeshData> MeshDataDictionary { get; private set; }

        void Awake()
        {
            SetInstance(this);
            meshDataDictionary = ToDictionary(serializableDataList);
            MeshDataDictionary = meshDataDictionary.AsReadOnlyDictionary();
        }

        Dictionary<string, DynamicMeshData> ToDictionary(List<SerializableDynamicMeshData> list)
        {
            Dictionary<string, DynamicMeshData> dictionary = new Dictionary<string, DynamicMeshData>();
            foreach (var item in list)
            {
                string name = item.Name;
                DynamicMeshData meshData = item.MeshData;
                dictionary.Add(name, meshData);
            }
            return dictionary;
        }

        /// <summary>
        /// 检查Unity运行模式;
        /// </summary>
        void CheckGameMode()
        {
            if (Application.isPlaying)
            {
                throw new CanNotEditException();
            }
        }

        /// <summary>
        /// 加入对应网格信息;(仅非运行编辑模式下有效)
        /// </summary>
        public void AddInEditor(string name, DynamicMeshData meshData)
        {
            CheckGameMode();
            if (meshData == null)
            {
                throw new ArgumentNullException();
            }
            if (ContainsInEditor(name))
            {
                throw new ArgumentException();
            }
            SerializableDynamicMeshData serializableData = new SerializableDynamicMeshData(name, meshData);
            serializableDataList.Add(serializableData);
        }

        /// <summary>
        /// 移除对应网格信息;(仅非运行编辑模式下有效)
        /// </summary>
        public bool RemoveInEditor(string name)
        {
            CheckGameMode();
            int index = FindIndexInEditor(name);
            if (index >= 0)
            {
                serializableDataList.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        int FindIndexInEditor(string name)
        {
            return serializableDataList.FindIndex(item => item.Name == name);
        }

        /// <summary>
        /// 确认是否存在对应网格数据;
        /// </summary>
        public bool ContainsInEditor(string name)
        {
            return FindIndexInEditor(name) >= 0;
        }

        /// <summary>
        /// 获取到对应网格数据,若不存在则返回异常;
        /// </summary>
        public DynamicMeshData Find(string name)
        {
            if (Application.isPlaying)
            {
                return meshDataDictionary[name];
            }
            else
            {
                return FindInEditor(name);
            }
        }

        /// <summary>
        /// 获取到对应网格数据,若不存在则返回异常;
        /// </summary>
        public DynamicMeshData FindInEditor(string name)
        {
            int index = FindIndexInEditor(name);
            if (index >= 0)
            {
                var data = serializableDataList[index];
                DynamicMeshData meshData = data.MeshData;
                return meshData;
            }
            else
            {
                throw new KeyNotFoundException();
            }
        }
    }

    /// <summary>
    /// 用于序列化保存;
    /// </summary>
    [Serializable]
    public struct SerializableDynamicMeshData
    {
        public SerializableDynamicMeshData(string name, DynamicMeshData meshData)
        {
            Name = name;
            MeshData = meshData;
        }

        public string Name;
        public DynamicMeshData MeshData;
    }

}
