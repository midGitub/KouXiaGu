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

        public static void Initialize()
        {
            staticCursorFactroy = new StaticCursorFactroy();
            animatedCursorFactroy = new AnimatedCursorFactroy();
            string cursorsDirectory = Path.Combine(Resource.ConfigDirectory, "Cursors");

            foreach (var item in Search(cursorsDirectory))
            {
                cursors.Add(item.Key, item.Value);
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
                Content content;
                if (TryRead(filePath, out content))
                {
                    ICustomCursor customCursor;
                    if (TryRead(content, out customCursor))
                    {
                        string name = Path.GetFileNameWithoutExtension(filePath);
                        yield return new KeyValuePair<string, ICustomCursor>(name, customCursor);
                    }
                }
            }
        }

        /// <summary>
        /// 尝试读取到压缩文件,若文件不属于ZIP压缩或者不存在则返回false;
        /// </summary>
        public static bool TryRead(string zipFilePath, out Content content)
        {
            try
            {
                content = new SharpZipLibContent(zipFilePath);
                return true;
            }
            catch (Exception)
            {
                content = null;
                return false;
            }
        }

        /// <summary>
        /// 尝试读取到光标文件;
        /// </summary>
        public static bool TryRead(Content content, out ICustomCursor cursor)
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

        public static IDisposable SetCursor(string name)
        {
            ICustomCursor cursor = cursors[name];
            return WindowCursor.SetCursor(cursor);
        }

        public static IDisposable SetCursor(CursorType cursor)
        {
            return SetCursor(cursor.ToString());
        }

        [ConsoleMethod(nameof(CursorNames), Message = "显示所有光标信息")]
        public static void CursorNames()
        {

        }
    }
}
