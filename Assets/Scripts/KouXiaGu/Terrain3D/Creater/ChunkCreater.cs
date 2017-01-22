using System;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


    [DisallowMultipleComponent]
    public class ChunkCreater : SceneSington<ChunkCreater>
    {

        static MapData Data
        {
            get { return MapDataManager.ActiveData; }
        }

        /// <summary>
        /// 地形块池;
        /// </summary>
        static Pool RestingChunks
        {
            get { return GetInstance.restingChunks; }
        }

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        static CustomDictionary<RectCoord, TerrainChunk> ActivatedChunks
        {
            get { return GetInstance.activatedChunks; }
        }

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        public static IReadOnlyDictionary<RectCoord, TerrainChunk> ReadOnlyActivatedChunks
        {
            get { return ActivatedChunks; }
        }


        /// <summary>
        /// 创建此地形块,若已经存在 或者 正在创建中 则返回true;
        /// </summary>
        public static bool Create(RectCoord coord)
        {
            if (!IsInstanced)
                throw new ArgumentNullException("类未实例化");

            if (ActivatedChunks.ContainsKey(coord) || Request.Requested.ContainsKey(coord))
                return false;

            Request.Create(coord, Data);
            return true;
        }

        /// <summary>
        /// 创建一个地图块,并且初始化后加入已激活列表;
        /// </summary>
        static void Create(RectCoord coord, TerrainTexPack textures, BuildingGroup buildings)
        {
            if (!IsInstanced)
                throw new ArgumentNullException("类未实例化");

            TerrainChunk chunk = RestingChunks.Get();

            chunk.Set(textures);
            chunk.BuildingGroup = buildings;

            ActivatedChunks.Add(coord, chunk);
        }


        ChunkCreater()
        {
        }

        /// <summary>
        /// 地形块对象池;
        /// </summary>
        [SerializeField]
        Pool restingChunks;

        /// <summary>
        /// 在场景中激活的地形块;
        /// </summary>
        CustomDictionary<RectCoord, TerrainChunk> activatedChunks;

        protected override void Awake()
        {
            base.Awake();
            restingChunks.Initialize();
            activatedChunks = new CustomDictionary<RectCoord, TerrainChunk>();
        }

        protected override void OnDestroy()
        {
            Request.Clear();
            base.OnDestroy();
        }


        [Serializable]
        class Pool : ObjectPool<TerrainChunk>
        {
            protected override TerrainChunk Instantiate()
            {
                return new TerrainChunk();
            }

            protected override void Reset(TerrainChunk item)
            {
                item.Reset();
            }

            protected override void Destroy(TerrainChunk item)
            {
                item.Destroy();
            }
        }


        /// <summary>
        /// 地形块创建请求;
        /// </summary>
        class Request : IEquatable<Request>, IBuildRequest, IBakeRequest
        {

            /// <summary>
            /// 创建请求;
            /// </summary>
            static readonly CustomDictionary<RectCoord, Request> requested =
                new CustomDictionary<RectCoord, Request>();

            /// <summary>
            /// 创建请求;
            /// </summary>
            public static IReadOnlyDictionary<RectCoord, Request> Requested
            {
                get { return requested; }
            }

            /// <summary>
            /// 创建这个地形块,若已经存在则返回false;
            /// </summary>
            public static bool Create(RectCoord chunkCoord, MapData data)
            {
                if (requested.ContainsKey(chunkCoord))
                    return false;

                Request request = new Request(chunkCoord, data);
                requested.Add(chunkCoord, request);
                request.RequesteAdd();

                return true;
            }

            /// <summary>
            /// 取消创建这个地图块;
            /// </summary>
            public static bool Remove(RectCoord chunkCoord)
            {
                Request request;
                if (requested.TryGetValue(chunkCoord, out request))
                {
                    requested.Remove(chunkCoord);
                    request.RequesteRemove();
                    request.Destroy();
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 清除所有的请求;
            /// </summary>
            internal static void Clear()
            {
                requested.Clear();
            }



            Request(RectCoord chunkCoord, MapData data)
            {
                this.ChunkCoord = chunkCoord;
                this.Data = data;
            }

            TerrainTexPack textures;
            BuildingGroup buildings;
            public RectCoord ChunkCoord { get; private set; }
            public MapData Data { get; private set; }

            void RequesteAdd()
            {
                TerrainBaker.Requested.AddLast(this);
                Architect.Requested.AddLast(this);
            }

            void RequesteRemove()
            {
                TerrainBaker.Requested.Remove(this);
                Architect.Requested.Remove(this);
            }

            void IBakeRequest.OnComplete(TerrainTexPack textures)
            {
                this.textures = textures;
                CreateChunk();
            }

            void IBuildRequest.OnComplete(BuildingGroup buildings)
            {
                this.buildings = buildings;
                CreateChunk();
            }

            void CreateChunk()
            {
                if (!requested.ContainsKey(ChunkCoord))
                    Debug.LogError("请求已经不存在,但是还是要求创建");

                if (textures != null && buildings != null)
                {
                    ChunkCreater.Create(ChunkCoord, textures, buildings);
                    requested.Remove(ChunkCoord);
                }
            }

            void Destroy()
            {
                if (textures != null)
                {
                    textures.Destroy();
                    textures = null;
                }
                if (buildings != null)
                {
                    buildings.Destroy();
                    buildings = null;
                }
            }

            public void OnError(Exception ex)
            {
                Debug.LogError(ex);
            }


            public override bool Equals(object obj)
            {
                Request _obj = obj as Request;

                if (obj == null)
                    return false;

                return Equals(_obj);
            }

            public bool Equals(Request other)
            {
                return 
                    other != null && 
                    other.ChunkCoord == ChunkCoord && 
                    other.Data == Data;
            }

            public override int GetHashCode()
            {
                return ChunkCoord.GetHashCode() ^ Data.GetHashCode();
            }

        }

    }

}
