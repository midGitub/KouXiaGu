using JiongXiaGu.Unity.Resources;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;

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
            string directory = "F:\\My_Code\\Unity5\\KouXiaGu\\NUnitTemp";
            FastZip fast = new FastZip();

            //string fileFilter = new PathFilter(string.Empty).ToString();
            //Debug.Log(fileFilter);
            fast.CreateZip(@"NUnitTemp.zip", directory, true, string.Empty);

            //using (ZipFile zipFile = ZipFile.Create(new FileStream(@"NUnitTemp.zip", FileMode.Create, FileAccess.ReadWrite)))
            //{
            //    zipFile.IsStreamOwner = true;

            //    zipFile.BeginUpdate();

            //    foreach (var path in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories))
            //    {
            //        zipFile.Add(path);
            //    }

            //    zipFile.CommitUpdate();
            //}

            using (ZipFile zipFile = new ZipFile(@"NUnitTemp.zip"))
            {
                foreach (ZipEntry zipEntry in zipFile)
                {
                    Debug.Log(zipEntry.Name);
                }
            }
        }
    }
}
