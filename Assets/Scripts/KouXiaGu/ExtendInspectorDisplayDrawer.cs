
namespace KouXiaGu
{
    using System;
    using UniRx;

#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(uLongReactiveProperty))]
    [UnityEditor.CustomPropertyDrawer(typeof(GameStatusReactiveProperty))]
    public class ExtendInspectorDisplayDrawer : UniRx.InspectorDisplayDrawer
    {
    }
#endif

    [Serializable]
    public class uLongReactiveProperty : ReactiveProperty<ulong>
    {
        public uLongReactiveProperty() : base() { }
        public uLongReactiveProperty(ulong initialValue) : base(initialValue) { }
    }

    [Serializable]
    public class GameStatusReactiveProperty : ReactiveProperty<GameStatus>
    {
        public GameStatusReactiveProperty() : base() { }
        public GameStatusReactiveProperty(GameStatus initialValue) : base(initialValue) { }
    }


}
