using System;
using System.IO;
using System.Threading;
using UnityEngine;

namespace KouXiaGu.Map
{

    ///// <summary>
    ///// 动态地图保存和读取;
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //[Obsolete]
    //public class DynamicMapIO<T> : IMapBlockIO<DynamicMapPaging<T>, T>
    //{

    //    public DynamicMapIO(MapBlockInfo pagingInfo)
    //    {
    //        this.pagingInfo = pagingInfo;
    //    }

    //    private MapBlockInfo pagingInfo;

    //    /// <summary>
    //    /// 获取到地图块保存到的文件路径
    //    /// </summary>
    //    public string GetMapPagingFilePath(ShortVector2 address)
    //    {
    //        string mapPagingName = pagingInfo.GetMapPagingName(address);
    //        return Path.Combine(pagingInfo.DataDirectoryPath, mapPagingName);
    //    }

    //    /// <summary>
    //    /// 异步保存这个地图块文件;
    //    /// </summary>
    //    public void SaveAsyn(DynamicMapPaging<T> mapPaging)
    //    {
    //        WaitCallback waitCallback = _ => Save(mapPaging);
    //        ThreadPool.QueueUserWorkItem(waitCallback);
    //    }

    //    /// <summary>
    //    /// 保存这个地图块,并且返回保存到的地址;
    //    /// </summary>
    //    public string Save(DynamicMapPaging<T> mapPaging)
    //    {
    //        string mapPagingFilePath = GetMapPagingFilePath(mapPaging.Address);
    //        SerializeHelper.Serialize_ProtoBuf(mapPagingFilePath, mapPaging);

    //        Debug.Log("已更改地图块 :" + mapPaging.ToString() +
    //            "\n保存到地址:" + mapPagingFilePath);

    //        return mapPagingFilePath;
    //    }

    //    /// <summary>
    //    /// 从文件读取到地图分页.若无法获取返回异常;
    //    /// FileNotFoundException : 不存在此地图块;
    //    /// </summary>
    //    public DynamicMapPaging<T> Load(ShortVector2 address)
    //    {
    //        string mapPagingFilePath = GetMapPagingFilePath(address);
    //        return SerializeHelper.Deserialize_ProtoBuf<DynamicMapPaging<T>>(mapPagingFilePath);
    //    }

    //    /// <summary>
    //    /// 尝试获取到这个地图块,若不存在则返回false,否则返回true;
    //    /// </summary>
    //    public bool TryLoad(ShortVector2 address, out DynamicMapPaging<T> mapPaging)
    //    {
    //        string mapPagingFilePath = GetMapPagingFilePath(address);
    //        try
    //        {
    //            mapPaging = SerializeHelper.Deserialize_ProtoBuf<DynamicMapPaging<T>>(mapPagingFilePath);
    //            return true;
    //        }
    //        catch (FileNotFoundException) { }
    //        mapPaging = default(DynamicMapPaging<T>);
    //        return false;
    //    }

    //    /// <summary>
    //    /// 删除地图块文件;
    //    /// </summary>
    //    public void DeleteMapPagingFile(DynamicMapPaging<T> mapPaging)
    //    {
    //        string mapPagingFilePath = GetMapPagingFilePath(mapPaging.Address);
    //        DeleteMapPagingFile(mapPagingFilePath);
    //    }

    //    /// <summary>
    //    /// 删除地图块文件;
    //    /// </summary>
    //    public void Delete(ShortVector2 address)
    //    {
    //        string mapPagingFilePath = GetMapPagingFilePath(address);
    //        DeleteMapPagingFile(mapPagingFilePath);
    //    }

    //    /// <summary>
    //    /// 删除地图块文件;
    //    /// </summary>
    //    public void DeleteMapPagingFile(string mapPagingFilePath)
    //    {
    //        if (DeleteFile(mapPagingFilePath))
    //        {
    //            Debug.Log("删除了地图块,地址:" + mapPagingFilePath);
    //        }
    //    }

    //    /// <summary>
    //    /// 移除文件,若移除成功返回true,否则返回false;
    //    /// 不存在这个文件也返回false;
    //    /// </summary>
    //    private bool DeleteFile(string mapPagingFilePath)
    //    {
    //        if (!File.Exists(mapPagingFilePath))
    //            return false;
    //        try
    //        {
    //            File.Delete(mapPagingFilePath);
    //            return true;
    //        }
    //        catch (DirectoryNotFoundException)
    //        {
    //            return false;
    //        }
    //    }

    //}

}
