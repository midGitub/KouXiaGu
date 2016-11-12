using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu
{

    [DisallowMultipleComponent]
    public class Properties : MonoBehaviour
    {
        private Properties() { }

        [SerializeField]
        private CoreData coreData;
        [SerializeField]
        private ArchiveData archiveData;
        [SerializeField]
        private ModData modData;


        //[ContextMenu("显示所有存档")]
        //private void Test_Archive()
        //{
        //    string strLog = "存在的存档:";
        //    var items = archiveControl.GetSmallArchiveds();
        //    foreach (var item in items)
        //    {
        //        ArchivedHead smallArchived = item.ArchivedHead;

        //        strLog +=
        //            "\n名称:" + smallArchived.Name +
        //            "\n路径:" + item.ArchivedPath;
        //    }
        //    Debug.Log(strLog);
        //}

        //[ContextMenu("保存存档")]
        //private void Test_SaveRandomArchive()
        //{
        //    ArchivedGroup archived = archiveControl.GetNewArchived();
        //    archiveControl.SaveInDisk(archived);
        //}

    }

}
