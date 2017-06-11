using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KouXiaGu.World
{

    public class WorldTimeDisplay : MonoBehaviour , IStateObserver<IWorldComplete>
    {
        public Text textObject;
        WorldTime time;

        void Start()
        {
            enabled = false;
            WorldSceneManager.OnWorldInitializeComplted.Subscribe(this);
        }

        void Update()
        {
            string text = time.CurrentTime.ToString();
            textObject.text = text;
        }

        void IStateObserver<IWorldComplete>.OnCompleted(IWorldComplete item)
        {
            time = item.Components.Time;
            enabled = true;
        }

        void IStateObserver<IWorldComplete>.OnFailed(Exception ex)
        {
            return;
        }
    }
}
