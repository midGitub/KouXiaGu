using System;
using System.Collections.Generic;
using UnityEngine;
using KouXiaGu.Collections;
using System.IO;

namespace KouXiaGu.Terrain3D.DynamicMeshs
{

    /// <summary>
    /// 动态网格资源管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class DynamicMeshManager : UnitySington<DynamicMeshManager>
    {
        DynamicMeshManager()
        {
        }

        static DynamicMeshReaderWriter _dynamicMeshSerializer;

        static DynamicMeshReaderWriter dynamicMeshSerializer
        {
            get { return _dynamicMeshSerializer ?? (_dynamicMeshSerializer = new DynamicMeshReaderWriter()); }
        }

        Dictionary<string, DynamicMeshData> meshDataDictionary;
        
        bool isReadFromDictionary
        {
            get { return meshDataDictionary != null; }
        }

        void Awake()
        {
            SetInstance(this);
            ReadAll();
        }

        /// <summary>
        /// 从文件读取到所有网格信息;
        /// </summary>
        [ContextMenu("Read")]
        public void ReadAll()
        {
            meshDataDictionary = dynamicMeshSerializer.Read();
        }

        /// <summary>
        /// 输出所有网格信息到文件;
        /// </summary>
        [ContextMenu("Write")]
        public void WriteAll()
        {
            dynamicMeshSerializer.Write(meshDataDictionary);
        }

        public DynamicMeshData Read(string name)
        {
            var data = dynamicMeshSerializer.Read(name);
            if (data.Key != name)
            {
                throw new ArgumentException("文件命名与预定义网格名不同,读取失败;");
            }
            return data.Value;
        }

        /// <summary>
        /// 将网格信息输出到文件;
        /// </summary>
        public void Write(string name, DynamicMeshData meshData, FileMode fileMode)
        {
            if (meshData == null)
                throw new ArgumentNullException("meshData");

            KeyValuePair<string, DynamicMeshData> data = new KeyValuePair<string, DynamicMeshData>(name, meshData);
            dynamicMeshSerializer.Write(data, name, fileMode);
        }

        /// <summary>
        /// 加入对应网格信息,若在编辑模式下则为输出到文件;
        /// </summary>
        public void Add(string name, DynamicMeshData meshData)
        {
            if (meshData == null)
                throw new ArgumentNullException("meshData");

            if (isReadFromDictionary)
            {
                meshDataDictionary.Add(name, meshData);
            }
            else
            {
                try
                {
                    Write(name, meshData, FileMode.CreateNew);
                }
                catch (IOException)
                {
                    throw new ArgumentException("已经存在相同名字的网格;");
                }
            }
        }

        /// <summary>
        /// 加入对应网格信息,若已经存在,则更新其;
        /// </summary>
        public void AddOrUpdate(string name, DynamicMeshData meshData)
        {
            if (meshData == null)
                throw new ArgumentNullException("meshData");

            if (isReadFromDictionary)
            {
                meshDataDictionary.AddOrUpdate(name, meshData);
            }
            else
            {
                Write(name, meshData, FileMode.Create);
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
                return dynamicMeshSerializer.Delete(name);
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
                return dynamicMeshSerializer.Exists(name);
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
                return dynamicMeshSerializer.Read(name).Value;
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
