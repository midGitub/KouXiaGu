using System.IO;
using UnityEngine;
using XLua;

namespace JiongXiaGu.Lua
{

    /// <summary>
    /// 全局Lua脚本控制;
    /// </summary>
    [DisallowMultipleComponent]
    public class LuaManager : MonoBehaviour
    {
        static readonly LuaEnv luaEnv = new LuaEnv();

        public static LuaEnv Luaenv
        {
            get { return luaEnv; }
        }

        internal static float lastGCTime = 0;
        internal const float GCInterval = 1; //second 

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
}
