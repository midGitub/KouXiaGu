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

        [ContextMenu("Test0")]
        private void Test0()
        {
            XmlSerializer<SerializableDictionary<RectCoord, ArchiveMapNode>> xmlSerializer = new XmlSerializer<SerializableDictionary<RectCoord, ArchiveMapNode>>();
            SerializableDictionary<RectCoord, ArchiveMapNode> value = new SerializableDictionary<RectCoord, ArchiveMapNode>()
            {
                { new RectCoord(0, 0), new ArchiveMapNode() },
                { new RectCoord(0, 1), new ArchiveMapNode() },
                {
                    new RectCoord(0, 2), new ArchiveMapNode(new MapNode()
                        {
                            Landform = new NodeLandformInfo()
                            {
                                Angle = 11,
                                TypeID = "123123",
                            },
                        })
                },
            };

            using (var stream = new FileStream(@"123.xml", FileMode.Create, FileAccess.ReadWrite))
            {
                xmlSerializer.Serialize(stream, value);
            }

            Debug.Log("OK!");
        }

        public MeshRenderer meshRenderer;

        [ContextMenu("Test1")]
        private async void Test1()
        {
            //LandformDescription description = new LandformDescription()
            //{
            //    HeightTex = new AssetInfo("terrain", "HeightMap_85"),
            //    HeightBlendTex = new AssetInfo("terrain", "HeightMap_Blend"),
            //    DiffuseTex = new AssetInfo("terrain", "SoilCracked2"),
            //    DiffuseBlendTex = new AssetInfo("terrain", "HeightMap_Blend"),
            //};

            //LandformRes res = await LandformResCreater.CreateAsync(LoadableResource.Core, description, default(CancellationToken));
            //meshRenderer.material.mainTexture = res.DiffuseTex;
        }

        [ContextMenu("Test2")]
        private async void Test2()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            for (int i = 0; i < 50; i++)
            {
                int temp = i;
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                TaskHelper.Run(delegate ()
                {
                    Debug.Log(temp);
                    Thread.Sleep(100);
                }, source.Token, UnityUpdateTaskScheduler.TaskScheduler).ContinueWith(task => Debug.Log(temp + "ContinueWith"), source.Token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            }
            await Task.Delay(1000);
            source.Cancel();
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
