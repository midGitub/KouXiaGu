using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KouXiaGu.Terrain3D;
using UnityEngine;

namespace KouXiaGu.Test
{

    /// <summary>
    /// 用于测试的角色脚本;
    /// </summary>
    [DisallowMultipleComponent]
    public class CharacterTest : MonoBehaviour
    {

        [SerializeField, Range(0.1f, 3f)]
        float movingSpeed = 0.5f;

        /// <summary>
        /// 中心点;
        /// </summary>
        Vector3 position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        void Update()
        {
            Move();
        }

        void Move()
        {
            Vector3 currentPosition = this.position;

            this.position = new Vector3(
                currentPosition.x + Input.GetAxis("Horizontal") * movingSpeed,
                Terrain3D.TerrainData.GetHeight(currentPosition), 
                currentPosition.z + Input.GetAxis("Vertical") * movingSpeed);
        }

    }

}
