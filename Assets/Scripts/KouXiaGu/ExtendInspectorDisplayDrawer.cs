using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;

namespace KouXiaGu
{


    [UnityEditor.CustomPropertyDrawer(typeof(GameStatusReactiveProperty))]
    public class ExtendInspectorDisplayDrawer : InspectorDisplayDrawer
    {
    }


    [Serializable]
    public class GameStatusReactiveProperty : ReactiveProperty<GameStatus>
    {
        public GameStatusReactiveProperty()
            : base()
        {

        }

        public GameStatusReactiveProperty(GameStatus initialValue)
            : base(initialValue)
        {

        }
    }


}
