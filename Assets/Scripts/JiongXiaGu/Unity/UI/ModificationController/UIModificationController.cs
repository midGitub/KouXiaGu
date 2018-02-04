using JiongXiaGu.Unity.Resources;
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
    public class UIModificationController : MonoBehaviour
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
            string modificationName = uiItem.Description.ID;
            ModificationInfo info;
            if (Modification.TryGetInfo(modificationName, out info))
            {
                uIModificationInfo.SetDescription(info.Description);
            }
            else
            {
                uIModificationInfo.Clear();
            }
        }

        /// <summary>
        /// 刷新UI;
        /// </summary>
        public void Refresh()
        {
            var activeModificationInfos = Modification.GetActiveModificationInfos();
            var activeDescriptions = activeModificationInfos.Select(item => item.Description);
            SetActiveModificationList(activeDescriptions);

            var idleModificationInfos = Modification.GetIdleModificationInfos(activeModificationInfos);
            var idleDescriptions = idleModificationInfos.Select(item => item.Description);
            SetIdleModificationList(idleDescriptions);
        }

        public void WriteConfig()
        {
            var activeModificationID = EnumerateActiveModificationInfo().Select(item => item.ID);
            var activeModification = new ActiveModification(activeModificationID);

            ActiveModificationSerializer activeModificationSerializer = new ActiveModificationSerializer();

            using (Resource.UserConfigContent.BeginUpdate())
            {
                activeModificationSerializer.Serialize(Resource.UserConfigContent, activeModification);
            }
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void SetActiveModificationList(IEnumerable<ModificationDescription> descriptions)
        {
            SetModificationList(activeModificationContent, descriptions);
        }

        public void SetIdleModificationList(IEnumerable<ModificationDescription> descriptions)
        {
            SetModificationList(idleModificationContent, descriptions);
        }

        private void SetModificationList(Transform parent, IEnumerable<ModificationDescription> descriptions)
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
        public void CreateAsActive(ModificationDescription description)
        {
            CreateAt(activeModificationContent, description);
        }

        /// <summary>
        /// 创建一个条目到失效列表;
        /// </summary>
        /// <param name="info"></param>
        public void CreateAsIdle(ModificationDescription description)
        {
            CreateAt(idleModificationContent, description);
        }

        private void CreateAt(Transform parent, ModificationDescription description)
        {
            var item = Instantiate(uIModificationItemPrefab, parent);
            item.Controller = this;
            item.SetDescription(description);
            item.ToggleObject.group = toggleGroupObject;
        }


        /// <summary>
        /// 按排列顺序枚举所有选中列表中的模组名;
        /// </summary>
        public IEnumerable<ModificationDescription> EnumerateActiveModificationInfo()
        {
            return EnumerateModificationInfo(activeModificationContent);
        }

        /// <summary>
        /// 按排列顺序枚举所以未选中列表中的模组名;
        /// </summary>
        public IEnumerable<ModificationDescription> EnumerateIdleModificationInfo()
        {
            return EnumerateModificationInfo(idleModificationContent);
        }

        /// <summary>
        /// 按排列顺序枚举所有模组名;
        /// </summary>
        private IEnumerable<ModificationDescription> EnumerateModificationInfo(Transform parent)
        {
            foreach (Transform child in parent)
            {
                var modItem = child.GetComponent<UIModificationItem>();
                if (modItem != null)
                {
                    yield return modItem.Description;
                }
            }
        }
    }
}
