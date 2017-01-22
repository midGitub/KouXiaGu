using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{

    [DisallowMultipleComponent]
    public class ChunkCreater : SceneSington<ChunkCreater>
    {

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
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建一个地图块,并且初始化后加入已激活列表;
        /// </summary>
        static void Create(RectCoord coord, TerrainTexPack textures, BuildingGroup buildings)
        {
            TerrainChunk chunk = RestingChunks.Get();

            chunk.Set(textures);
            chunk.BuildingGroup = buildings;

            ActivatedChunks.Add(coord, chunk);
        }


        ChunkCreater()
        {
        }

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


        class Request : IEquatable<Request>, IBuildRequest, IBakeRequest
        {

            /// <summary>
            /// 请求创建的地形块;
            /// </summary>
            static readonly CustomDictionary<RectCoord, Request> requested =
                new CustomDictionary<RectCoord, Request>();

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

                Request request = new Request()
                {
                    ChunkCoord = chunkCoord,
                    Data = data,
                };
                requested.Add(chunkCoord, request);
                return true;
            }

            /// <summary>
            /// 取消创建这个地图块;
            /// </summary>
            public static void Remove(RectCoord chunkCoord)
            {
                throw new NotImplementedException();
            }

            public static void Clear()
            {
                requested.Clear();
            }


            Request()
            {
                TerrainBaker.Requested.AddLast(this);
            }


            TerrainTexPack textures;
            public MapData Data { get; private set; }
            public RectCoord ChunkCoord { get; private set; }


            public override bool Equals(object obj)
            {
                Request _obj = obj as Request;

                if (obj == null)
                    return false;

                return Equals(_obj);
            }

            public bool Equals(Request other)
            {
                return other.ChunkCoord == ChunkCoord && other.Data == Data;
            }

            public override int GetHashCode()
            {
                return ChunkCoord.GetHashCode() ^ Data.GetHashCode();
            }

            void IBakeRequest.OnComplete(TerrainTexPack textures)
            {
                if (requested.ContainsKey(ChunkCoord))
                {
                    this.textures = textures;
                    Architect.RequestQueue.AddLast(this);
                }
                else
                {
                    textures.Destroy();
                    requested.Remove(ChunkCoord);
                }
            }

            void IBakeRequest.OnError(Exception ex)
            {
                Debug.LogError(ex);
            }

            void IBuildRequest.OnComplete(BuildingGroup buildings)
            {
                throw new NotImplementedException();
            }

            void IBuildRequest.OnError(Exception ex)
            {
                Debug.LogError(ex);
            }

        }

    }

}
