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

        [ContextMenu("Test")]
        private void Test()
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            using (var stream = new FileStream(@"1.zip", FileMode.Open, FileAccess.Read))
            {
                var md5 = mD5CryptoServiceProvider.ComputeHash(stream);
                string md5Str = string.Join(string.Empty, md5);
                Debug.Log("0 : " + md5Str);

                ZipFile zipFile = new ZipFile(stream);

                stream.Seek(0, SeekOrigin.Begin);
                md5 = mD5CryptoServiceProvider.ComputeHash(stream);
                md5Str = string.Join(string.Empty, md5);
                Debug.Log("1 : " + md5Str);
            }
        }

        public MeshRenderer meshRenderer;

        private async void Start()
        {
            LandformDescription description = new LandformDescription()
            {
                HeightTex = new AssetInfo("HeightMap_85"),
                HeightBlendTex = new AssetInfo("HeightMap_Blend"),
                DiffuseTex = new AssetInfo(LoadMode.File, @"Terrain\Landforms\SoilCracked2.jpg"),
                DiffuseBlendTex = new AssetInfo("HeightMap_Blend"),
            };

            Task<LandformRes> infoTask = null;
            await Task.Run(delegate ()
            {
                infoTask = LandformRes.CreateAsync(LoadableResource.Core, description);
            });

            await infoTask;
            meshRenderer.material.mainTexture = infoTask.Result.DiffuseTex;

            //var task = Resource.Core.ReadAsTextureAsync(new AssetInfo("HeightMap_Blend"));
            //await task;
            //Debug.Log(task.Result.texelSize);
            //meshRenderer.material.mainTexture = task.Result;
        }

        //private async Task Start()
        //{
        //    await TaskHelper.Run(() => Debug.Log("0 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //    await Task.Delay(1000);
        //    await TaskHelper.Run(() => Debug.Log("1 : " + XiaGu.IsUnityThread + ",Time:" + Time.time), XiaGu.UnityTaskScheduler);
        //}
    }
}
