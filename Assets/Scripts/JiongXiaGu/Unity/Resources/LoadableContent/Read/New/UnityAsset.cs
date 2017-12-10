using System;
using UnityEngine;
using System.Threading.Tasks;

namespace JiongXiaGu.Unity.Resources
{

    internal class UnityAsset<T> : WeakReferenceObject<T>
        where T : UnityEngine.Object
    {
        public UnityAsset(T value) : base(value)
        {
        }
    }
}
