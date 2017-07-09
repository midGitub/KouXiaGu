using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;

namespace KouXiaGu.World.TimeSystem
{

    class CalendarFormLuaScript
    {
        [CSharpCallLua]
        public delegate ICalendar CalendarReader();
        public const string luaScriptName = "Calendar.New";

        /// <summary>
        /// 从Lua文件获取到日历信息;
        /// </summary>
        public static ICalendar Read()
        {
            const string errorString = "无法从Lua获取到日历信息;";

            LuaEnv luaenv = LuaManager.Luaenv;
            CalendarReader creater = luaenv.Global.GetInPath<CalendarReader>(luaScriptName);
            if (creater == null)
                throw new ArgumentException(errorString);

            ICalendar calendar = creater();
            if (calendar == null)
                throw new ArgumentException(errorString);

            return calendar;
        }
    }
}
