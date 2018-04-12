using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using System.Collections.Generic;
using JiongXiaGu.Unity.RectTerrain;
using System.Xml.Serialization;
using JiongXiaGu.Unity.Maps;
using JiongXiaGu.Grids;
using JiongXiaGu.Collections;
using JiongXiaGu.Unity.UI.Cursors;
using JiongXiaGu.Unity.RunTime;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Writers;
using System.Text;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController : MonoBehaviour
    {
        private static readonly GlobalSingleton<GlobalController> singleton = new GlobalSingleton<GlobalController>();
        public static GlobalController Instance => singleton.GetInstance();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [SerializeField]
        private string filePath;
        [SerializeField]
        private AnimatedCursor animatedCursor;
        [SerializeField]
        private StaticCursor staticCursor;

        [ContextMenu("Test0")]
        private void Test0()
        {
            var animatedCursorFactroy = new AnimatedCursorFactroy();

            using (var content = new SharpZipLibContent(Path.Combine(GameCursor.CursorsDirectory, "Wait.zip")))
            {
                animatedCursor = animatedCursorFactroy.Read(content);
            }

            WindowCursor.SetCursor(animatedCursor);
        }

        [ContextMenu("Test1")]
        private void Test1()
        {
            var animatedCursorFactroy = new AnimatedCursorFactroy();
            var staticCursorFactroy = new StaticCursorFactroy();

            using (var content = SharpZipLibContent.CreateNew(Path.Combine(GameCursor.CursorsDirectory, "AnimatedCursor.zip")))
            {
                using (content.BeginUpdateAuto())
                {
                    animatedCursorFactroy.Write(content, animatedCursor);
                }
            }

            using (var content = SharpZipLibContent.CreateNew(Path.Combine(GameCursor.CursorsDirectory, "StaticCursor.zip")))
            {
                using (content.BeginUpdateAuto())
                {
                    staticCursorFactroy.Write(content, staticCursor);
                }
            }
        }

        [ContextMenu("Test2")]
        private void Test2()
        {
            string file = Path.Combine(Resource.ConfigDirectory, "1.zip");

            using (ZipArchive zipArchive = ZipArchive.Create())
            {
                zipArchive.AddEntry("11.txt", new MemoryStream(Encoding.UTF8.GetBytes("123123")));
                using (var stream = new FileStream(file, FileMode.Create))
                {
                    zipArchive.SaveTo(stream, CompressionType.Deflate);
                }
            }

            using (ZipArchive zipArchive = ZipArchive.Open(file))
            {

            }
        }

        [ContextMenu("Test3")]
        private void Test3()
        {
            ParallelOptions parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = -1,
            };

            Parallel.ForEach(GetValues(), parallelOptions, delegate (string value)
            {
                if (value == "2")
                {
                    throw new ArgumentException("错误2!");
                }
                if (value == "3")
                {
                    throw new ArgumentException("错误3!");
                }
                Debug.Log(string.Format("ThreadID : {0}, Value : {1}", Thread.CurrentThread.ManagedThreadId, value));
            });
        }

        private IEnumerable<string> GetValues()
        {
            yield return "1";
            yield return "2";
            yield return "3";
            yield return "4";
        }

        //private async Task Start()
        //{
        //    await TaskHelper.Run(() => Debug.Log("0 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //    await Task.Delay(1000);
        //    await TaskHelper.Run(() => Debug.Log("1 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //}
    }
}
