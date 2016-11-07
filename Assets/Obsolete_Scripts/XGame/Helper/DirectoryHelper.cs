using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XGame
{

    /// <summary>
    /// 文件,文档读取工具;
    /// </summary>
    public static class DirectoryHelper
    {

        /// <summary>
        /// 转换到Unity类WWW使用的链接;
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetWWWurl(string fullPath)
        {
            return "file://" + fullPath;
        }

        /// <summary>
        /// 转换到Unity类WWW使用的链接;
        /// </summary>
        /// <param name="fullPath"></param>
        public static void GetWWWurl(ref string fullPath)
        {
            fullPath = "file://" + fullPath;
        }

        /// <summary>
        /// 读取Sprite贴图;
        /// </summary>
        /// <param name="path">完整文件路径;F:/My_Code/Unity5/1.png</param>
        /// <returns></returns>
        public static Sprite LoadSprite(string fullPath)
        {
            GetWWWurl(ref fullPath);
            WWW www = new WWW(fullPath);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }

        /// <summary>
        /// 读取Sprite贴图;
        /// </summary>
        /// <param name="fullPath">完整文件路径;F:/My_Code/Unity5/1.png</param>
        /// <param name="rect"></param>
        /// <param name="pivot"></param>
        /// <returns></returns>
        public static Sprite LoadSprite(string fullPath, Vector2 pivot)
        {
            GetWWWurl(ref fullPath);
            WWW www = new WWW(fullPath);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), pivot);
            return sprite;
        }

        /// <summary>
        /// 获取到格式为 Texture 的贴图;
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static Texture LoadTexture(string fullPath)
        {
            GetWWWurl(ref fullPath);
            WWW www = new WWW(fullPath);
            Texture2D texture2D = www.texture;
            return texture2D;
        }

    }

}
