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
