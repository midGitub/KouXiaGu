using JiongXiaGu.Unity.Resources;
using System;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{
    [Serializable]
    internal struct UIModificationInfoStruct
    {
        public InputField NameLabel;
        public InputField AuthorLabel;
        public InputField VersionLabel;
        public InputField MessageLabel;

        public void SetDescription(ModificationDescription description)
        {
            NameLabel.text = description.Name;
            AuthorLabel.text = description.Author;
            VersionLabel.text = description.Version;
            MessageLabel.text = description.Message;
        }

        public void Clear()
        {
            NameLabel.text = string.Empty;
            AuthorLabel.text = string.Empty;
            VersionLabel.text = string.Empty;
            MessageLabel.text = string.Empty;
        }
    }
}
