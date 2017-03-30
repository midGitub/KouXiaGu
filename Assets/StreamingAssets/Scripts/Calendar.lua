

-- 实现接口 ICalendar

		-- /// <summary>
        -- /// 这一个月存在的天数; 0 ~ max;
        -- /// </summary>
        -- int GetDaysInMonth(int year, int month);

        -- /// <summary>
        -- /// 这一年存在的月数; 1 ~ max;
        -- /// </summary>
        -- int GetMonthsInYear(int year);

        -- /// <summary>
        -- /// 这个月是否为闰月?
        -- /// </summary>
        -- bool IsLeapMonth(int year, int month);

        -- /// <summary>
        -- /// 这年是否为闰年?
        -- /// </summary>
        -- bool IsLeapYear(int year);

        -- /// <summary>
        -- /// 获取到枚举类型的月份表示;
        -- /// </summary>
        -- MonthType GetMonthType(int year, int month, out bool isLeapMonth);
        
--年月日时分秒 初始值都为 0;

		
Calendar = {
	New = function()
		i = {
			GetDaysInMonth = _GetDaysInMonth,
			GetMonthsInYear = _GetMonthsInYear,
			IsLeapMonth = _IsLeapMonth,
			IsLeapYear = _IsLeapYear,
			GetMonthType = _GetMonthType,
		}
		return setmetatable(i, {__index = self})
	end
}

--这一个月存在的天数; 0 ~ max;
_GetDaysInMonth = function(self, year, month)
	return 30
end

--这一年存在的月数; 1 ~ max;
_GetMonthsInYear = function(self, year)
	return 12
end

--这个月是否为闰月?
_IsLeapMonth = function (self, year, month)
	return false
end

--这年是否为闰年?
_IsLeapYear = function(self, year)
	return false
end


local MonthType = CS.KouXiaGu.World.MonthType

--数值对应的月份;
local MonthArray = {
	MonthType.January,
	MonthType.February,
	MonthType.March,
	MonthType.April,
	MonthType.May,
	MonthType.June,
	MonthType.July,
	MonthType.August,
	MonthType.September,
	MonthType.October,
	MonthType.November,
	MonthType.December,
}

--获取到枚举类型的月份表示;
_GetMonthType = function(self, year, month)
	month = month + 1
	print(month)
	return MonthArray[month], false
end