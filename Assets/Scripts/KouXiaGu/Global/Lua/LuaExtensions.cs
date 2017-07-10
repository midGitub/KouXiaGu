using System.IO;
using XLua;

namespace KouXiaGu.Lua
{

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
