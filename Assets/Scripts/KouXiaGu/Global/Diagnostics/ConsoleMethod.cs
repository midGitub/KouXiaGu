using System;

namespace KouXiaGu.Diagnostics
{

    /// <summary>
    /// 委托类型的条目;
    /// </summary>
    public class ConsoleMethod : IConsoleMethod
    {
        public ConsoleMethod(string key, Action<string[]> action, string message, bool isDeveloperMethod, params string[] parameterTypes)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            Action = action;
            ParameterNumber = parameterTypes.Length;
            Message = XiaGuConsole.ConvertMassage(key, message, parameterTypes);
            IsDeveloperMethod = isDeveloperMethod;
        }

        public int ParameterNumber { get; private set; }
        public Action<string[]> Action { get; private set; }
        public string Message { get; private set; }
        public bool IsDeveloperMethod { get; private set; }

        public void Operate(string[] parameters)
        {
            Action(parameters);
        }
    }
}
