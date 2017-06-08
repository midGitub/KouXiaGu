using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KouXiaGu.World;

namespace KouXiaGu.Cameras
{

    public class CanmeraTarget : MonoBehaviour, IStateObserver<IWorldComplete>
    {
        [Range(0.01f, 10)]
        public float MovementDistance = 0.1f;
        public Vector3 finalPosition;
        public Vector3 currentVelocity;
        IWorldComplete world;

        void Start()
        {
            enabled = false;
            finalPosition = transform.position;
            WorldSceneManager.OnWorldInitializeComplted.Subscribe(this);
        }

        void FixedUpdate()
        {
            MovementRespond(ref finalPosition);
            Vector3 pos = transform.position;
            pos = Vector3.SmoothDamp(pos, finalPosition, ref currentVelocity, 0.1f);
            pos.y = world.Components.Landform.GetHeight(pos);
            transform.position = pos;
        }

        void MovementRespond(ref Vector3 pos)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                pos.z += MovementDistance;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                pos.z -= MovementDistance;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= MovementDistance;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += MovementDistance;
            }
        }

        void IStateObserver<IWorldComplete>.OnCompleted(IWorldComplete item)
        {
            world = item;
            enabled = true;
        }

        void IStateObserver<IWorldComplete>.OnFailed(Exception ex)
        {
            return;
        }
    }
}
