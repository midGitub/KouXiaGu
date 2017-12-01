using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JiongXiaGu.Unity.Resources
{


    public static class ContentReadTool
    {

        /// <summary>
        /// 获取到对应资源,若不存在则返回null;
        /// 允许在任何线程调用,
        /// 若为AssetBundle,在Unity线程则直接读取,在其它线程则阻塞此线程,等待Unity线程执行完毕;
        /// 若为File,在Unity线程则直接读取,在其它线程则直接读取;
        /// </summary>
        public static T GetAsset<T>(this LoadableContent content, AssetInfo assetInfo)
            where T :class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取到对应资源,若未能获取到则返回失败的Task;
        /// 允许在任何线程调用,
        /// 若为AssetBundle,在Unity线程则为同步的读取到,在其它线程则为异步操作;
        /// 若为File,都为异步操作;
        /// </summary>
        public static Task<T> GetAssetAsync<T>(this LoadableContent content, AssetInfo assetInfo)
        {
            throw new NotImplementedException();
        }


        public static Texture ReadAsTexture(this AssetInfo assetInfo, LoadableContent content)
        {
            throw new NotImplementedException();
        }

        private static Texture2D InternalReadFileAsTexture(AssetInfo assetInfo, LoadableContent content)
        {
            ILoadableEntry entry = content.GetEntry(assetInfo.Name);
            var stream = content.GetInputStream(entry);
            byte[] imageData = new byte[stream.Length];
            stream.Read(imageData, 0, (int)stream.Length);
            Texture2D texture2D = new Texture2D(2, 2);
            texture2D.LoadImage(imageData);
            return texture2D;
        }

        //private static Texture2D InternalReadAssetBundleAsTexture(AssetInfo assetInfo, LoadableContent content)
        //{
            
        //}
    }
}
