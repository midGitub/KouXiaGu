//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;

//namespace JiongXiaGu.UI
//{

//    /// <summary>
//    /// 边缘对齐模块;
//    /// </summary>
//    [DisallowMultipleComponent]
//    public sealed class EdgeAlignment : MonoBehaviour
//    {
//        EdgeAlignment()
//        {
//        }

//        /// <summary>
//        /// 相吸距离;
//        /// </summary>
//        [SerializeField]
//        Vector2 magnetism = new Vector2(10, 10);
//        internal List<RectTransform> Elements { get; private set; }

//        /// <summary>
//        /// 相吸距离;
//        /// </summary>
//        public Vector2 Magnetism
//        {
//            get { return magnetism; }
//            set { magnetism = value; }
//        }

//        /// <summary>
//        /// 订阅到;
//        /// </summary>
//        public IDisposable Subscribe(RectTransform transform)
//        {
//            if (Elements == null)
//            {
//                Elements = new List<RectTransform>();
//            }
//            Elements.Add(transform);
//            Clamp(transform);
//            return new CollectionUnsubscriber<RectTransform>(Elements, transform);
//        }

//        /// <summary>
//        /// 进行对齐限制;
//        /// </summary>
//        public void Clamp(RectTransform transform)
//        {
//            Vector3 pos = transform.localPosition;
//            pos.x += OffsetX(transform, Elements);
//            pos.y += OffsetY(transform, Elements);
//            transform.localPosition = pos;
//        }

//        float OffsetX(RectTransform transform, IEnumerable<RectTransform> elements)
//        {
//            Rect rect = transform.rect;
//            Vector3 pos = transform.localPosition;
//            float min = Magnetism.x;
//            float value = default(float);
//            foreach (var element in elements)
//            {
//                if (element == transform)
//                {
//                    continue;
//                }
//                Rect rectElement = element.rect;
//                var elementPos = element.localPosition;
//                float xMinOffset1 = (elementPos.x + rectElement.xMin) - (pos.x + rect.xMin);
//                float xMinOffset2 = (elementPos.x + rectElement.xMax) - (pos.x + rect.xMin);
//                float xMaxOffset1 = (elementPos.x + rectElement.xMin) - (pos.x + rect.xMax);
//                float xMaxOffset2 = (elementPos.x + rectElement.xMax) - (pos.x + rect.xMax);
//                Min(ref min, ref value, xMinOffset1);
//                Min(ref min, ref value, xMinOffset2);
//                Min(ref min, ref value, xMaxOffset1);
//                Min(ref min, ref value, xMaxOffset2);
//            }
//            return IsClose(min, Magnetism.x) ? value : 0;
//        }

//        float OffsetY(RectTransform transform, IEnumerable<RectTransform> elements)
//        {
//            Rect rect = transform.rect;
//            Vector3 pos = transform.localPosition;
//            float min = Magnetism.y;
//            float value = default(float);
//            foreach (var element in elements)
//            {
//                if (element == transform)
//                {
//                    continue;
//                }
//                Rect rectElement = element.rect;
//                var elementPos = element.localPosition;
//                float yMinOffset1 = (elementPos.y + rectElement.yMin) - (pos.y + rect.yMin);
//                float yMinOffset2 = (elementPos.y + rectElement.yMax) - (pos.y + rect.yMin);
//                float yMaxOffset1 = (elementPos.y + rectElement.yMin) - (pos.y + rect.yMax);
//                float yMaxOffset2 = (elementPos.y + rectElement.yMax) - (pos.y + rect.yMax);
//                Min(ref min, ref value, yMinOffset1);
//                Min(ref min, ref value, yMinOffset2);
//                Min(ref min, ref value, yMaxOffset1);
//                Min(ref min, ref value, yMaxOffset2);
//            }
//            return IsClose(min, Magnetism.y) ? value : 0;
//        }

//        void Min(ref float min, ref float originalValue, float value)
//        {
//            var absValue = Math.Abs(value);
//            if (min > absValue)
//            {
//                min = absValue;
//                originalValue = value;
//            }
//        }

//        bool IsClose(float value, float offset)
//        {
//            return value < offset && value > -offset;
//        }
//    }
//}
