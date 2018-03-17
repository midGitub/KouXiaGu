using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace JiongXiaGu.Unity.RectTerrain
{

    public sealed class BakeDrawingBoardMesh
    {
        public const string MeshName = "Landform Bake DrawingBoard Mesh";
        private static readonly float chunkHalfWidth = LandformBake.BakeMeshWidth / 2;
        private static readonly float chunkHalfHeight = LandformBake.BakeMeshHeight / 2;
        private const float altitude = 0;

        /// <summary>
        /// 网格顶点数据;
        /// </summary>
        private static readonly Vector3[] vertices = new Vector3[]
            {
                new Vector3(-chunkHalfWidth , altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, chunkHalfHeight),
                new Vector3(chunkHalfWidth, altitude, -chunkHalfHeight),
                new Vector3(-chunkHalfWidth, altitude, -chunkHalfHeight),
            };

        /// <summary>
        /// 网格三角形数据;
        /// </summary>
        private static readonly int[] triangles = new int[]
           {
                0,1,2,
                0,2,3,
           };

        /// <summary>
        /// 网格UV坐标数据;
        /// </summary>
        private static readonly Vector2[] uvs = new Vector2[]
           {
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
                new Vector2(0f, 0f),
           };

        private static Mesh publicMesh;

        /// <summary>
        /// 创建一个新的地形块网格结构;
        /// </summary>
        private static Mesh CreateMesh()
        {
            Mesh mesh = new Mesh();
            UpdateMesh(ref mesh);
            return mesh;
        }

        private static void UpdateMesh(ref Mesh mesh)
        {
            mesh.Clear();
            mesh.name = MeshName;
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
        }

        private static Mesh GetPublicMesh()
        {
            if (publicMesh == null)
            {
                publicMesh = CreateMesh();
            }
            return publicMesh;
        }


        private MeshFilter meshFilter;

        public BakeDrawingBoardMesh(MeshFilter meshFilter)
        {
            if (meshFilter == null)
                throw new ArgumentNullException(nameof(meshFilter));

            this.meshFilter = meshFilter;
            meshFilter.sharedMesh = GetPublicMesh();
        }

        public void Reset()
        {
            meshFilter.sharedMesh = GetPublicMesh();
        }
    }

    public sealed class BakeDrawingBoardRenderer
    {
        private static readonly Lazy<Shader> displayInBakeDrawingBoardShader = new Lazy<Shader>(delegate ()
        {
            return Shader.Find("RectLandform/DisplayInMesh");
        });

        private MeshRenderer meshRenderer;
        private Material meterial;

        public BakeDrawingBoardRenderer(MeshRenderer meshRenderer)
        {
            if (meshRenderer == null)
                throw new ArgumentNullException(nameof(meshRenderer));

            this.meshRenderer = meshRenderer;
            meterial = meshRenderer.material = new Material(displayInBakeDrawingBoardShader.Value);
        }

        public void Reset()
        {
            SetDiffuseMap(null);
            SetBlendMap(null);
        }

        public void SetDiffuseMap(Texture texture)
        {
            meterial.SetTexture("_MainTex", texture);
        }

        public void SetBlendMap(Texture texture)
        {
            meterial.SetTexture("_BlendTex", texture);
        }
    }

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class BakeDrawingBoard : MonoBehaviour
    {
        private BakeDrawingBoard()
        {
        }

        public BakeDrawingBoardMesh Mesh { get; private set; }
        public BakeDrawingBoardRenderer Renderer { get; private set; }

        private void Awake()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;

            Mesh = new BakeDrawingBoardMesh(meshFilter);
            Renderer = new BakeDrawingBoardRenderer(meshRenderer);
        }

        public void Reset()
        {
            Mesh.Reset();
            Renderer.Reset();
        }
    }
}
