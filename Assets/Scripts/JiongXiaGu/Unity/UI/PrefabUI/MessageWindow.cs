using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    /// <summary>
    /// 消息窗口基类;
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class MessageWindow : MonoBehaviour, IPanel
    {
        protected MessageWindow()
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

        public virtual void OnActivate()
        {
            if (isRaycastBlock)
            {
                IsRaycastBlock = true;
            }
        }

        public virtual void OnUnactivate()
        {
            IsRaycastBlock = false;
        }
    }
}
