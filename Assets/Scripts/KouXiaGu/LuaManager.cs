using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using XLua;

namespace KouXiaGu
{

    /// <summary>
    /// 全局Lua脚本控制;
    /// </summary>
    [DisallowMultipleComponent]
    public class LuaManager : MonoBehaviour
    {

        static readonly LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!

        public static LuaEnv Luaenv
        {
            get { return luaEnv; }
        }

        internal static float lastGCTime = 0;
        internal const float GCInterval = 1; //1 second 

        public static string SpritesPath
        {
            get { return Path.Combine(Application.streamingAssetsPath, "Scripts"); }
        }

        public static string MianSpritePath
        {
            get { return Path.Combine(SpritesPath, "Main.lua"); }
        }

        void Awake()
        {
            Luaenv.DoFile(MianSpritePath);
        }

        void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                lastGCTime = Time.time;
            }
        }

    }


    public static class LuaExtensions
    {

        public static object[] DoFile(this LuaEnv luaEnv, string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                return luaEnv.DoString(file.ReadToEnd());
            }
        }

        public static LuaFunction LoadFile(this LuaEnv luaEnv, string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                return luaEnv.LoadString(file.ReadToEnd());
            }
        }

        public static T LoadFile<T>(this LuaEnv luaEnv, string path)
        {
            using (StreamReader file = new StreamReader(path))
            {
                return luaEnv.LoadString<T>(file.ReadToEnd());
            }
        }

    }

}
