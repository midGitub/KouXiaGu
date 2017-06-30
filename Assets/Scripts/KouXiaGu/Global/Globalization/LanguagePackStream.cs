using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.Globalization
{

    public class LanguagePackStream : LanguagePack
    {
        public LanguagePackStream(string name, string locName, Stream stream) : base(name, locName)
        {
            Stream = stream;
        }

        public Stream Stream { get; private set; }

        /// <summary>
        /// 结束流;
        /// </summary>
        public void Close()
        {
            Stream.Dispose();
            Stream = null;
        }

        /// <summary>
        /// 关闭所有;
        /// </summary>
        public static void CloseAll(IEnumerable<LanguagePackStream> streams)
        {
            foreach (var stream in streams)
            {
                try
                {
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                }
            }
        }

        /// <summary>
        /// 关闭所有,并且转换;
        /// </summary>
        public static List<LanguagePack> CloseAndConvertAll(IEnumerable<LanguagePackStream> streams)
        {
            List<LanguagePack> packs = new List<LanguagePack>();
            foreach (var stream in streams)
            {
                try
                {
                    stream.Close();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                }
                finally
                {
                    packs.Add(stream);
                }
            }
            return packs;
        }
    }
}
