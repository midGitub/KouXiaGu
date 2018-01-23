using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 预制物体脚本 消息窗口;
    /// </summary>
    public sealed class MessageWindow : Panel
    {
        private MessageWindow()
        {
        }

        [SerializeField]
        private bool isRaycastBlock;

        [SerializeField]
        private Image raycastBlock;

        [SerializeField]
        private RectTransform windowTransform;
        public RectTransform WindowTransform => windowTransform;

        [SerializeField]
        private Image titleBackgroupImage;
        public Image TitleBackgroupImage => titleBackgroupImage;

        [SerializeField]
        private Text titleMessageText;
        public Text TitleMessageText => titleMessageText;

        [SerializeField]
        private Image backgroupImage;
        public Image BackgroupImage => backgroupImage;

        [SerializeField]
        private Text messageText;
        public Text MessageText => messageText;

        [SerializeField]
        private LayoutGroup multipleChoices;
        public MultipleLayoutGroup MultipleChoices => new MultipleLayoutGroup(multipleChoices);

        public bool IsRaycastBlock
        {
            get { return raycastBlock.enabled; }
            set { raycastBlock.enabled = value; }
        }

        private void OnValidate()
        {
            IsRaycastBlock = isRaycastBlock;
        }

        protected override void InternalOnActivate()
        {
            if (isRaycastBlock)
            {
                IsRaycastBlock = true;
            }
        }

        protected override void InternalOnUnactivate()
        {
            IsRaycastBlock = false;
        }
    }
}
