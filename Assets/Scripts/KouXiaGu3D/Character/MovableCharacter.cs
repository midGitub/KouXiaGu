using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Grids;
using KouXiaGu.Navigation;
using KouXiaGu.Terrain3D;
using KouXiaGu.Terrain3D.Navigation;
using UnityEngine;

namespace KouXiaGu.Character
{

    [DisallowMultipleComponent]
    public class MovableCharacter : MonoBehaviour, IMovable
    {

        [SerializeField]
        float movingSpeed;

        public float MovingSpeed
        {
            get { return movingSpeed; }
            set { movingSpeed = value; }
        }

    }

}
