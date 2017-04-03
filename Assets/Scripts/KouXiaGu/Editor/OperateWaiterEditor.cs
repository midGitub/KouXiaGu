using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace KouXiaGu
{

    [CustomEditor(typeof(OperateWaiter), true)]
    [CanEditMultipleObjects]
    public class OperateWaiterEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            OperateWaiter target = (OperateWaiter)this.target;

            EditorGUILayout.LabelField("剩余总数:" + target.Remainder);
            EditorGUILayout.LabelField("失败?", target.IsFaulted.ToString());
        }

    }

}
