﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Map;
using UnityEngine;

namespace KouXiaGu.Test
{

    [DisallowMultipleComponent]
    public class MoveTo2D : MonoBehaviour
    {

        [SerializeField]
        private Transform target;

        [SerializeField]
        private float maxDistanceDelta = 1;

        private Vector2 targetPosition;

        private void Awake()
        {
            target = target == null ? transform : target;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                targetPosition = BuildMap.GetMousePosition();
            }

            Vector2 newPosition = Vector2.MoveTowards(target.position, targetPosition, maxDistanceDelta);
            target.position = new Vector3(newPosition.x, newPosition.y, target.position.z);

        }

    }

}