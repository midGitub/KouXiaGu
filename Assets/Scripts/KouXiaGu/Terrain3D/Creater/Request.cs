using System;
using KouXiaGu.Collections;
using KouXiaGu.Grids;
using UnityEngine;

namespace KouXiaGu.Terrain3D
{


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
        /// 创建这个地形块,若已经在队列则返回false;
        /// </summary>
        /// <param name="callback">当完成所有进度时回调;</param>
        public static bool Create(RectCoord chunkCoord, Action<Request> callback)
        {
            if (callback == null)
                throw new ArgumentNullException();
            if (requested.ContainsKey(chunkCoord))
                return false;

            Request request = new Request(chunkCoord, callback);
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
        internal static void ClearRequested()
        {
            requested.Clear();
        }


        Request(RectCoord chunkCoord, Action<Request> callback)
        {
            this.ChunkCoord = chunkCoord;
            this.callback = callback;
        }


        Action<Request> callback;
        public RectCoord ChunkCoord { get; private set; }
        public TerrainTexPack Textures { get; private set; }
        public BuildingGroup Buildings { get; private set; }

        public MapData Data
        {
            get { return MapDataManager.ActiveData; }
        }


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
            this.Textures = textures;
            CreateChunk();
        }

        void IBuildRequest.OnComplete(BuildingGroup buildings)
        {
            this.Buildings = buildings;
            CreateChunk();
        }

        void CreateChunk()
        {
            if (!requested.ContainsKey(ChunkCoord))
                Debug.LogError("请求已经不存在,但是还是要求创建");

            if (Textures != null && Buildings != null)
            {
                callback(this);
                requested.Remove(ChunkCoord);
                Clear();
            }
        }

        void Clear()
        {
            Textures = null;
            Buildings = null;
            callback = null;
        }

        void Destroy()
        {
            if (Textures != null)
            {
                Textures.Destroy();
                Textures = null;
            }
            if (Buildings != null)
            {
                Buildings.Destroy();
                Buildings = null;
            }
        }

        public void OnError(Exception ex)
        {
            throw ex;
            //Debug.LogError(ex);
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
