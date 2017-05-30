using KouXiaGu.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KouXiaGu.Terrain3D.DynamicMesh
{

    public class DynamicMeshFilePath : MultipleFilePath
    {
        public const string fileName = "World\\Terrain\\DynamicMeshs\\Mesh.data";

        public override string FileName
        {
            get { return fileName; }
        }
    }

    class DynamicMeshReaderWriter : FilesReaderWriter<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>>
    {
        static readonly IFileSerializer<KeyValuePair<string, DynamicMeshData>> fileSerializer = new ProtoFileSerializer<KeyValuePair<string, DynamicMeshData>>();

        static readonly ICombiner<KeyValuePair<string, DynamicMeshData>, Dictionary<string, DynamicMeshData>> combiner = new DataCombiner();

        public DynamicMeshReaderWriter(IMultipleFilePath file) : base(file, fileSerializer, combiner)
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

}
