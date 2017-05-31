using KouXiaGu.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World
{

    /// <summary>
    /// 在控制台输出时间信息;
    /// </summary>
    [DisallowMultipleComponent]
    class OutputTimeSprite : MonoBehaviour, IObserver<IWorld>, IObserver<DateTime>
    {

        OutputTimeSprite()
        {
        }

        [SerializeField]
        Text textObject;

        [SerializeField]
        WorldInitializer world;

        IDisposable unsubscribe;

        void Start()
        {
            unsubscribe = world.Subscribe(this);
        }

        void IObserver<IWorld>.OnNext(IWorld world)
        {
            world.WorldData.Time.Subscribe(this);
        }

        void IObserver<DateTime>.OnNext(DateTime item)
        {
            string str = item.ToString();
            str += "\n" + item.GetMonthType();
            textObject.text = str;
        }

        public void OnError(Exception error)
        {
            return;
        }

        public void OnCompleted()
        {
            unsubscribe.Dispose();
        }

        [ContextMenu("123")]
        void Test()
        {
            IList<string> definedTags = new List<string>()
            {
                "as",
                "ass",
                "asss",
                "assss",
            };

            int mask = TagsToMask(definedTags, ToTags("ASSSs, as, 4"));
            Debug.Log(mask);
        }

        static readonly char[] separator = new char[]
            {
                  ',',
            };

        IEnumerable<string> ToTags(string tag)
        {
            tag = tag.ToLower();
            return tag.Split(separator, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim());
        }

        int TagsToMask(IList<string> definedTags, IEnumerable<string> tags)
        {
            int mask = 0;
            foreach (var tag in tags)
            {
                int index = definedTags.IndexOf(tag);
                if (index != -1)
                {
                    mask |= 1 << index;
                }
            }
            return mask;
        }
    }
}
