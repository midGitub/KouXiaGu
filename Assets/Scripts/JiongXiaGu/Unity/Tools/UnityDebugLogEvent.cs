﻿using UnityEngine;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// Unity.Debug 事件;
    /// </summary>
    public struct UnityDebugLogEvent
    {
        public LogType LogType { get; set; }
        public UnityEngine.Object Context { get; set; }
        public string Message { get; set; }

        public UnityDebugLogEvent(LogType logType, UnityEngine.Object context, string message)
        {
            LogType = logType;
            Context = context;
            Message = message;
        }

        public UnityDebugLogEvent(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            LogType = logType;
            Context = context;
            Message = string.Format(format, args);
        }
    }
}
