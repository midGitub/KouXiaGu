using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Terrain3D.DynamicMesh
{

    public class DynamicMeshFilePath : MultipleFilePath
    {
        public const string fileName = "World\\Terrain\\DynamicMeshs\\Mesh_.data";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class DynamicMeshReaderWriter : FilesReaderWriter<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>>
    {
        static readonly IFileSerializer<KeyValuePair<string, DynamicMeshData>> fileSerializer = new ProtoFileSerializer<KeyValuePair<string, DynamicMeshData>>();

        static readonly ICombiner<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>> combiner = new DataCombiner();

        public DynamicMeshReaderWriter() : base(new DynamicMeshFilePath(), fileSerializer, combiner)
        {
        }

        class DataCombiner : ICombiner<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>>
        {
            public Dictionary<string, DynamicMeshData> Combine(IEnumerable<KeyValuePair<string, DynamicMeshData>> items)
            {
                Dictionary<string, DynamicMeshData> dictionary = new Dictionary<string, DynamicMeshData>();
                foreach (var item in items)
                {
                    string name = item.Key;
                    DynamicMeshData data = item.Value;
                    if (data == null)
                    {
                        Debug.LogWarning("[DynamicMeshReaderWriter]ArgumentNullException");
                    }
                    if (dictionary.ContainsKey(name))
                    {
                        Debug.LogWarning("[DynamicMeshReaderWriter]读取到相同Key:" + name + ";");
                    }
                    else
                    {
                        dictionary.Add(name, item.Value);
                    }
                }
                return dictionary;
            }

            public IEnumerable<WriteInfo<KeyValuePair<string, DynamicMeshData>>> Separate(Dictionary<string, DynamicMeshData> dictionary)
            {
                foreach (var item in dictionary)
                {
                    yield return new WriteInfo<KeyValuePair<string, DynamicMeshData>>(item.Key, item);
                }
            }
        }
    }

}
