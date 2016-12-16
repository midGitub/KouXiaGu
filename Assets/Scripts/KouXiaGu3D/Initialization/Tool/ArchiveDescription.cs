using System;
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using UnityEngine;

namespace KouXiaGu.Initialization
{

    /// <summary>
    /// 存档描述文件;
    /// </summary>
    public sealed class ArchiveDescription : MonoBehaviour
    {
        /// <summary>
        /// 存档文件名;
        /// </summary>
        const string FILE_NAME = "Description.xml";

        static readonly XmlSerializer descriptionSerializer = new XmlSerializer(typeof(Description));

        /// <summary>
        /// 将要保存的信息;
        /// </summary>
        public static Description Activated { get; private set; }

        /// <summary>
        /// 设置下次保存时的描述文件信息;
        /// </summary>
        public static void SetDescription(object sender, Description description)
        {
            Activated = description;
        }

        /// <summary>
        /// 保存现在的描述信息到文件夹内;
        /// </summary>
        public static void Save(string directoryPath)
        {
            string filePath = GetFilePath(directoryPath);
            descriptionSerializer.Serialize(filePath, Activated);
        }

        /// <summary>
        /// 从文件夹读取到;
        /// </summary>
        public static Description Load(string directoryPath)
        {
            string filePath = GetFilePath(directoryPath);
            Description description = (Description)descriptionSerializer.Deserialize(filePath);
            return description;
        }

        static string GetFilePath(string directoryPath)
        {
            string filePath = Path.Combine(directoryPath, FILE_NAME);
            return filePath;
        }


        void Awake()
        {
            ArchiveStage.Subscribe(ArchiveObserver.Instance);
        }


        [XmlType("Description")]
        public struct Description
        {

            /// <summary>
            /// 存档名;
            /// </summary>
            [XmlAttribute("name")]
            public string Name;

            /// <summary>
            /// 保存存档的真实时间;
            /// </summary>
            [XmlElement("Time")]
            public long Time;

            /// <summary>
            /// 用户留言;
            /// </summary>
            [XmlElement("Message")]
            public string Message;

        }


        class ArchiveObserver : IStageObserver<ArchiveFile>
        {
            public static readonly ArchiveObserver Instance = new ArchiveObserver();

            static ArchiveObserver()
            {
                Activated = DefalutDescription;
            }

            /// <summary>
            /// 返回一个默认的描述;
            /// </summary>
            public static Description DefalutDescription
            {
                get
                {
                    return new Description()
                    {
                        Name = "Default",
                        Time = DateTime.Now.Ticks,
                        Message = "NULL",
                    };
                }
            }

            IEnumerator IStageObserver<ArchiveFile>.OnEnter(ArchiveFile item)
            {
                Save(item.DirectoryPath);
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnLeave(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnEnterRollBack(ArchiveFile item)
            {
                yield break;
            }

            IEnumerator IStageObserver<ArchiveFile>.OnLeaveRollBack(ArchiveFile item)
            {
                yield break;
            }

            void IStageObserver<ArchiveFile>.OnEnterCompleted()
            {
                Activated = DefalutDescription;
            }

            void IStageObserver<ArchiveFile>.OnLeaveCompleted()
            {
                return;
            }
        }

    }

}
