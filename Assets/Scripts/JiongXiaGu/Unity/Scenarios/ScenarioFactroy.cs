using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.Resources.BindingSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace JiongXiaGu.Unity.Scenarios
{


    public class ScenarioFactory
    {
        private Lazy<BindingSerializer<ScenarioResource>> scenarioSerializer = new Lazy<BindingSerializer<ScenarioResource>>();

        public ScenarioResource Read(Content content)
        {
            return scenarioSerializer.Value.Deserialize(content);
        }

        public void Write(Content content, ScenarioResource scenario)
        {
            scenarioSerializer.Value.Serialize(content, ref scenario);
        }


        private Lazy<XmlSerializer<ScenarioDescription>> descriptionSerializer = new Lazy<XmlSerializer<ScenarioDescription>>();
        public const string ScenarioDescriptionName = "ScenarioDescription.xml";

        public ScenarioInfo ReadInfo(string directory)
        {
            using (var content = new DirectoryContent(directory))
            {
                var description = ReadDescription(content);
                var contentInfo = new DirectoryContentInfo(directory);
                return new ScenarioInfo(contentInfo, description);
            }
        }

        public ScenarioInfo ReadZipInfo(string file)
        {
            using (var content = new SharpZipLibContent(file))
            {
                var description = ReadDescription(content);
                var contentInfo = new ZipContentInfo(file);
                return new ScenarioInfo(contentInfo, description);
            }
        }

        public ScenarioDescription ReadDescription(Content content)
        {
            using (var stream = content.GetInputStream(ScenarioDescriptionName))
            {
                var description = descriptionSerializer.Value.Deserialize(stream);
                return description;
            }
        }


        /// <summary>
        /// 枚举所有场景信息;
        /// </summary>
        public IEnumerable<ScenarioInfo> Searche(string scenariosDirectory)
        {
            return EnumerateDirectory(scenariosDirectory)
                .Concat(EnumerateZip(scenariosDirectory));
        }

        /// <summary>
        /// 枚举目录下所有 目录 类型的资源;
        /// </summary>
        public IEnumerable<ScenarioInfo> EnumerateDirectory(string scenariosDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var directory in Directory.EnumerateDirectories(scenariosDirectory, "*", searchOption))
            {
                string directoryName = Path.GetFileName(directory);
                if (!SearcheHelper.IsIgnore(directoryName))
                {
                    ScenarioInfo info;

                    try
                    {
                        info = ReadInfo(directory);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(string.Format("[读取剧情失败]Path : {0}, Exception : {1}", directory, ex));
                        continue;
                    }

                    yield return info;
                }
            }
        }

        /// <summary>
        /// 枚举目录下所有 压缩包 类型的资源;
        /// </summary>
        public IEnumerable<ScenarioInfo> EnumerateZip(string scenariosDirectory, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            foreach (var filePath in Directory.EnumerateFiles(scenariosDirectory, "*.zip", searchOption))
            {
                string fileName = Path.GetFileName(filePath);
                if (!SearcheHelper.IsIgnore(fileName))
                {
                    ScenarioInfo info;

                    try
                    {
                        info = ReadZipInfo(filePath);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning(string.Format("[读取剧情失败]Path : {0}, Exception : {1}", filePath, ex));
                        continue;
                    }

                    yield return info;
                }
            }
        }
    }
}
