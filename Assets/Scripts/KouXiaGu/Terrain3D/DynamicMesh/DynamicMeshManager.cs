﻿using System;
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

        [SerializeField]
        List<SerializableDynamicMeshData> serializableData;
        Dictionary<string, DynamicMeshData> meshData;

        public List<SerializableDynamicMeshData> SerializableData
        {
            get { return serializableData; }
        }

        void Awake()
        {
            SetInstance(this);
        }

        public void Add(string name, DynamicMeshData meshData)
        {
            
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
