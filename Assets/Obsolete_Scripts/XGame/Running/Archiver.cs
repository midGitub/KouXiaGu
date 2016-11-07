using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace XGame.Running
{

    /// <summary>
    /// 存档管理;
    /// </summary>
    [DisallowMultipleComponent]
    public class Archiver : MonoBehaviour
    {

        [Header("存档信息;")]

        /// <summary>
        /// 保存到的存档位置 Application.persistentDataPath + savesPath;
        /// </summary>
        [SerializeField]
        private string savesPath;

        /// <summary>
        /// 存档信息文件名;
        /// </summary>
        [SerializeField]
        private string saveInfoPath;

        /// <summary>
        /// 存档数据文件名;
        /// </summary>
        [SerializeField]
        private string saveDataPath;

        /// <summary>
        /// 存档版本;
        /// </summary>
        [SerializeField]
        private uint version;

        /// <summary>
        /// 获取到存档存放的位置;
        /// </summary>
        public string SavesPath
        {
            get { return Path.Combine(Application.persistentDataPath, savesPath); }
        }

        //protected override Archiver This { get { return this; } }

        /// <summary>
        /// 获取到一个随机的存档文件夹路径;
        /// </summary>
        /// <returns></returns>
        public string GetRandomSavePath()
        {
            string path = Path.Combine(SavesPath, Path.GetRandomFileName());
            while (Directory.Exists(path))
            {
                path += Path.GetRandomFileName();
            }
            return path;
        }

        /// <summary>
        /// 获取到保存信息文件的文件目录;
        /// </summary>
        /// <param name="savePath">包括存档目录的完整路径;</param>
        /// <returns>保存信息文件的文件目录;</returns>
        public string GetInfoFilePath(string savePath)
        {
            return Path.Combine(savePath, saveInfoPath);
        }

        /// <summary>
        /// 获取到保存数据文件的文件目录;
        /// </summary>
        /// <param name="savePath">包括存档目录的完整路径;</param>
        /// <returns>保存数据文件的文件目录;</returns>
        public string GetDataFilePath(string savePath)
        {
            return Path.Combine(savePath, saveDataPath);
        }

        /// <summary>
        /// 保存存档信息文件;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="info">存档信息类;</param>
        internal void Save(string savePath, GameSaveInfo info)
        {
            string infoFilePath = GetInfoFilePath(savePath);
            info.Version = version;
            SerializeHelper.Serialize_ProtoBuf(infoFilePath, info);
        }

        /// <summary>
        /// 保存存档数据文件;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="data">存档数据类;</param>
        internal void Save(string savePath, GameSaveData data)
        {
            string dataFilePath = GetDataFilePath(savePath);
            SerializeHelper.Serialize_ProtoBuf(dataFilePath, data);
        }

        /// <summary>
        /// 保存存档到此目录;若已经存在存档文件,则覆盖;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="info">存档信息类;</param>
        /// <param name="data">存档数据类;</param>
        public void Save(string savePath, GameSaveInfo info, GameSaveData data)
        {
            Save(savePath, info);
            Save(savePath, data);
        }

        /// <summary>
        /// 保存新存档;
        /// </summary>
        /// <param name="info">存档信息类;</param>
        /// <param name="data">存档数据类;</param>
        public void Save(GameSaveInfo info, GameSaveData data)
        {
            string savePath = GetRandomSavePath();

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            Save(savePath, info, data);
        }

        /// <summary>
        /// 读取信息文件;并且初始化路径信息;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="info">返回存档信息类;</param>
        internal void Load(string savePath, out GameSaveInfo info)
        {
            string infoFilePath = GetInfoFilePath(savePath);
            info = SerializeHelper.Deserialize_ProtoBuf<GameSaveInfo>(infoFilePath);
        }

        /// <summary>
        /// 读取数据文件;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="data">返回存档数据类;</param>
        internal void Load(string savePath, out GameSaveData data)
        {
            string dataFilePath = GetDataFilePath(savePath);
            data = SerializeHelper.Deserialize_ProtoBuf<GameSaveData>(dataFilePath);
        }

        /// <summary>
        /// 读取游戏;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        /// <param name="info">返回存档信息类;</param>
        /// <param name="data">返回存档数据类;</param>
        public void Load(string savePath, out GameSaveInfo info, out GameSaveData data)
        {
            Load(savePath, out info);
            Load(savePath, out data);
        }

        /// <summary>
        /// 删除此存档;
        /// </summary>
        /// <param name="savePath">已经存在的存档路径,完整路径;</param>
        public void Delete(string savePath)
        {
            Directory.Delete(savePath, true);
        }

        /// <summary>
        /// 获取到所有存档文件目录和存档信息;
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, GameSaveInfo>> GetSavesInfo()
        {
            GameSaveInfo info;
            IEnumerable<string> SaveDirectories = Directory.GetDirectories(SavesPath);
            foreach (string savePath in SaveDirectories)
            {
                Load(savePath, out info);
                yield return new KeyValuePair<string, GameSaveInfo>(savePath, info);
            }
        }

        /// <summary>
        /// 对存档文件按日期进行降序排列;
        /// </summary>
        /// <param name="saveFileInfos"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, GameSaveInfo>> DescendingOrderOfTime(
            IEnumerable<KeyValuePair<string, GameSaveInfo>> saveFileInfos)
        {
            return saveFileInfos.OrderBy(info => info.Value.SavingTime_Ticks);
        }

        /// <summary>
        /// 对存档文件按日期进行升序排列;
        /// </summary>
        /// <param name="saveFileInfos"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, GameSaveInfo>> AscendingOrderOfTime(
            IEnumerable<KeyValuePair<string, GameSaveInfo>> saveFileInfos)
        {
            return saveFileInfos.OrderByDescending(info => info.Value.SavingTime_Ticks);
        }

    }

}
