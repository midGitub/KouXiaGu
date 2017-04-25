//using System;
//using KouXiaGu.Collections;
//using System.Collections.Generic;
//using KouXiaGu.Grids;
//using UnityEngine;
//using KouXiaGu.World.Map;

//namespace KouXiaGu.Terrain3D
//{

//    /// <summary>
//    /// 地形块创建;
//    /// </summary>
//    public class LandformCreater
//    {

//        public LandformCreater(IDictionary<CubicHexCoord, MapNode> data)
//        {
//            this.Data = data;
//            restingChunks = new Pool();
//            activatedChunks = new CustomDictionary<RectCoord, OLandformChunk>();
//            OBaker.Initialize();
//        }


//        /// <summary>
//        /// 地形资源;
//        /// </summary>
//        public IDictionary<CubicHexCoord, MapNode> Data { get; private set; }

//        /// <summary>
//        /// 地形块对象池;
//        /// </summary>
//        [SerializeField]
//        Pool restingChunks;

//        /// <summary>
//        /// 在场景中激活的地形块;
//        /// </summary>
//        CustomDictionary<RectCoord, OLandformChunk> activatedChunks;


//        /// <summary>
//        /// 在场景中激活的地形块;
//        /// </summary>
//        public IReadOnlyDictionary<RectCoord, OLandformChunk> ActivatedChunks
//        {
//            get { return activatedChunks; }
//        }


//        /// <summary>
//        /// 创建此地形块,若已经存在 或者 正在创建中 则返回true;
//        /// </summary>
//        public bool Create(RectCoord coord)
//        {
//            if (activatedChunks.ContainsKey(coord) || Request.Requested.ContainsKey(coord))
//                return false;

//            Request.Create(coord, Data, CreateOrUpdate);
//            return true;
//        }

//        /// <summary>
//        /// 创建或者更新已创建的地图块;
//        /// </summary>
//        public void CreateOrUpdate(RectCoord coord)
//        {
//            Request.Create(coord, Data, CreateOrUpdate);
//        }

//        void CreateOrUpdate(Request request)
//        {
//            RectCoord coord = request.ChunkCoord;
//            TerrainTexPack textures = request.Textures;
//            OLandformChunk chunk;

//            if (!activatedChunks.TryGetValue(request.ChunkCoord, out chunk))
//            {
//                chunk = restingChunks.Get();
//                activatedChunks.Add(coord, chunk);
//            }

//            chunk.Set(coord);
//            chunk.Set(textures);
//        }

//        /// <summary>
//        /// 取消创建或者摧毁已创建的;
//        /// </summary>
//        public bool Destroy(RectCoord coord)
//        {
//            OLandformChunk chunk;

//            if (activatedChunks.TryGetValue(coord, out chunk))
//            {
//                chunk.Destroy();
//                activatedChunks.Remove(coord);
//                return true;
//            }
//            else
//            {
//                return Request.Remove(coord);
//            }
//        }



//        class Pool : ObjectPool<OLandformChunk>
//        {
//            public override OLandformChunk Instantiate()
//            {
//                return new OLandformChunk();
//            }

//            public override void ResetWhenEnterPool(OLandformChunk item)
//            {
//                item.Reset();
//            }

//            public override void Destroy(OLandformChunk item)
//            {
//                item.Destroy();
//            }

//            public override void ResetWhenOutPool(OLandformChunk item)
//            {
//                throw new NotImplementedException();
//            }
//        }


//        /// <summary>
//        /// 地形块创建请求;
//        /// </summary>
//        class Request : IEquatable<Request>, IBakeRequest
//        {

//            /// <summary>
//            /// 创建请求;
//            /// </summary>
//            static readonly CustomDictionary<RectCoord, Request> requested =
//                new CustomDictionary<RectCoord, Request>();

//            /// <summary>
//            /// 创建请求;
//            /// </summary>
//            public static IReadOnlyDictionary<RectCoord, Request> Requested
//            {
//                get { return requested; }
//            }

//            /// <summary>
//            /// 创建这个地形块,若已经在队列则返回false;
//            /// </summary>
//            /// <param name="callback">当完成所有进度时回调;</param>
//            public static bool Create(RectCoord chunkCoord, IDictionary<CubicHexCoord, MapNode> data, Action<Request> callback)
//            {
//                if (callback == null)
//                    throw new ArgumentNullException();
//                if (requested.ContainsKey(chunkCoord))
//                    return false;

//                Request request = new Request(chunkCoord, data, callback);
//                requested.Add(chunkCoord, request);
//                request.RequesteAdd();

//                return true;
//            }

//            /// <summary>
//            /// 取消创建这个地图块;
//            /// </summary>
//            public static bool Remove(RectCoord chunkCoord)
//            {
//                Request request;
//                if (requested.TryGetValue(chunkCoord, out request))
//                {
//                    requested.Remove(chunkCoord);
//                    request.RequesteRemove();
//                    request.Destroy();
//                    return true;
//                }
//                return false;
//            }

//            /// <summary>
//            /// 清除所有的请求;
//            /// </summary>
//            public static void ClearRequested()
//            {
//                requested.Clear();
//            }



//            Request(RectCoord chunkCoord, IDictionary<CubicHexCoord, MapNode> data, Action<Request> callback)
//            {
//                this.ChunkCoord = chunkCoord;
//                this.Data = data;
//                this.callback = callback;
//            }


//            Action<Request> callback;
//            public RectCoord ChunkCoord { get; private set; }
//            public TerrainTexPack Textures { get; private set; }
//            public IDictionary<CubicHexCoord, MapNode> Data { get; private set; }


//            void RequesteAdd()
//            {
//                OBaker.Requested.AddLast(this);
//            }

//            void RequesteRemove()
//            {
//                OBaker.Requested.Remove(this);
//            }

//            void IBakeRequest.OnComplete(TerrainTexPack textures)
//            {
//                this.Textures = textures;

//                if (!requested.ContainsKey(ChunkCoord))
//                    Debug.LogError("请求已经不存在,但是还是要求创建");

//                if (Textures != null)
//                {
//                    callback(this);
//                    requested.Remove(ChunkCoord);
//                    Clear();
//                }
//            }

//            void IBakeRequest.OnError(Exception ex)
//            {
//                throw ex;
//            }

//            void Clear()
//            {
//                Textures = null;
//                callback = null;
//            }

//            void Destroy()
//            {
//                if (Textures != null)
//                {
//                    Textures.Destroy();
//                    Textures = null;
//                }
//            }

//            public override bool Equals(object obj)
//            {
//                Request _obj = obj as Request;

//                if (obj == null)
//                    return false;

//                return Equals(_obj);
//            }

//            public bool Equals(Request other)
//            {
//                return
//                    other != null &&
//                    other.ChunkCoord == ChunkCoord &&
//                    other.Data == Data;
//            }

//            public override int GetHashCode()
//            {
//                return ChunkCoord.GetHashCode() ^ Data.GetHashCode();
//            }

//        }

//    }

//}
