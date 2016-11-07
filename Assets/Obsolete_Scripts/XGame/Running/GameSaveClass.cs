using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;
using UnityEngine;
using XGame.Running.Map;

namespace XGame.Running
{

    /// <summary>
    /// 游戏存档文件;
    /// </summary>
    [ProtoContract]
    public sealed class GameSaveData
    {
        public GameSaveData() { }


        #region MapManager 4~6

        [ProtoMember(4)]
        public MapInfo MapInfo { get; set; }

        #endregion


        #region PrefabManager 7~9

        [ProtoMember(7)]
        public GameObjectState[] GameObjectStates { get; set; }

        #endregion


        #region GameTimer 21~30

        /// <summary>
        /// 上一次更新的时间;
        /// </summary>
        [ProtoMember(21)]
        public long LastTime_Ticks;

        #endregion

    }


    /// <summary>
    /// 存档信息;
    /// </summary>
    [ProtoContract]
    public class GameSaveInfo
    {
        public GameSaveInfo()
        {
            this.SavingTime_Ticks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 使用默认时间创建;
        /// </summary>
        /// <param name="name">存档名;</param>
        internal GameSaveInfo(string name) : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// 存档信息;
        /// </summary>
        /// <param name="name">存档名;</param>
        /// <param name="time">存档时间;</param>
        internal GameSaveInfo(string name, DateTime time)
        {
            this.Name = name;
            this.SavingTime_Ticks = time.Ticks;
        }

        #region 存档信息 1~10

        /// <summary>
        /// 存档名;
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }

        /// <summary>
        /// 存档保存时间;
        /// </summary>
        [ProtoMember(2)]
        public long SavingTime_Ticks;

        /// <summary>
        /// 存档版本;
        /// </summary>
        [ProtoMember(3)]
        public uint Version { get; set; }

        #endregion


        #region GameTime 11~20

        /// <summary>
        /// 游戏存档开始时间;
        /// </summary>
        [ProtoMember(11)]
        public long StartTime_Ticks;

        /// <summary>
        /// 游戏进行到的时间;
        /// </summary>
        [ProtoMember(12)]
        public long Time_Ticks;

        #endregion


        #region 资金 21~30

        /// <summary>
        /// 游戏持有资金;
        /// </summary>
        [ProtoMember(21)]
        public int Capital;

        #endregion


        #region MapController 31~40

        /// <summary>
        /// 地图信息;
        /// </summary>
        [ProtoMember(31)]
        public MapInfo mapInfo;

        #endregion

    }


    /// <summary>
    /// 保存为预制到的文件;
    /// </summary>
    [ProtoContract]
    public class GameObjectState
    {

        /// <summary>
        /// 位置;
        /// </summary>
        [ProtoMember(1)]
        public ProtoBuf_Vector3 Position;

        /// <summary>
        /// 旋转角度,欧拉角(保存此值;)
        /// </summary>
        [ProtoMember(2)]
        public ProtoBuf_Vector3 EulerAngles;

        /// <summary>
        /// 存在于AssetBundle内的预制名;
        /// </summary>
        [ProtoMember(3)]
        public string ObjectName;

        /// <summary>
        /// 旋转角度;
        /// </summary>
        [ProtoMember(4)]
        public Aspect Rotation;

        /// <summary>
        /// 旋转角度;四元数;
        /// </summary>
        public Quaternion quaternion
        {
            get { return Quaternion.Euler(EulerAngles); }
            set { EulerAngles = value.eulerAngles; }
        }

    }


}
