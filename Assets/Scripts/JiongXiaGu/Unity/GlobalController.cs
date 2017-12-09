using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Cryptography;
using JiongXiaGu.Unity.RectTerrain;

namespace JiongXiaGu.Unity
{

    /// <summary>
    /// 在程序一开始就存在的物体,保持该物体不随场景切换销毁;
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class GlobalController :MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        [ContextMenu("Test2")]
        private async void Test2()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            Action action = delegate ()
            {
                Debug.Log(string.Format("ThreadId : [{0}]", Thread.CurrentThread.ManagedThreadId));
            };

            action.Invoke();
            Task task0 = Task.Run(action, source.Token);
            await task0;
            action.Invoke();
            await task0.ContinueWith(task => action.Invoke(), source.Token);
            await task0.ContinueWith(task => action.Invoke(), source.Token);
            await task0.ContinueWith(task => action.Invoke(), source.Token);
            await task0.ContinueWith(task => action.Invoke(), source.Token);
        }

        [ContextMenu("Test1")]
        private async void Test1()
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
                }, source.Token, UnityTaskScheduler.TaskScheduler).ContinueWith(task => Debug.Log(temp + "ContinueWith"), source.Token);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            }
            await Task.Delay(1000);
            source.Cancel();
        }


        [ContextMenu("Test")]
        private void Test()
        {
            //object asyncLock = new object();
            //Monitor.Enter(asyncLock);

            //Debug.Log(string.Format("[{0}]IsEntered:{1}!", Thread.CurrentThread.ManagedThreadId, Monitor.IsEntered(asyncLock)));

            //await Task.Run(delegate ()
            //{
            //    Debug.Log(string.Format("[{0}]IsEntered:{1}!", Thread.CurrentThread.ManagedThreadId, Monitor.IsEntered(asyncLock)));
            //});

            //Debug.Log(string.Format("[{0}]IsEntered:{1}!", Thread.CurrentThread.ManagedThreadId, Monitor.IsEntered(asyncLock)));
            //Monitor.Exit(asyncLock);

            //await Task.Run(delegate ()
            //{
            //    lock (asyncLock)
            //    {
            //        Debug.Log(string.Format("[{0}]IsEntered:{1}!", Thread.CurrentThread.ManagedThreadId, Monitor.IsEntered(asyncLock)));
            //    }
            //});
        }

        public MeshRenderer meshRenderer;

        private async void Test0()
        {
            LandformDescription description = new LandformDescription()
            {
                HeightTex = new AssetInfo("terrain", "HeightMap_85"),
                HeightBlendTex = new AssetInfo("terrain", "HeightMap_Blend"),
                DiffuseTex = new AssetInfo(@"Terrain\Landforms\SoilCracked2.jpg"),
                DiffuseBlendTex = new AssetInfo("terrain", "HeightMap_Blend"),
            };

            Task<LandformRes> infoTask = null;
            await Task.Run(delegate ()
            {
                infoTask = LandformResPool.Create(LoadableResource.Core, description, default(CancellationToken));
            });

            await infoTask;
            meshRenderer.material.mainTexture = infoTask.Result.DiffuseTex;
        }

        //private async Task Start()
        //{
        //    await TaskHelper.Run(() => Debug.Log("0 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //    await Task.Delay(1000);
        //    await TaskHelper.Run(() => Debug.Log("1 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //}
    }
}
