using JiongXiaGu.Unity.Resources;
using JiongXiaGu.Unity.RunTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace JiongXiaGu.Unity.UI
{

    [DisallowMultipleComponent]
    public class UIModificationController : MessageWindow
    {
        private UIModificationController()
        {
        }

        [SerializeField]
        private UIModificationItem uIModificationItemPrefab;
        [SerializeField]
        private ToggleGroup toggleGroupObject;
        [SerializeField]
        private Transform activeModificationContent;
        [SerializeField]
        private Transform idleModificationContent;
        [SerializeField]
        private UIModificationInfoStruct uIModificationInfo;

        public UIModificationItem SelectTarget { get; private set; }

        private void Start()
        {
            Refresh();
        }

        public void SetTargetActive()
        {
            if (activeModificationContent == null)
                throw new ArgumentNullException(nameof(activeModificationContent));

            TargetMoveTo(activeModificationContent);
        }

        public void SetTargetIdle()
        {
            if (idleModificationContent == null)
                throw new ArgumentNullException(nameof(idleModificationContent));

            TargetMoveTo(idleModificationContent);
        }

        private void TargetMoveTo(Transform parent)
        {
            if (SelectTarget != null)
            {
                Transform targetTransform = SelectTarget.transform;
                if (targetTransform.parent != parent)
                {
                    targetTransform.SetParent(parent, false);
                }
            }
        }

        public void TargetMoveUp()
        {
            if (SelectTarget != null)
            {
                Transform targetTransform = SelectTarget.transform;
                var siblingIndex = targetTransform.GetSiblingIndex();
                if (siblingIndex > 0)
                {
                    siblingIndex--;
                    targetTransform.SetSiblingIndex(siblingIndex);
                }
            }
        }

        public void TargetMoveDown()
        {
            if (SelectTarget != null)
            {
                Transform targetTransform = SelectTarget.transform;
                var siblingIndex = targetTransform.GetSiblingIndex();
                if (++siblingIndex < targetTransform.parent.childCount)
                {
                    targetTransform.SetSiblingIndex(siblingIndex);
                }
            }
        }

        internal void OnSelected(UIModificationItem uiItem)
        {
            if (uiItem == null)
                throw new ArgumentNullException(nameof(uiItem));

            SelectTarget = uiItem;
            ModificationInfo info = uiItem.ModificationInfo;
            uIModificationInfo.SetDescription(info);
        }

        /// <summary>
        /// 刷新UI;
        /// </summary>
        public void Refresh()
        {
            SetActiveModificationList(ModificationController.ActivatedModificationInfos);

            var idleModificationInfos = new List<ModificationInfo>();
            ModificationController.GetIdleModificationInfos(idleModificationInfos);
            SetIdleModificationList(idleModificationInfos);
        }

        public void WriteConfig()
        {
            var activeModificationID = EnumerateActiveModificationInfo().Select(item => item.Description.ID);
            var activeModification = new ModificationLoadOrder(activeModificationID);

            ModificationLoadOrderSerializer activeModificationSerializer = new ModificationLoadOrderSerializer();

            using (Resource.UserConfigContent.BeginUpdate())
            {
                activeModificationSerializer.Serialize(Resource.UserConfigContent, activeModification);
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetActiveModificationList(IEnumerable<ModificationInfo> descriptions)
        {
            SetModificationList(activeModificationContent, descriptions);
        }

        public void SetIdleModificationList(IEnumerable<ModificationInfo> descriptions)
        {
            SetModificationList(idleModificationContent, descriptions);
        }

        private void SetModificationList(Transform parent, IEnumerable<ModificationInfo> descriptions)
        {
            if (parent.childCount > 0)
            {
                foreach (Transform child in parent)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }

            foreach (var description in descriptions)
            {
                CreateAt(parent, description);
            }
        }


        /// <summary>
        /// 创建一个条目到激活列表;
        /// </summary>
        public void CreateAsActive(ModificationInfo info)
        {
            CreateAt(activeModificationContent, info);
        }

        /// <summary>
        /// 创建一个条目到失效列表;
        /// </summary>
        /// <param name="info"></param>
        public void CreateAsIdle(ModificationInfo info)
        {
            CreateAt(idleModificationContent, info);
        }

        private void CreateAt(Transform parent, ModificationInfo info)
        {
            var item = Instantiate(uIModificationItemPrefab, parent);
            item.Controller = this;
            item.SetDescription(info);
            item.ToggleObject.group = toggleGroupObject;
        }


        /// <summary>
        /// 按排列顺序枚举所有选中列表中的模组名;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateActiveModificationInfo()
        {
            return EnumerateModificationInfo(activeModificationContent);
        }

        /// <summary>
        /// 按排列顺序枚举所以未选中列表中的模组名;
        /// </summary>
        public IEnumerable<ModificationInfo> EnumerateIdleModificationInfo()
        {
            return EnumerateModificationInfo(idleModificationContent);
        }

        /// <summary>
        /// 按排列顺序枚举所有模组名;
        /// </summary>
        private IEnumerable<ModificationInfo> EnumerateModificationInfo(Transform parent)
        {
            foreach (Transform child in parent)
            {
                var modItem = child.GetComponent<UIModificationItem>();
                if (modItem != null)
                {
                    yield return modItem.ModificationInfo;
                }
            }
        }
    }
}
