//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace KouXiaGu.WorldEvents
//{

//    /// <summary>
//    /// 效果 组/合集;
//    /// </summary>
//    [Obsolete]
//    public class EffectGroup : ICollection<IEffect>, IEffect
//    {

//        public EffectGroup() : this(false)
//        {
//        }

//        public EffectGroup(bool effective)
//        {
//            this.effects = new List<IEffect>();
//            this.Effective = effective;
//        }

//        public EffectGroup(IEnumerable<IEffect> effects, bool effective)
//        {
//            this.effects = new List<IEffect>(effects);
//            SetEffective(effective);
//        }

//        /// <summary>
//        /// 获取到一个未启用的效果组副本;
//        /// </summary>
//        public EffectGroup(EffectGroup effects)
//        {
//            this.effects = new List<IEffect>(effects);
//            this.Effective = false;
//        }


//        /// <summary>
//        /// 效果合集;
//        /// </summary>
//        List<IEffect> effects;

//        /// <summary>
//        /// 效果是否启用?
//        /// </summary>
//        public bool Effective { get; private set; }

//        /// <summary>
//        /// 存在的效果总数;
//        /// </summary>
//        public int Count
//        {
//            get { return effects.Count; }
//        }

//        public bool IsReadOnly
//        {
//            get { return false; }
//        }


//        /// <summary>
//        /// 设置是否启用;
//        /// </summary>
//        public void SetEffective(bool effective)
//        {
//            if (effective)
//            {
//                Enable();
//            }
//            else
//            {
//                Disable();
//            }
//        }

//        /// <summary>
//        /// 启用所有效果;
//        /// </summary>
//        public void Enable()
//        {
//            if (!Effective)
//            {
//                foreach (var effect in effects)
//                {
//                    effect.Enable();
//                }
//                Effective = true;
//            }
//        }

//        /// <summary>
//        /// 停用所有效果;
//        /// </summary>
//        public void Disable()
//        {
//            if (Effective)
//            {
//                foreach (var effect in effects)
//                {
//                    effect.Disable();
//                }
//                Effective = false;
//            }
//        }


//        /// <summary>
//        /// 添加一个效果;
//        /// </summary>
//        public void Add(IEffect effect)
//        {
//            if (this.Effective)
//            {
//                effect.Enable();
//            }
//            effects.Add(effect);
//        }

//        /// <summary>
//        /// 移除这个效果;
//        /// </summary>
//        public bool Remove(IEffect effect)
//        {
//            if (effects.Remove(effect))
//            {
//                if (this.Effective)
//                {
//                    effect.Disable();
//                }
//                return true;
//            }
//            return false;
//        }

//        /// <summary>
//        /// 移除所有效果,并且清空;
//        /// </summary>
//        public void Clear()
//        {
//            Disable();
//            this.effects.Clear();
//        }

//        public bool Contains(IEffect item)
//        {
//            return this.effects.Contains(item);
//        }

//        public void CopyTo(IEffect[] array, int arrayIndex)
//        {
//            this.effects.CopyTo(array, arrayIndex);
//        }

//        public IEnumerator<IEffect> GetEnumerator()
//        {
//            return this.effects.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return this.effects.GetEnumerator();
//        }

//    }

//}
