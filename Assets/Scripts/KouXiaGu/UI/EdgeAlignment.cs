using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.UI
{

    /// <summary>
    /// 边缘对齐模块;
    /// </summary>
    [Serializable]
    public class EdgeAlignment
    {
        [SerializeField]
        Vector2 offset = new Vector2(10, 10);
        internal List<RectTransform> Elements { get; private set; }
        List<float> tempOffsetList;

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        /// <summary>
        /// 订阅到;
        /// </summary>
        public IDisposable Subscribe(RectTransform transform)
        {
            if (Elements == null)
            {
                Elements = new List<RectTransform>();
            }
            if (tempOffsetList == null)
            {
                tempOffsetList = new List<float>();
            }

            Elements.Add(transform);
            Clamp(transform);
            return new CollectionUnsubscriber<RectTransform>(Elements, transform);
        }

        /// <summary>
        /// 进行对齐限制;
        /// </summary>
        public void Clamp(RectTransform transform)
        {
            Vector3 pos = transform.localPosition;
            pos.x += OffsetX(transform, Elements);
            pos.y += OffsetY(transform, Elements);
            transform.localPosition = pos;
        }

        float OffsetX(RectTransform transform, IEnumerable<RectTransform> elements)
        {
            Rect rect = transform.rect;
            Vector3 pos = transform.localPosition;
            foreach (var element in elements)
            {
                if (element == transform)
                {
                    continue;
                }

                Rect rectElement = element.rect;
                var elementPos = element.localPosition;

                float xMinOffset1 = (elementPos.x + rectElement.xMin) - (pos.x + rect.xMin);
                float xMinOffset2 = (elementPos.x + rectElement.xMax) - (pos.x + rect.xMin);
                float xMaxOffset1 = (elementPos.x + rectElement.xMin) - (pos.x + rect.xMax);
                float xMaxOffset2 = (elementPos.x + rectElement.xMax) - (pos.x + rect.xMax);
                tempOffsetList.Add(xMinOffset1);
                tempOffsetList.Add(xMinOffset2);
                tempOffsetList.Add(xMaxOffset1);
                tempOffsetList.Add(xMaxOffset2);
            }
            float min = tempOffsetList.MinSource(value => Math.Abs(value));
            tempOffsetList.Clear();
            return IsClose(min, Offset.x) ? min : 0;
        }

        float OffsetY(RectTransform transform, IEnumerable<RectTransform> elements)
        {
            Rect rect = transform.rect;
            Vector3 pos = transform.localPosition;
            foreach (var element in elements)
            {
                if (element == transform)
                {
                    continue;
                }

                Rect rectElement = element.rect;
                var elementPos = element.localPosition;

                float yMinOffset1 = (elementPos.y + rectElement.yMin) - (pos.y + rect.yMin);
                float yMinOffset2 = (elementPos.y + rectElement.yMax) - (pos.y + rect.yMin);
                float yMaxOffset1 = (elementPos.y + rectElement.yMin) - (pos.y + rect.yMax);
                float yMaxOffset2 = (elementPos.y + rectElement.yMax) - (pos.y + rect.yMax);
                tempOffsetList.Add(yMinOffset1);
                tempOffsetList.Add(yMinOffset2);
                tempOffsetList.Add(yMaxOffset1);
                tempOffsetList.Add(yMaxOffset2);
            }
            float min = tempOffsetList.MinSource(value => Math.Abs(value));
            tempOffsetList.Clear();
            return IsClose(min, Offset.y) ? min : 0;
        }

        bool IsClose(float value, float offset)
        {
            return value <= offset && value >= -offset;
        }
    }
}
