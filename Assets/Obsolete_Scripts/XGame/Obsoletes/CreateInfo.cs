//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using System.Xml.Serialization;
//using UnityEngine;

//namespace XGame.GameRunning
//{

//    /// <summary>
//    /// 预览创建方式;
//    /// </summary>
//    public enum CreatStyle
//    {

//        /// <summary>
//        /// 单个创建方式一;
//        /// </summary>
//        Single_1,

//        /// <summary>
//        /// 多个创建方式一;
//        /// </summary>
//        Multiple_1,

//    }

//    /// <summary>
//    /// 每个预制物体创建的方法;
//    /// </summary>
//    [Serializable]
//    public class CreateInfo /*: IXmlModule*/
//    {

//        #region 创建参数;

//        /// <summary>
//        /// 单个创建;
//        /// </summary>
//        [SerializeField]
//        public CreatStyle CreateStyle = CreatStyle.Single_1;

//        /// <summary>
//        /// 多个创建时长边最大值;
//        /// </summary>
//        [SerializeField]
//        public byte LimitLongestRow;

//        /// <summary>
//        /// 多个创建时短边最大值;
//        /// </summary>
//        [SerializeField]
//        public byte LimitShortestRow;

//        /// <summary>
//        /// 多个创建时的间隔;
//        /// </summary>
//        [SerializeField]
//        public IntVector2 spacing = new IntVector2(1, 1);

//        /// <summary>
//        /// 创建时是否允许旋转;
//        /// </summary>
//        [SerializeField]
//        public bool CanRotate;

//        #endregion

//    }

//}
