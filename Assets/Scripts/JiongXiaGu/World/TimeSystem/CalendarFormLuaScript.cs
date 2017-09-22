using JiongXiaGu.Lua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;

namespace JiongXiaGu.World.TimeSystem
{

    class CalendarFormLuaScript
    {
        [CSharpCallLua]
        public delegate ICalendar CalendarReader();
        public const string luaScriptName = "Calendar.New";

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize()
        {
            LuaEnv luaenv = LuaManager.Luaenv;
            CalendarReader creater = luaenv.Global.GetInPath<CalendarReader>(luaScriptName);
            if (creater != null)
            {
                ICalendar calendar = creater();
                if (calendar != null)
                {
                    WorldDateTime.SetCalendar(calendar);
                }
            }
        }
    }
}
