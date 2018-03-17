using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.RectTerrain
{

    /// <summary>
    /// 地形块;
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public sealed class LandformChunk : MonoBehaviour
    {
        private LandformChunk()
        {
        }

        public LandformMesh Mesh { get; private set; }
        public LandformRenderer Renderer { get; private set; }
        public LandformCollider Collider { get; private set; }

        private void Awake()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            MeshCollider meshCollider = GetComponent<MeshCollider>();

            Mesh = new LandformMesh(meshFilter);
            Renderer = new LandformRenderer(meshRenderer);
            Collider = new LandformCollider(meshCollider, Renderer);
        }

        public void SetValue(BakeResult value)
        {
            Renderer.SetDiffuseMap(value.DiffuseMap);
            Renderer.SetHeightMap(value.HeightMap);
            Collider.Update();
        }
    }
}
