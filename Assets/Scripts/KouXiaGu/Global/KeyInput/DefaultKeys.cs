using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KouXiaGu.KeyInput
{

    /// <summary>
    /// 默认按键;
    /// </summary>
    public class DefaultKeys : Dictionary<int, KeyCode>, IReadOnlyDictionary<int, KeyCode>
    {

        static readonly DefaultKeys defaultKeys = new DefaultKeys()
        {
             { KeyFunction.Console_DisplayOrHide, KeyCode.BackQuote },

             { KeyFunction.Camera_movement_up, KeyCode.W },
             { KeyFunction.Camera_movement_down, KeyCode.S },
             { KeyFunction.Camera_movement_left, KeyCode.A },
             { KeyFunction.Camera_movement_right, KeyCode.D },
             { KeyFunction.Camera_return, KeyCode.Space },
        };

        static readonly IEnumerable<CustomKey> defaultKeyPairs;

        /// <summary>
        /// 默认按键合集;
        /// </summary>
        public static IReadOnlyDictionary<int, KeyCode> ReadOnlyKeys
        {
            get { return defaultKeys; }
        }

        IEnumerable<int> IReadOnlyDictionary<int, KeyCode>.Keys
        {
            get {  return Keys; }
        }

        IEnumerable<KeyCode> IReadOnlyDictionary<int, KeyCode>.Values
        {
            get { return Values; }
        }

        public void Add(KeyFunction func, KeyCode keyCode)
        {
            Add((int)func, keyCode);
        }




        //static DefaultKey()
        //{
        //    DefaultKeys = new ReadOnlyCollection<CustomKey>(defaultKeys);
        //}

        ///// <summary>
        ///// 只读的默认按键合集;
        ///// </summary>
        //public static ReadOnlyCollection<CustomKey> DefaultKeys { get; private set; }

        //static readonly Dictionary<int, KeyCode> defaultKeyDictionary = new Dictionary<int, KeyCode>()
        //{
        //    { (int)KeyFunction.Console_DisplayOrHide, KeyCode.BackQuote },

        //    { (int)KeyFunction.Camera_movement_up, KeyCode.W },
        //};

        //static CustomKey[] defaultKeys = new CustomKey[]
        //    {
        //        new CustomKey(KeyFunction.Console_DisplayOrHide, KeyCode.BackQuote),

        //        new CustomKey(KeyFunction.Camera_movement_up, KeyCode.W),
        //    };

    }

}
