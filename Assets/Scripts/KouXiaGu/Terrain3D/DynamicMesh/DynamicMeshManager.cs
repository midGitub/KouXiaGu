using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.Collections;
using KouXiaGu.Resources;

namespace KouXiaGu.Terrain3D.Wall
{

    public class DynamicMeshFilePath : MultipleFilePath
    {
        public const string fileName = "World\\Terrain\\DynamicMesh\\Mesh.data";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class DynamicMeshSerializer : FilesReaderWriter<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>>
    {
        static readonly IFileSerializer<KeyValuePair<string, DynamicMeshData>> fileSerializer = new ProtoFileSerializer<KeyValuePair<string, DynamicMeshData>>();

        static readonly ICombiner<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>> combiner = new DataCombiner();

        public DynamicMeshSerializer(IMultipleFilePath file) : base(file, fileSerializer, combiner)
        {
        }

        class DataCombiner : ICombiner<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>>
        {
            public Dictionary<string, DynamicMeshData> Combine(IEnumerable<KeyValuePair<string, DynamicMeshData>> items)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<KeyValuePair<string, KeyValuePair<string, DynamicMeshData>>> Separate(Dictionary<string, DynamicMeshData> item)
            {
                throw new NotImplementedException();
            }
        }
    }


    /// <summary>
    /// 动态网格资源管理;
    /// </summary>
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
        
        bool isReadFromDictionary
        {
            get { return meshDataDictionary != null; }
        }

        void Awake()
        {
            SetInstance(this);
            meshDataDictionary = ToDictionary(serializableDataList);
        }

        /// <summary>
        /// 从文件读取到动态网格信息;
        /// </summary>
        public void Read()
        {

        }

        public void Write()
        {

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
        /// 加入对应网格信息;
        /// </summary>
        public void Add(string name, DynamicMeshData meshData)
        {
            if (meshData == null)
            {
                throw new ArgumentNullException();
            }

            if (isReadFromDictionary)
            {
                meshDataDictionary.Add(name, meshData);
            }
            else
            {
                if (FindIndexInEditor(name) >= 0)
                {
                    throw new ArgumentException("已经存在相同名字的网格;");
                }
                SerializableDynamicMeshData serializableData = new SerializableDynamicMeshData(name, meshData);
                serializableDataList.Add(serializableData);
            }
        }

        /// <summary>
        /// 在可序列化链表内寻找;
        /// </summary>
        int FindIndexInEditor(string name)
        {
            return serializableDataList.FindIndex(item => item.Name == name);
        }

        /// <summary>
        /// 加入对应网格信息,若已经存在,则更新其;
        /// </summary>
        public void AddOrUpdate(string name, DynamicMeshData meshData)
        {
            if (meshData == null)
            {
                throw new ArgumentNullException();
            }

            if (isReadFromDictionary)
            {
                meshDataDictionary.AddOrUpdate(name, meshData);
            }
            else
            {
                SerializableDynamicMeshData serializableData = new SerializableDynamicMeshData(name, meshData);
                int index = FindIndexInEditor(name);
                if (index >= 0)
                {
                    serializableDataList[index] = serializableData;
                }
                else
                {
                    serializableDataList.Add(serializableData);
                }
            }
        }

        /// <summary>
        /// 移除对应的网格;
        /// </summary>
        public bool Remove(string name)
        {
            if (isReadFromDictionary)
            {
                return meshDataDictionary.Remove(name);
            }
            else
            {
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
        }

        /// <summary>
        /// 确认是否存在对应网格数据;
        /// </summary>
        public bool Contains(string name)
        {
            if (isReadFromDictionary)
            {
                return meshDataDictionary.ContainsKey(name);
            }
            else
            {
                return FindIndexInEditor(name) >= 0;
            }
        }

        /// <summary>
        /// 获取到对应网格数据,若不存在则返回异常;
        /// </summary>
        public DynamicMeshData Find(string name)
        {
            if (isReadFromDictionary)
            {
                return meshDataDictionary[name];
            }
            else
            {
                return FindInEditor(name);
            }
        }

        /// <summary>
        /// 在可序列化链表内寻找到对应网格数据,若不存在则返回异常;
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

        /// <summary>
        /// 用于序列化保存;
        /// </summary>
        [Serializable]
        struct SerializableDynamicMeshData
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
}
