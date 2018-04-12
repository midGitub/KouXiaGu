using JiongXiaGu.Unity.GameConsoles;
using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.UI.Cursors;
using System;
using System.Collections.Generic;
using System.IO;

namespace JiongXiaGu.Unity.RunTime
{


    [ConsoleMethodClass]
    public static class GameCursor
    {
        private const string CursorFileExtension = ".zip";
        private static readonly IDictionary<string, ICustomCursor> cursors = new Dictionary<string, ICustomCursor>();
        private static StaticCursorFactroy staticCursorFactroy;
        private static AnimatedCursorFactroy animatedCursorFactroy;
        public static string CursorsDirectory => Path.Combine(Resource.ConfigDirectory, "Cursors");

        public static void Initialize()
        {
            staticCursorFactroy = new StaticCursorFactroy();
            animatedCursorFactroy = new AnimatedCursorFactroy();

            foreach (var item in Search(CursorsDirectory))
            {
                string name = item.Key.ToLower();
                cursors.Add(name, item.Value);
            }

            staticCursorFactroy = null;
            animatedCursorFactroy = null;
        }

        public static IEnumerable<KeyValuePair<string, ICustomCursor>> Search(string directory)
        {
            const string searchPattern = "*" + CursorFileExtension;
            var filePaths = Directory.EnumerateFiles(directory, searchPattern, SearchOption.TopDirectoryOnly);

            foreach (var filePath in filePaths)
            {
                ICustomCursor customCursor;
                if (TryReadFromZip(filePath, out customCursor))
                {
                    string name = Path.GetFileNameWithoutExtension(filePath);
                    yield return new KeyValuePair<string, ICustomCursor>(name, customCursor);
                }
            }

            var directoryPaths = Directory.EnumerateDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var directoryPath in directoryPaths)
            {
                ICustomCursor customCursor;
                if (TryReadFromDirectory(directoryPath, out customCursor))
                {
                    string name = Path.GetFileNameWithoutExtension(directoryPath);
                    yield return new KeyValuePair<string, ICustomCursor>(name, customCursor);
                }
            }
        }

        /// <summary>
        /// 尝试读取到光标文件;
        /// </summary>
        private static bool TryReadFromZip(string zipFilePath, out ICustomCursor cursor)
        {
            try
            {
                using (var content = new SharpZipLibContent(zipFilePath))
                {
                    return TryRead(content, out cursor);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex);
                cursor = null;
                return false;
            }
        }

        private static bool TryReadFromDirectory(string directory, out ICustomCursor cursor)
        {
            try
            {
                using (var content = new DirectoryContent(directory))
                {
                    return TryRead(content, out cursor);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex);
                cursor = null;
                return false;
            }
        }

        private static bool TryRead(IReadOnlyContent content, out ICustomCursor cursor)
        {
            try
            {
                cursor = staticCursorFactroy.Read(content);
                return true;
            }
            catch
            {
                try
                {
                    cursor = animatedCursorFactroy.Read(content);
                    return true;
                }
                catch
                {
                    cursor = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// 设置光标;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public static IDisposable SetCursor(string name)
        {
            name = name.ToLower();
            ICustomCursor cursor = cursors[name];
            return WindowCursor.SetCursor(cursor);
        }

        /// <summary>
        /// 设置光标;
        /// </summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public static IDisposable SetCursor(CursorType cursor)
        {
            return SetCursor(cursor.ToString());
        }

        /// <summary>
        /// 设置光标;
        /// </summary>
        public static IDisposable SetCursor(ICustomCursor cursor)
        {
            if (cursor == null)
                throw new ArgumentNullException(nameof(cursor));

            return WindowCursor.SetCursor(cursor);
        }

        [ConsoleMethod(nameof(CursorNames), Message = "显示所有已经定义的光标名称")]
        public static void CursorNames()
        {
            string message = string.Join(", ", cursors.Keys);
            GameConsole.Write(string.Format("已经定义的光标总数:{0}, 名称:{1}", cursors.Count, message));
        }

        private static IDisposable showCursorCanceler;

        [ConsoleMethod(nameof(ShowCursor), Message = "展示指定光标")]
        public static void ShowCursor(string name)
        {
            if (showCursorCanceler == null)
            {
                showCursorCanceler.Dispose();
                showCursorCanceler = null;
            }

            ICustomCursor customCursor;
            if (cursors.TryGetValue(name, out customCursor))
            {
                showCursorCanceler = SetCursor(customCursor);
                GameConsole.WriteSuccessful(string.Format("设置光标为[{0}]成功,使用[{1}]方法取消展示;", name, nameof(ResetCursor)));
            }
            else
            {
                GameConsole.WriteWarning(string.Format("未找到光标[{0}];", name));
            }
        }

        [ConsoleMethod(nameof(ResetCursor), Message = "重置展示光标内容")]
        public static void ResetCursor()
        {
            if (showCursorCanceler == null)
            {
                showCursorCanceler.Dispose();
                showCursorCanceler = null;
            }
            GameConsole.WriteSuccessful("取消光标展示;");
        }
    }
}
